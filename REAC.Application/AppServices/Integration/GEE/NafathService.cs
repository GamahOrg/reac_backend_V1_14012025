using Core.Dtos;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.Dto;
using Core.Log.Application.Contracts.IAppServices;
using Core.Security.Contracts;
using Core.Security.Entities;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using REAC.Application.AppServices.Integration.Hubs;
using REAC.Application.Contracts.Dtos.Integration.GEE;
using REAC.Application.Contracts.Dtos.Integration.Nafath;
using REAC.Application.Contracts.Dtos.UserManage;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Application.Contracts.IAppServices.Integration.Hubs.Contracts;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Actor;
using REAC.EntityFrameworkCore.Entities.Integration;
using REAC.EntityFrameworkCore.Entities.System;
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
        private readonly INafathTransactionService _nafathTransactionService;
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _loggerManager;

        public NafathService(ISysSettingService sysSetting, IAccountService accountService, IEntityRepository repository, IGEELoginService gEELoginService, IHubContext<IdentityHub, IIdentityClient> identityHub, 
            INafathTransactionService nafathTransactionService, UserManager<User> userManager, ILoggerManager loggerManager)
        {
            _sysSetting = sysSetting;
            _accountService = accountService;
            _repository = repository;
            _gEELoginService = gEELoginService;
            _identityHub = identityHub;
            _nafathTransactionService = nafathTransactionService;
            _userManager = userManager;
            _loggerManager = loggerManager;
        }

        public async Task<DataResponse<NafathResponseDto>> Nafath(NafathRequestDto nafathRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                if (nafathRequestDto is null)
                    return new DataResponse<NafathResponseDto> { IsDone = false, Message = "" };

                string? accessToken = null;
                string? refreshToken = null;
                bool isDraftAccount = false;
                bool isArbitrator = false;
                bool isReformer = false;
                bool isLawyer = false;

                var existCitizen = (await _repository.GetQueryable<Citizen, Guid>())
                    .FirstOrDefault(x => x.IdentityNumber == nafathRequestDto.NationalId);

                if (existCitizen is not null)
                {
                    isArbitrator = existCitizen.IsArbitrator ?? false;
                    isReformer = existCitizen.IsReformer ?? false;
                    isLawyer = existCitizen.IsLawyer ?? false;
                    isDraftAccount = existCitizen.IsDraftAccount ?? false;
                }

                if (!nafathRequestDto.IsRegister && (existCitizen is not null && !isDraftAccount))
                {
                    UserLoginDto UserLogin = new UserLoginDto()
                    { UserName = nafathRequestDto.NationalId, Password = "123456" };
                    var login = await _accountService.LoginAsync(UserLogin, cancellationToken);
                    if (!login.IsDone || login == null || login.Data == null)
                        return new DataResponse<NafathResponseDto> { IsDone = false, Message = login?.Message ?? string.Empty };

                    accessToken = login.Data.AccessToken;
                    refreshToken = login.Data.RefreshToken;
                }
               
                if(!nafathRequestDto.IsRegister && existCitizen is null && !isDraftAccount)
                    return new DataResponse<NafathResponseDto> { IsDone = false, Message = "هذا المستخدم غير مسجل بالنظام" };

                if (nafathRequestDto.IsRegister)
                {
                    var isExistUser = _userManager.Users.Any(x => x.UserName == nafathRequestDto.NationalId);
                    if(!isDraftAccount && isExistUser)
                        return new DataResponse<NafathResponseDto> { IsDone = false, Message = "المستخدم موجود بالفعل" };
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

                var x = response.Content.ReadAsStringAsync();
                NafathResponseDto? nafathResponse = new NafathResponseDto();
                if (response.IsSuccessStatusCode)
                {
                    nafathResponse.Data = await response.Content.ReadFromJsonAsync<NafathResponseDataDto>();
                    nafathResponse.IsDraftAccount = isDraftAccount;
                    //if(nafathResponse.Data is null || nafathResponse.Data.TransId is null || nafathResponse.Data.Random is null)
                    //{
                    //    var nafathTransaction = _nafathTransactionService.GettAll().FirstOrDefault(x => x.NationalId == nafathRequestDto.NationalId);
                    //    NafathResponseDataDto nafathResponseData = new NafathResponseDataDto()
                    //    {
                    //        Random = nafathTransaction.RandomNumber,
                    //        TransId = nafathTransaction.TransactionId,
                    //    };
                    //    nafathResponse.Data = nafathResponseData;
                    //    return new DataResponse<NafathResponseDto> { IsDone = true, Data = nafathResponse };

                    //}
                    if (nafathResponse.Data is not null && nafathResponse.Data.Random is not null && nafathResponse.Data.TransId is not null)
                    {
                        NafathTransaction nafathTransaction = new NafathTransaction()
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                            NationalId = nafathRequestDto.NationalId,
                            RandomNumber = nafathResponse.Data.Random,
                            TransactionId = nafathResponse.Data.TransId,
                            RequestExpired = DateTime.Now.AddMinutes(2),
                            IsDraftAccount = isDraftAccount,
                            IsReformer = isReformer,
                            IsArbitrator = isArbitrator,
                            IsLawye = isLawyer,
                        };
                        _nafathTransactionService.Add(nafathTransaction);
                    }
                    return new DataResponse<NafathResponseDto> { IsDone = true, Data = nafathResponse };
                }
                else
                {
                    nafathResponse.Error = await response.Content.ReadFromJsonAsync<NafathResponseErrorDto>();
                    nafathResponse.IsDraftAccount = isDraftAccount;
                    return new DataResponse<NafathResponseDto> { IsDone = false, Message = nafathResponse.Error?.Message ?? nafathResponse.Error?.Code ?? "", Data = nafathResponse };
                }
            }
            catch (Exception e)
            {
                return new DataResponse<NafathResponseDto>() { IsDone = false, Message = e.Message + e.InnerException };
            }
        }
        
        
        public async Task CheckNafathStatus()
        {
            try
            {
                var token = (await _gEELoginService.Login()).Data?.Access_token;
                if (token is null)
                    _loggerManager.Log(Core.Log.Domain.Shared.Enums.LoggerType.Database
                       , new LogDto()
                       {
                           Type = Core.Log.Domain.Shared.Enums.LogType.Error,
                           Message = "get access token from API GEE error"
                       });

                var nafathCheckList = _nafathTransactionService.GettAll();
                var removeList = new List<NafathTransaction>();
                if (nafathCheckList is not null && nafathCheckList.Any())
                {
                    foreach (var nafathTransaction in nafathCheckList)
                    {
                        if (nafathTransaction.RequestExpired < DateTime.Now)
                        {
                            IdentityStatus identityStatus = new IdentityStatus()
                            {
                                TransactionId = nafathTransaction.TransactionId,
                                IsValid = false,
                                Token = null,
                                UserInfoUpdated = null,
                            };
                            removeList.Add(nafathTransaction);
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
                                if (nafathTransaction.AccessToken is not null && nafathTransaction.RefreshToken is not null)
                                {
                                    IdentityStatus identityStatusWithToken = new IdentityStatus()
                                    {
                                        TransactionId = nafathTransaction.TransactionId,
                                        IsValid = true,
                                        Token = nafathTransaction.AccessToken,
                                        RefreshToken = nafathTransaction.RefreshToken,
                                        UserInfoUpdated = new UserInfoUpdatedDto()
                                        {
                                            IsDraftAccount = nafathTransaction.IsDraftAccount,
                                            IsLawye = nafathTransaction.IsLawye,
                                            IsArbitrator = nafathTransaction.IsArbitrator,
                                            IsReformer = nafathTransaction.IsReformer,
                                        }
                                    };
                                    await _identityHub.Clients.Group(identityStatusWithToken.TransactionId).ReceivedIdentityStatus(identityStatusWithToken);

                                    removeList.Add(nafathTransaction);
                                    continue;
                                }

                                var userInfo = await NafathGetUserInfo(nafathCheckStatusResponse.Data.Data.AccessToken);
                                if (userInfo.IsDone)
                                {
                                    IdentityStatus identityStatus = new IdentityStatus()
                                    {
                                        TransactionId = nafathTransaction.TransactionId,
                                        IsValid = true,
                                        Token = nafathTransaction.AccessToken,
                                        RefreshToken = nafathTransaction.RefreshToken,
                                        UserInfoUpdated = new UserInfoUpdatedDto()
                                        {
                                            IsDraftAccount = nafathTransaction.IsDraftAccount,
                                            IsLawye = nafathTransaction.IsLawye,
                                            IsArbitrator = nafathTransaction.IsArbitrator,
                                            IsReformer = nafathTransaction.IsReformer,
                                            DateOfBirth = userInfo.Data?.DateOfBirth,
                                            FirstName = userInfo.Data?.FirstName,
                                            FullName = userInfo.Data?.FullName,
                                            Gender = userInfo.Data?.Gender,
                                            NationalId = userInfo.Data?.NationalId,
                                            NationalIdExpiryDate = userInfo.Data?.NationalIdExpiryDate,
                                            Nationality = userInfo.Data?.Nationality,
                                            NationalityId = userInfo.Data?.NationalityId,
                                        },
                                    };
                                    await _identityHub.Clients.Group(identityStatus.TransactionId).ReceivedIdentityStatus(identityStatus);

                                    removeList.Add(nafathTransaction);
                                    continue;
                                }
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

                                removeList.Add(nafathTransaction);
                                continue;
                            }
                        }
                    }

                    if (removeList.Any())
                    {
                        foreach (var nafathTransaction in removeList)
                        {
                            _nafathTransactionService.Remove(nafathTransaction);
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            
        }
        
        
        
        
        private async Task<DataResponse<UserInfoUpdatedDto>> NafathGetUserInfo(string token)
        {
            try
            {
                var url = _sysSetting.GetBySettingKey(SysSettingKeyConstants.NafathGetUserInfo_URL).Result.Data?.Value;

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(url))
                    return new DataResponse<UserInfoUpdatedDto>() { IsDone = false, Message = "" };

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

                    var userInfoUpdated = await MapUserInfoDataAsync(userInfo);
                    return new DataResponse<UserInfoUpdatedDto> { IsDone = true, Data = userInfoUpdated.Data };
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return new DataResponse<UserInfoUpdatedDto> { IsDone = false, Message = "" };
                else
                    return new DataResponse<UserInfoUpdatedDto> { IsDone = false, Message = "" };
            }
            catch (Exception e)
            {
                return new DataResponse<UserInfoUpdatedDto>() { IsDone = false, Message = e.Message + e.InnerException };
            }
        }
        private async Task<DataResponse<UserInfoUpdatedDto>> MapUserInfoDataAsync(NafathUserInfoDto nafathUserInfoDto)
        {
            var nationality = (await _repository.GetQueryable<SysNationality, Guid>()).FirstOrDefault(x => x.NationalityCode == nafathUserInfoDto.user_info.nationality);

            var nationalityId = Guid.NewGuid();
            if (nationality == null)
            {
                SysNationality sysNationality = new SysNationality()
                {
                    Id = Guid.NewGuid(),
                    NationalityCode = nafathUserInfoDto.user_info.nationality,
                    NationalityName = nafathUserInfoDto.user_info.nationality_ar,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                };

                await _repository.AddAsync<SysNationality, Guid>(sysNationality);
                var result = await _repository.SaveChangesAsync();
                if (!result.IsDone) return new DataResponse<UserInfoUpdatedDto> { IsDone = false, Message = result.Message };

                nationalityId = sysNationality.Id;
            }

            return new DataResponse<UserInfoUpdatedDto>()
            {
                IsDone = true,
                Data = new UserInfoUpdatedDto()
                {
                    FullName = nafathUserInfoDto.user_info.full_name_ar,
                    FirstName = nafathUserInfoDto.user_info.first_name_ar,
                    NationalId = nafathUserInfoDto.user_info.id,
                    DateOfBirth = DateTime.Parse(nafathUserInfoDto.user_info.dob_g),
                    NationalIdExpiryDate = nafathUserInfoDto.user_info.id_expiry_date_g is not null ? 
                        DateTime.Parse(nafathUserInfoDto.user_info.id_expiry_date_g) : null,
                    Gender = nafathUserInfoDto.user_info.gender == "M" ? Domain.Shared.Enum.Gender.Male : Domain.Shared.Enum.Gender.Female,
                    NationalityId = nationality?.Id ?? nationalityId,
                    Nationality = nationality?.NationalityName ?? nafathUserInfoDto.user_info.nationality_ar,
                }
            };
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
