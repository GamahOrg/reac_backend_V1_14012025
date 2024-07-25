using AutoMapper;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.Application.Contracts.Dtos.CitizenRequests;
using REAC.Application.Contracts.Dtos.Delegator;
using REAC.Application.Contracts.Dtos.Facility;
using REAC.Application.Contracts.Dtos.Integration.Nafath;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.System.AreaDto;
using REAC.Application.Contracts.Dtos.System.CitizenRequestStatusDtos;
using REAC.Application.Contracts.Dtos.System.CityDto;
using REAC.Application.Contracts.Dtos.System.DepartmentDtos;
using REAC.Application.Contracts.Dtos.System.EmployeeDtos;
using REAC.Application.Contracts.Dtos.System.GenderDtos;
using REAC.Application.Contracts.Dtos.System.IdentityTypeDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestCategoryDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestContractTypeDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestCorrespondenceDestinationDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestExtendPeriodRequestStatusDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestOwnerTypeDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestStatusDtos;
using REAC.Application.Contracts.Dtos.Wpf.Business.WorkFlowLog;
using REAC.EntityFrameworkCore.Entities;
using REAC.EntityFrameworkCore.Entities.Integration;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;
using System.Globalization;

namespace REAC.Application.Contracts
{
    public class DtosMapper : Profile
    {
        public DtosMapper()
        {
            CreateMap<SysArea, SysAreaDto>()
                                       .ForMember(des => des.AreaName,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).AreaName)) ?
                                       src.AreaName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).AreaName));

            CreateMap<SysCity, SysCityDto>()
                    .ForMember(des => des.CityName,
                    opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                    string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).CityName)) ?
                     src.CityName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).CityName));


            CreateMap<WorkFlowLog, WorkFlowLogDto>()
                                       .ForMember(des => des.LogMessage,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).LogMessage)) ?
                                       src.LogMessage : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).LogMessage));




            CreateMap<SysCitizenRequestType, SysCitizenRequestTypeDto>()
                                       .ForMember(des => des.CitizenRequestTypeName,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).CitizenRequestTypeName)) ?
                                       src.CitizenRequestTypeName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).CitizenRequestTypeName));

            CreateMap<SysArbitratorClass, SysArbitratorClassDto>()
                                      .ForMember(des => des.ArbitratorClassName,
                                      opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                      string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).ArbitratorClassName)) ?
                                      src.ArbitratorClassName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).ArbitratorClassName));


            CreateMap<SysGender, SysGenderDto>()
                                       .ForMember(des => des.Gender,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).Gender)) ?
                                       src.Gender : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).Gender));

            CreateMap<SysEmployee, SysEmployeeDto>()
                                       .ForMember(des => des.EmployeeName,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EmployeeName)) ?
                                       src.EmployeeName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EmployeeName));

            CreateMap<SysIdentityType, SysIdentityTypeDto>()
                                       .ForMember(des => des.IdentityType,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).IdentityType)) ?
                                       src.IdentityType : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).IdentityType));


            CreateMap<SysLitigationRequestCategory, SysLitigationRequestCategoryDto>()
                    .ForMember(des => des.CategoryName,
                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).CategoryName)) ? src.CategoryName : src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).CategoryName))



             .ForMember(des => des.TermsAndConditions,
                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).TermsAndConditions)) ? src.TermsAndConditions : src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).TermsAndConditions))
             .ForMember(des => des.AcknowledgmentContent,
                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).AcknowledgmentContent)) ? src.AcknowledgmentContent : src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).AcknowledgmentContent));


            CreateMap<SysLitigationRequestContractType, SysLitigationRequestContractTypeDto>()
                    .ForMember(des => des.TypeName,
                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).TypeName)) ? src.TypeName : src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).TypeName))



             .ForMember(des => des.TermsAndConditions,
                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).TermsAndConditions)) ? src.TermsAndConditions : src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).TermsAndConditions))
             .ForMember(des => des.AcknowledgmentContent,
                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).AcknowledgmentContent)) ? src.AcknowledgmentContent : src.Translations.FirstOrDefault(
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).AcknowledgmentContent));


            CreateMap<SysLitigationRequestCorrespondenceDestination, SysLitigationRequestCorrespondenceDestinationDto>()
                                      .ForMember(des => des.Destination,
                                      opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                      string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).Destination)) ?
                                      src.Destination : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).Destination));

            CreateMap<SysLitigationRequestExtendPeriodRequestStatus, SysLitigationRequestExtendPeriodRequestStatusDto>()
                                     .ForMember(des => des.StatusName,
                                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                     string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).StatusName)) ?
                                     src.StatusName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).StatusName));


            CreateMap<SysLitigationRequestOwnerType, SysLitigationRequestOwnerTypeDto>()
                                     .ForMember(des => des.LitigationRequestOwnerType,
                                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                     string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).LitigationRequestOwnerType)) ?
                                     src.LitigationRequestOwnerType : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).LitigationRequestOwnerType));

            CreateMap<SysLitigationRequestStatus, SysLitigationRequestStatusDto>()
                                     .ForMember(des => des.RequestStatus,
                                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                     string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).RequestStatus)) ?
                                     src.RequestStatus : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).RequestStatus));

            CreateMap<SysCitizenRequestStatus, SysCitizenRequestStatusDto>()
                                     .ForMember(des => des.RequestStatus,
                                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                     string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).RequestStatus)) ?
                                     src.RequestStatus : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).RequestStatus));


            CreateMap<SysDepartment, SysDepartmentDto>()
                           .ForMember(des => des.DepartmentName,
                           opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                           string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).DepartmentName)) ?
                           src.DepartmentName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).DepartmentName));

            CreateMap<Citizen, CitizenBasicDataProfileDto>()
                                .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender.Gender))
                                .ForMember(dest => dest.NationalityName, opt => opt.MapFrom(src => src.Nationality.NationalityName))
                                .ReverseMap();



            CreateMap<Citizen, CitizenShortDto>().ReverseMap();

            CreateMap<Citizen, CitizenCreateUpdateDto>().ReverseMap();

            CreateMap<CitizenRequestFreelanceWorkDocumentFile, CitizenRequestFreelanceWorkDocumentFileCreateUpdateDto>().ReverseMap();

            CreateMap<CitizenRequestAcademicDegreeFile, CitizenRequestAcademicDegreeFileCreateUpdateDto>().ReverseMap();

            CreateMap<CitizenRequestExperienceCertificate, CitizenRequestExperienceCertificateCreateUpdateDto>().ReverseMap();


            CreateMap<CitizenRequestFreelanceWorkDocumentFile, CitizenRequestFreelanceWorkDocumentFileFullDataDto>().ReverseMap();

            CreateMap<CitizenRequestAcademicDegreeFile, CitizenRequestAcademicDegreeFileFullDataDto>().ReverseMap();

            CreateMap<CitizenRequestExperienceCertificate, CitizenRequestExperienceCertificateFullDataDto>().ReverseMap();
            CreateMap<CitizenRequestExperienceCertificateFile, CitizenRequestExperienceCertificateFileFullDataDto>().ReverseMap();
            CreateMap<CitizenRequestExperienceCertificateFile, CitizenRequestExperienceCertificateFileCreateUpdateDto>().ReverseMap();

            CreateMap<CitizenRequestNote, CitizenRequestNoteCreateUpdateDto>().ReverseMap();

            CreateMap<CitizenRequest, CitizenRequestFullDataDto>().ReverseMap();

            CreateMap<CitizenRequest, CitizenRequestCreateUpdateDto>().ReverseMap();

            CreateMap<DelegationRequest, DelegationRequestCreateUpdateDto>().ReverseMap();
            CreateMap<Facility, FacilityCreateUpdateDto>().ReverseMap();
            CreateMap<Facility, FacilityShortDto>().ReverseMap();
            CreateMap<Facility, GetAllFacilitiesResponseDto>().ReverseMap();
            CreateMap<Facility, GetFacilityByIdResponse>().ReverseMap();
            CreateMap<Facility, EditFacilityRequestDto>().ReverseMap();


            CreateMap<Delegator, DelegatorFullDataDto>().ReverseMap();
            CreateMap<Delegator, DelegatorShortDataDto>()
                      .ForMember(dest => dest.DelegatorName, opt => opt.MapFrom(src => src.DelegatorCitizen.FullName))
                    .ForMember(dest => dest.FacilityName, opt => opt.MapFrom(src => src.Facility.ArabicFacilityName))
                    .ForMember(dest => dest.DelegationRelationshipTypeName, opt => opt.MapFrom(src => src.SysDelegationRelationshipType.DelegationRelationshipTypeName))

                    .ReverseMap();

            CreateMap<CitizenRequest, CitizenRequestShortDataDto>()
                      .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.SysArea.AreaName))
                    .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.RequestStatus))
                    .ForMember(dest => dest.ArbitratorClassName, opt => opt.MapFrom(src => src.SysArbitratorClass.ArbitratorClassName))
                    .ForMember(dest => dest.CitizenRequestType, opt => opt.MapFrom(src => src.SysCitizenRequestType.CitizenRequestTypeName))
                    .ReverseMap();
            CreateMap<SysCitizenRequestType, SysCitizenRequestTypeCreateUpdateDto>().ReverseMap();
            CreateMap<SysCitizenRequestTypeTranslation, SysCitizenRequestTypeTranslationCreateUpdateDto>().ReverseMap();


            CreateMap<Delegator, FacilityDelegatorCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequest, LitigationRequestCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestContract, LitigationRequestContractCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestCorrespondence, LitigationRequestCorrespondenceCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestCorrespondenceFile, LitigationRequestCorrespondenceFileCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestExtendPeriodRequest, LitigationRequestExtendPeriodRequestCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestExtendPeriodRequestFile, LitigationRequestExtendPeriodRequestFileCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestGround, LitigationRequestGroundCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestGroundFile, LitigationRequestGroundFileCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestReconciliationReportItem, LitigationRequestReconciliationReportItemCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestNote, LitigationRequestNoteCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestNoteFile, LitigationRequestNoteFileCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestReconciliationReport, LitigationRequestReconciliationReportCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestRequest, LitigationRequestRequestCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestRequestFile, LitigationRequestRequestFileCreateUpdateDto>().ReverseMap();
            CreateMap<SysFileManager, SysFileManagerCreateUpdateDto>().ReverseMap();
            CreateMap<SysLitigationRequestAmount, SysLitigationRequestAmountCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestSession, LitigationRequestSessionCreateUpdateDto>().ReverseMap();
            CreateMap<SysSetting, SysSettingCreateUpdateDto>().ReverseMap();
            CreateMap<SysRegistrationRequest, SysRegistrationRequestCreateUpdateDto>().ReverseMap();
            CreateMap<NafathTransaction, NafathTransactionDto>().ReverseMap();
        }
    }
}
