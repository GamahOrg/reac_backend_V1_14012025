using Core.GenericRepository.BaseRepository;
using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Integration.GEE;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.System;
using System.Net.Http.Json;

namespace REAC.Application.AppServices.Integration.GEE
{
    public class MOJService : IMOJService
    {
        private readonly ISysSettingService _sysSetting;
        private readonly IGEELoginService _gEELoginService;

        public MOJService(ISysSettingService sysSetting, IGEELoginService gEELoginService)
        {
            _sysSetting = sysSetting;
            _gEELoginService = gEELoginService;
        }

        public async Task<DataResponse<MOJResponseDto>> ExternalAttorneyInquiry(int attorneyCode, string identityNumber)
        {
            try
            {
                var token = (await _gEELoginService.Login()).Data?.Access_token;
                if (token is null)
                    return new DataResponse<MOJResponseDto> { IsDone = false, Message = "" };

                var settings = await _sysSetting.GetByIntegrationKey(SysSettingKeyConstants.MOJ_Key);
                if (!settings.IsDone || settings.Data is null)
                    return new DataResponse<MOJResponseDto>() { IsDone = false, Message = "" };

                var url = settings.Data.FirstOrDefault(x => x.Key == SysSettingKeyConstants.MOJ_URL)?.Value;

                var client = new HttpClient();
                var queryParams = $"{url}?request.attorneyCode={attorneyCode}&request.identityNumber={identityNumber}";
                var request = new HttpRequestMessage(HttpMethod.Get, queryParams);
                request.Headers.Add("Authorization", $"Bearer {token}");
                var response = await client.SendAsync(request);
                MOJResponseDto? responseDto = new MOJResponseDto();

                if (response.IsSuccessStatusCode)
                {
                    responseDto = await response.Content.ReadFromJsonAsync<MOJResponseDto>();
                    return new DataResponse<MOJResponseDto> { IsDone = true, Data = responseDto };
                }
                else
                {
                    return new DataResponse<MOJResponseDto> { IsDone = false, Data = responseDto, Message = responseDto.ResponseObject.ResponseMessage ?? string.Empty };
                }
            }
            catch (Exception e)
            {
                return new DataResponse<MOJResponseDto> { IsDone = false, Message = e.Message + e.InnerException?.Message };
            }
        }

    }
}
