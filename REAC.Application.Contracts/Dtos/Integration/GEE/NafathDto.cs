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
        public string? Password{ get; set; }
        public bool IsRegister { get; set; }
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


    #region Nafath Get User Info

    public class NafathUserInfoDto
    {
        public UserInfoDto user_info { get; set; }
    }

    
    public class UserInfoDto
    {
        public string id { get; set; }
        public string full_name_ar { get; set; }
        public string two_names_ar { get; set; }
        public string id_expiry_date_g { get; set; }
        public int nationality { get; set; }
        public string nationality_ar { get; set; }
        public string dob_g { get; set; }
        public string gender { get; set; }
    }

    #endregion

    public class NafathResponseErrorDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }


}
