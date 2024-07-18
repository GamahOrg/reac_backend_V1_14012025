using Core.Dtos;
using Core.GenericRepository.BaseRepository;
using Core.Security.Contracts;
using Core.Shared.Responses;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using REAC.Application.AppServices.Integration.Hubs;
using REAC.Application.Contracts.Dtos.Integration.GEE;
using REAC.Application.Contracts.Dtos.Integration.Nafath;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Application.Contracts.IAppServices.Integration.Hubs.Contracts;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Integration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace REAC.Application.AppServices.Integration.GEE
{
    public class NafathService : INafathService
    {
        private readonly ISysSettingService _sysSetting;
        private readonly IAccountService _accountService;
        private readonly IEntityRepository _repository;
        private readonly IGEELoginService _gEELoginService;
        private readonly IHubContext<IdentityHub, IIdentityClient> _identityHub;

        public NafathService(ISysSettingService sysSetting, IEntityRepository repository, IGEELoginService gEELoginService
            , IHubContext<IdentityHub, IIdentityClient> identityHub, IAccountService accountService)
        {
            _sysSetting = sysSetting;
            _repository = repository;
            _gEELoginService = gEELoginService;
            _identityHub = identityHub;
            _accountService = accountService;
        }

        public async Task<DataResponse<NafathResponseDto>> Nafath(NafathRequestDto nafathRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                if (nafathRequestDto is null)
                    return new DataResponse<NafathResponseDto> { IsDone = false, Message = "" };

                if (!nafathRequestDto.IsRegister)
                {
                    UserLoginDto UserLogin = new UserLoginDto()
                    { UserName = nafathRequestDto.NationalId, Password = nafathRequestDto.Password };
                    var login = await _accountService.LoginAsync(UserLogin, cancellationToken);
                    if (!login.IsDone || login == null)
                        return new DataResponse<NafathResponseDto> { IsDone = false, Message = login?.Message ?? string.Empty };
                }

                var token = (await _gEELoginService.Login()).Data?.Access_token;
                if (token is null)
                    return new DataResponse<NafathResponseDto> { IsDone = false, Message = "" };

                var requestBody = await GetNafathSettingsData(nafathRequestDto.NationalId);
                if (!requestBody.IsDone || requestBody.Data is null)
                    return new DataResponse<NafathResponseDto>() { IsDone = false, Message = "" };

                var url = _sysSetting.GetByIntegrationKey(SysSettingKeyConstants.Nafath_URL).Result.Data?.FirstOrDefault()?.Value;
                if (string.IsNullOrEmpty(url))
                    return new DataResponse<NafathResponseDto>() { IsDone = false, Message = "" };

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", $"Bearer {token}");
                var body = JsonConvert.SerializeObject(requestBody.Data);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                NafathResponseDto? nafathResponse = new NafathResponseDto();
                if (response.IsSuccessStatusCode)
                {
                    nafathResponse.Data = await response.Content.ReadFromJsonAsync<NafathResponseDataDto>();
                    var checkNationalIdIsExist = (await _repository.GetQueryable<NafathTransaction, Guid>()).Any(x => x.NationalId == nafathRequestDto.NationalId);
                    if (!checkNationalIdIsExist && nafathResponse.Data is not null && nafathResponse.Data.Random is not null && nafathResponse.Data.TransId is not null)
                    {
                        NafathTransaction nafathTransaction = new NafathTransaction()
                        {
                            NationalId = nafathRequestDto.NationalId,
                            RandomNumber = nafathResponse.Data.Random,
                            TransactionId = nafathResponse.Data.TransId,
                            RequestExpired = DateTime.Now.AddMinutes(2),
                        };
                        await _repository.AddAsync<NafathTransaction, Guid>(nafathTransaction);
                        var result = await _repository.SaveChangesAsync();
                        if (!result.IsDone)
                            return new DataResponse<NafathResponseDto> { IsDone = false, Message = result.Message }; ;
                    }
                    return new DataResponse<NafathResponseDto> { IsDone = true, Data = nafathResponse };
                }
                else
                {
                    nafathResponse.Error = await response.Content.ReadFromJsonAsync<NafathResponseErrorDto>();
                    return new DataResponse<NafathResponseDto> { IsDone = false, Data = nafathResponse };
                }
            }
            catch (Exception e)
            {
                return new DataResponse<NafathResponseDto>() { IsDone = false, Message = e.Message + e.InnerException };
            }
        }

        public async Task<DataResponse<NafathUserInfoDto>> CheckNafathStatus()
        {
            var nafathTransactions = (await _repository.GetQueryable<NafathTransaction, Guid>()).ToList();
            if (nafathTransactions is null || !nafathTransactions.Any())
                return new DataResponse<NafathUserInfoDto> { IsDone = false, Message = "" };

            var token = (await _gEELoginService.Login()).Data?.Access_token;
            if (token is null)
                return new DataResponse<NafathUserInfoDto> { IsDone = false, Message = "" };

            foreach (var nafathTransaction in nafathTransactions)
            {
                if (nafathTransaction.RequestExpired < DateTime.Now)
                {
                    _repository.Remove<NafathTransaction, Guid>(nafathTransaction);
                    var result = await _repository.SaveChangesAsync();
                    if (!result.IsDone) return new DataResponse<NafathUserInfoDto> { IsDone = false, Message = result.Message };

                    IdentityStatus identityStatus = new IdentityStatus()
                    {
                        TransactionId = nafathTransaction.TransactionId,
                        IsValid = false,
                    };
                    await _identityHub.Clients.Group(identityStatus.TransactionId).ReceivedIdentityStatus(identityStatus);
                    continue;
                }

                NafathCheckRequestDto nafathCheckRequestDto = new NafathCheckRequestDto()
                {
                    Id = nafathTransaction.NationalId,
                    Random = nafathTransaction.RandomNumber,
                    TransId = nafathTransaction.TransactionId,
                    Token = token
                };

                var nafathCheckStatusResponse = (await NafathCheckStatus(nafathCheckRequestDto));
                if (nafathCheckStatusResponse is not null && nafathCheckStatusResponse.Data is not null)
                {
                    if (nafathCheckStatusResponse.Data?.Data?.Status == SysSettingKeyConstants.NafathStatus_COMPLETED
                        && nafathCheckStatusResponse.Data?.Data?.AccessToken is not null)
                    {
                        IdentityStatus identityStatus = new IdentityStatus()
                        {
                            TransactionId = nafathTransaction.TransactionId,
                            IsValid = true,
                        };
                        await _identityHub.Clients.Group(identityStatus.TransactionId).ReceivedIdentityStatus(identityStatus);

                        _repository.Remove<NafathTransaction, Guid>(nafathTransaction);
                        var result = await _repository.SaveChangesAsync();
                        if (!result.IsDone) return (DataResponse<NafathUserInfoDto>)result;// new DataResponse<NafathCheckResponseDto> { IsDone = false, Message = result.Message };

                        var userInfo = NafathGetUserInfo(nafathCheckStatusResponse.Data.Data.AccessToken).Result;
                        if (!userInfo.IsDone)
                            return new DataResponse<NafathUserInfoDto> { IsDone = false, Message = "" };

                        return new DataResponse<NafathUserInfoDto> { IsDone = true, Data = userInfo.Data };
                    }

                    if (nafathCheckStatusResponse.Data?.Data?.Status == SysSettingKeyConstants.NafathStatus_REJECTED
                    || nafathCheckStatusResponse.Data?.Data?.Status == SysSettingKeyConstants.NafathStatus_EXPIRED)
                    {
                        IdentityStatus identityStatus = new IdentityStatus()
                        {
                            TransactionId = nafathTransaction.TransactionId,
                            IsValid = false,
                        };
                        await _identityHub.Clients.Group(identityStatus.TransactionId).ReceivedIdentityStatus(identityStatus);

                        _repository.Remove<NafathTransaction, Guid>(nafathTransaction);
                        var result = await _repository.SaveChangesAsync();
                        if (!result.IsDone) return new DataResponse<NafathUserInfoDto> { IsDone = false, Message = result.Message };
                    }
                }
            }
            return new DataResponse<NafathUserInfoDto> { IsDone = false, Message = "" };
        }



        private async Task<DataResponse<NafathUserInfoDto>> NafathGetUserInfo(string token)
        {
            try
            {
                var url = _sysSetting.GetBySettingKey(SysSettingKeyConstants.NafathGetUserInfo_URL).Result.Data?.Value;

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(url))
                    return new DataResponse<NafathUserInfoDto>() { IsDone = false, Message = "" };

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", $"Bearer {token}");
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jwtToken = await response.Content.ReadAsStringAsync();
                    var handler = new JwtSecurityTokenHandler();
                    var readJwtToken = handler.ReadJwtToken(jwtToken);
                    var serializePayloadToJsonObj = readJwtToken.Payload.SerializeToJson();
                    var userInfo = JsonSerializer.Deserialize<NafathUserInfoDto>(serializePayloadToJsonObj);

                    return new DataResponse<NafathUserInfoDto> { IsDone = true, Data = userInfo };
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return new DataResponse<NafathUserInfoDto> { IsDone = false, Message = "" };
                else
                    return new DataResponse<NafathUserInfoDto> { IsDone = false, Message = "" };
            }
            catch (Exception e)
            {
                return new DataResponse<NafathUserInfoDto>() { IsDone = false, Message = e.Message + e.InnerException };
            }
        }

        private async Task<DataResponse<NafathCheckResponseDto>> NafathCheckStatus(NafathCheckRequestDto nafathCheckRequestDto)
        {
            try
            {
                var settings = await _sysSetting.GetByIntegrationKey(SysSettingKeyConstants.NafathCheck_Key);
                if (nafathCheckRequestDto is null || settings is null || !settings.IsDone || settings.Data is null)
                    return new DataResponse<NafathCheckResponseDto>() { IsDone = false, Message = "" };

                var url = settings.Data.FirstOrDefault(x => x.Key == SysSettingKeyConstants.NafathCheck_URL)?.Value;

                NafathCheckBodyDto nafathCheckBodyDto = new NafathCheckBodyDto()
                {
                    Action = settings.Data.FirstOrDefault(x => x.Key == SysSettingKeyConstants.NafathCheck_Action).Value,
                    Parameters = new NafathTransactionDto()
                    {
                        id = nafathCheckRequestDto.Id,
                        random = nafathCheckRequestDto.Random,
                        transId = nafathCheckRequestDto.TransId
                    },
                };


                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", $"Bearer {nafathCheckRequestDto.Token}");
                var body = JsonConvert.SerializeObject(nafathCheckBodyDto);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                var x = await response.Content.ReadAsStringAsync();
                NafathCheckResponseDto? nafathCheckResponse = new NafathCheckResponseDto();
                if (response.IsSuccessStatusCode)
                {
                    nafathCheckResponse.Data = await response.Content.ReadFromJsonAsync<NafathCheckResponseDataDto>();
                    return new DataResponse<NafathCheckResponseDto> { IsDone = true, Data = nafathCheckResponse };
                }
                else
                {
                    nafathCheckResponse.Error = await response.Content.ReadFromJsonAsync<NafathResponseErrorDto>();
                    return new DataResponse<NafathCheckResponseDto> { IsDone = false, Data = nafathCheckResponse };
                }
            }
            catch (Exception e)
            {
                return new DataResponse<NafathCheckResponseDto>() { IsDone = false, Message = e.Message + e.InnerException };
            }
        }

        private async Task<DataResponse<NafathRequestBodyDto>> GetNafathSettingsData(string nationalId)
        {
            var settings = await _sysSetting.GetByIntegrationKey(SysSettingKeyConstants.Nafath_Key);

            if (settings is null || !settings.IsDone || settings.Data is null || string.IsNullOrEmpty(nationalId))
                return new DataResponse<NafathRequestBodyDto>() { IsDone = false, Message = "" };

            NafathRequestBodyDto nafathRequestBodyDto = new NafathRequestBodyDto()
            {
                Action = settings.Data.FirstOrDefault(x => x.Key == SysSettingKeyConstants.Nafath_Action).Value,
                Parameters = new NafathRequestParametersBodyDto()
                {
                    service = settings.Data.FirstOrDefault(x => x.Key == SysSettingKeyConstants.Nafath_Service).Value,
                    id = nationalId,
                },
            };
            return new DataResponse<NafathRequestBodyDto> { Data = nafathRequestBodyDto, IsDone = true };
        }

    }
}
