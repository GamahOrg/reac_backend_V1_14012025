using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Integration.GEE
{
    #region CRInformationByCRNumber

    public class MCCRInformationByCRNumberResponseDto
    {
        public CRInformationByCRNumberResponseDataDto? Data { get; set; }
        public GeneralGEEErrorDto? Error { get; set; }
        public UnauthorizedErrorDto? UnauthorizedError { get; set; }
    }

    
    public class CRInformationByCRNumberResponseDataDto
    {
        public GetCRInformationByCRNumberResponseDto GetCRInformationByCRNumberResponse { get; set; }
    }

    public class GetCRInformationByCRNumberResponseDto
    {
        public GetCRInformationByCRNumberResultDto GetCRInformationByCRNumberResult { get; set; }
    }

    public class GetCRInformationByCRNumberResultDto
    {
        public GetCRInformationByCRNumberResponseDetailObjectDto getCRInformationByCRNumberResponseDetailObject { get; set; }
    }

    public class GetCRInformationByCRNumberResponseDetailObjectDto
    {
        public string CRNationalNumber { get; set; }
        public string CRNumber { get; set; }
        public string CRIssueHijriDate { get; set; }
        public string CRExpiryHijriDate { get; set; }
        public string CRNameAR { get; set; }
        public string CRStatus { get; set; }
        public string CRAddress { get; set; }
    }



    public class CRInformationByCRNumberUpdatedDto
    {
        public CRInformationByCRNumberDto? Data { get; set; }
        public GeneralGEEErrorDto? Error { get; set; }
        public UnauthorizedErrorDto? UnauthorizedError { get; set; }
    }

    public class CRInformationByCRNumberDto
    {
        public string CRNationalNumber { get; set; }
        public string CRNumber { get; set; }
        public DateTime CRIssueHijriDate { get; set; }
        public DateTime CRExpiryHijriDate { get; set; }
        public string CRName { get; set; }
        public string CRStatus { get; set; }
        public string CRAddress { get; set; }
    }

    #endregion

    #region CR Information By CR Owners

    public class MCCRInformationByCROwnersResponseDto
    {
        public CRInformationByCROwnersResponseDataDto? Data { get; set; }
        public GeneralGEEErrorDto? Error { get; set; }
        public UnauthorizedErrorDto? UnauthorizedError { get; set; }
    }

    public class CRInformationByCROwnersResponseDataDto
    {
        public GetCROwnersByCRNumberResponseDto GetCROwnersByCRNumberResponse { get; set; }
    }

    public class GetCROwnersByCRNumberResponseDto
    {
        public GetCROwnersByCRNumberResultDto GetCROwnersByCRNumberResult { get; set; }
    }

    public class GetCROwnersByCRNumberResultDto
    {
        public GetCROwnersByCRNumberResponseDetailObjectDto getCROwnersByCRNumberResponseDetailObject { get; set; }
    }

    public class GetCROwnersByCRNumberResponseDetailObjectDto
    {
        public CROwnersListDto CROwnersList { get; set; }
    }

    public class CROwnersListDto
    {
        public List<CROwnerDto> CROwner { get; set; }
    }

    public class CROwnerDto
    {
        public OwnerIDDto OwnerID { get; set; }
    }

    public class OwnerIDDto
    {
        public PersonIdentifierDto PersonIdentifier { get; set; }
    }

    public class PersonIdentifierDto
    {
        public string NationalID { get; set; }
    }


    public class CRInformationByCROwnersUpdatedDto
    {
        public List<string>? Data { get; set; } = new List<string> ();
        public GeneralGEEErrorDto? Error { get; set; }
        public UnauthorizedErrorDto? UnauthorizedError { get; set; }
    }
    #endregion


    public class UnauthorizedErrorDto
    {
        public GeneralGEEErrorDto ServiceError { get; set; }
    }

}
