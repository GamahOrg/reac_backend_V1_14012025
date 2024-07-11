using REAC.Application.Contracts.Dtos.Integration.Nafath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Integration.GEE
{

    #region Nafath

    public class NafathRequestDto
    {
        public string NationalId { get; set; }
        public string Token { get; set; }
    }

    public class NafathRequestBodyDto
    {
        public string Action { get; set; }
        public NafathRequestParametersBodyDto Parameters { get; set; }
    }

    public class NafathRequestParametersBodyDto
    {
        public string service { get; set; }
        public string id { get; set; }
    }

    public class NafathResponseDto
    {
        public NafathResponseDataDto? Data { get; set; }
        public NafathResponseErrorDto? Error { get; set; }
    }

    public class NafathResponseDataDto
    {
        public string TransId { get; set; }
        public string Random { get; set; }
    }

    #endregion


    #region Nafath Check API

    public class NafathCheckBodyDto
    {
        public string Action { get; set; }
        public NafathTransactionDto Parameters { get; set; }
    }

    public class NafathCheckRequestDto
    {
        public string Id { get; set; }
        public string Random { get; set; }
        public string TransId { get; set; }
        public string Token { get; set; }
    }

    public class NafathCheckResponseDto
    {
        public NafathCheckResponseDataDto? Data { get; set; }
        public NafathResponseErrorDto? Error { get; set; }
    }

    public class NafathCheckResponseDataDto
    {
        public string Status { get; set; }
        public string? AccessToken { get; set; }
    }


    public class WafiReceivedCallBackModel
    {
        public string TransactionId { get; set; }
        public bool IsValid { get; set; }
    }

    #endregion


    public class NafathResponseErrorDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }











    public class NafathLoginDto
    {
        public string nationalId { get; set; }
        public string service { get; set; }
    }

    public class FullNafathLoginDto
    {
        public string LoginURL { get; set; }
        public string AppId { get; set; }
        public string AppKey { get; set; }
        public string Local { get; set; }
        public string RequestId { get; set; }
        public NafathLoginDto NafathLogin { get; set; }
    }

    public class NafathCallbackDto
    {
        public string Token { get; set; }
        public string TransId { get; set; }
        public string NationalId { get; set; }
    }

    





    //public class FullNafathLoginResponseDto
    //{
    //    public int Status { get; set; }
    //    public NafathLoginResponseDto NafathLoginData { get; set; } = new NafathLoginResponseDto();
    //}

    public class NafathErrorResponseDto
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public int Reference { get; set; }
    }

    //public class NafathLoginIntegrationDto
    //{
    //    [JsonPropertyName("data")]
    //    public NafathLoginResponseDto Data { get; set; }
    //}

    public class NafathCallbackIntegrationDto
    {
        [JsonPropertyName("data")]
        public NafathCallbackDto Data { get; set; }
    }

    



}
