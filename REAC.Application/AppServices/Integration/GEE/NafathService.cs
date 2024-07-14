using Core.Shared.Responses;
using Newtonsoft.Json;
using REAC.Application.Contracts.IAppServices.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using REAC.Application.Contracts.Dtos.Integration.GEE;
using REAC.Domain.Shared.Constants;
using static Dapper.SqlMapper;
using System.Linq.Dynamic.Core.Tokenizer;
using REAC.Application.Contracts.IAppServices.Integration;
using Core.Dtos;
using Microsoft.Extensions.Options;
using REAC.EntityFrameworkCore.Entities.Integration;
using Core.GenericRepository.BaseRepository;
using REAC.Application.Contracts.Dtos.Integration.Nafath;
using REAC.EntityFrameworkCore.Entities.System;
using Microsoft.AspNetCore.SignalR;
using REAC.Application.AppServices.Integration.Hubs;
using REAC.Application.Contracts.IAppServices.Integration.Hubs.Contracts;


namespace REAC.Application.AppServices.Integration.GEE
{
    public class NafathService : INafathService
    {
        private readonly ISysSettingService _sysSetting;
        private readonly IEntityRepository _repository;
        private readonly IGEELoginService _gEELoginService;
        private readonly IHubContext<IdentityHub, IIdentityClient> _identityHub;

        public NafathService(ISysSettingService sysSetting, IEntityRepository repository, IGEELoginService gEELoginService
            , IHubContext<IdentityHub, IIdentityClient> identityHub)
        {
            _sysSetting = sysSetting;
            _repository = repository;
            _gEELoginService = gEELoginService;
            _identityHub = identityHub;
        }

        public async Task<DataResponse<NafathResponseDto>> Nafath(NafathRequestDto nafathRequestDto)
        {
            try
            {
                var requestBody = await GetNafathSettingsData(nafathRequestDto.NationalId);
                if (!requestBody.IsDone || requestBody.Data is null)
                    return new DataResponse<NafathResponseDto>() { IsDone = false, Message = "" };

                var url = _sysSetting.GetByIntegrationKey(SysSettingKeyConstants.Nafath_URL).Result.Data?.FirstOrDefault()?.Value;
                if (string.IsNullOrEmpty(url))
                    return new DataResponse<NafathResponseDto>() { IsDone = false, Message = "" };

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", $"Bearer {nafathRequestDto.Token}");
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

        public async Task<DataResponse<NafathCheckResponseDto>> CheckNafathStatus()
        {
            var nafathTransactions = (await _repository.GetQueryable<NafathTransaction, Guid>()).ToList();
            if(nafathTransactions is null || !nafathTransactions.Any())
                return new DataResponse<NafathCheckResponseDto> { IsDone = false, Message = ""};

            var token = (await _gEELoginService.Login()).Data?.Access_token;
            if(token is null)
                return new DataResponse<NafathCheckResponseDto> { IsDone = false, Message = "" };

            foreach (var nafathTransaction in nafathTransactions)
            {
                if(nafathTransaction.RequestExpired < DateTime.Now)
                {
                    var x = _repository.Remove<NafathTransaction, Guid>(nafathTransaction);
                    var result = await _repository.SaveChangesAsync();
                    if(!result.IsDone) return new DataResponse<NafathCheckResponseDto> { IsDone = false, Message = result.Message };

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

                var nafathResponse = (await NafathCheckStatus(nafathCheckRequestDto));
                if (nafathResponse is not null && nafathResponse.Data is not null)
                {
                    if (nafathResponse.Data?.Data?.Status == SysSettingKeyConstants.NafathStatus_COMPLETED && nafathResponse.Data?.Data?.AccessToken is not null)
                    {
                        IdentityStatus identityStatus = new IdentityStatus()
                        {
                            TransactionId = nafathTransaction.TransactionId,
                            IsValid = true,
                        };
                        await _identityHub.Clients.Group(identityStatus.TransactionId).ReceivedIdentityStatus(identityStatus);

                        _repository.Remove<NafathTransaction, Guid>(nafathTransaction);
                        var result = await _repository.SaveChangesAsync();
                        if (!result.IsDone) return new DataResponse<NafathCheckResponseDto> { IsDone = false, Message = result.Message };
                    }

                    if (nafathResponse.Data?.Data?.Status == SysSettingKeyConstants.NafathStatus_REJECTED
                    || nafathResponse.Data?.Data?.Status == SysSettingKeyConstants.NafathStatus_EXPIRED)
                    {
                        IdentityStatus identityStatus = new IdentityStatus()
                        {
                            TransactionId = nafathTransaction.TransactionId,
                            IsValid = false,
                        };
                        await _identityHub.Clients.Group(identityStatus.TransactionId).ReceivedIdentityStatus(identityStatus);

                        _repository.Remove<NafathTransaction, Guid>(nafathTransaction);
                        var result = await _repository.SaveChangesAsync();
                        if (!result.IsDone) return new DataResponse<NafathCheckResponseDto> { IsDone = false, Message = result.Message };
                    }
                }
                
            }
            return new DataResponse<NafathCheckResponseDto> { IsDone = false, Message = "" };
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
