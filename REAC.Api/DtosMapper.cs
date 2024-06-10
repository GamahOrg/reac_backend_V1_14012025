using AutoMapper;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Contract;
using REAC.Application.Contracts.Dtos.Litigation.Create.Defendant;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.Application.Contracts.Dtos.System;
using REAC.Application.Contracts.Dtos.System.DepartmentDtos;
using REAC.Application.Contracts.Dtos.System.EmployeeDtos;
using REAC.Application.Contracts.Dtos.System.GenderDtos;
using REAC.Application.Contracts.Dtos.System.IdentityTypeDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestCategoryDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestContractTypeDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestCorrespondenceDestinationDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestExtendPeriodRequestStatusDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestMeetingDecisionDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestOwnerTypeDtos;
using REAC.Application.Contracts.Dtos.System.LitigationRequestStatusDtos;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;
using REAC.EntityFrameworkCore.Entities.System;
using System.Globalization;

namespace REAC.Application.Contracts
{
    public class DtosMapper : Profile
    {
        public DtosMapper()
        {
            CreateMap<SysGender, SysGenderDto>()
                                       .ForMember(des => des.Gender,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).Gender)) ?
                                       src.Gender : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).Gender)).ReverseMap();

            CreateMap<SysEmployee, SysEmployeeDto>()
                                       .ForMember(des => des.EmployeeName,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EmployeeName)) ?
                                       src.EmployeeName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EmployeeName)).ReverseMap();

            CreateMap<SysIdentityType, SysIdentityTypeDto>()
                                       .ForMember(des => des.IdentityType,
                                       opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                       string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).IdentityType)) ?
                                       src.IdentityType : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).IdentityType)).ReverseMap();


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
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).AcknowledgmentContent)).ReverseMap();


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
                     x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).AcknowledgmentContent)).ReverseMap();


            CreateMap<SysLitigationRequestCorrespondenceDestination, SysLitigationRequestCorrespondenceDestinationDto>()
                                      .ForMember(des => des.Destination,
                                      opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                      string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).Destination)) ?
                                      src.Destination : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).Destination)).ReverseMap();

            CreateMap<SysLitigationRequestExtendPeriodRequestStatus, SysLitigationRequestExtendPeriodRequestStatusDto>()
                                     .ForMember(des => des.StatusName,
                                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                     string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).StatusName)) ?
                                     src.StatusName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).StatusName)).ReverseMap();

            CreateMap<SysLitigationRequestMeetingDecision, SysLitigationRequestMeetingDecisionDto>()
                                     .ForMember(des => des.MeetingDecision,
                                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                     string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).MeetingDecision)) ?
                                     src.MeetingDecision : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).MeetingDecision)).ReverseMap();

            CreateMap<SysLitigationRequestOwnerType, SysLitigationRequestOwnerTypeDto>()
                                     .ForMember(des => des.LitigationRequestOwnerType,
                                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                     string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).LitigationRequestOwnerType)) ?
                                     src.LitigationRequestOwnerType : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).LitigationRequestOwnerType)).ReverseMap();

            CreateMap<SysLitigationRequestStatus, SysLitigationRequestStatusDto>()
                                     .ForMember(des => des.RequestStatus,
                                     opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                                     string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).RequestStatus)) ?
                                     src.RequestStatus : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).RequestStatus)).ReverseMap();



            CreateMap<SysDepartment, SysDepartmentDto>()
                           .ForMember(des => des.DepartmentName,
                           opt => opt.MapFrom(src => (src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null ||
                           string.IsNullOrEmpty(src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).DepartmentName)) ?
                           src.DepartmentName : src.Translations.FirstOrDefault(x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).DepartmentName)).ReverseMap();





            CreateMap<Citizen, CitizenCreateUpdateDto>().ReverseMap();
            CreateMap<DelegationRequest, DelegationRequestCreateUpdateDto>().ReverseMap();
            CreateMap<Facility, FacilityCreateUpdateDto>().ReverseMap();
            CreateMap<FacilityDelegator, FacilityDelegatorCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequest, LitigationRequestCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestContract, LitigationRequestContractCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestCorrespondence, LitigationRequestCorrespondenceCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestCorrespondenceFile, LitigationRequestCorrespondenceFileCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestExtendPeriodRequest, LitigationRequestExtendPeriodRequestCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestExtendPeriodRequestFile, LitigationRequestExtendPeriodRequestFileCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestGround, LitigationRequestGroundCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestGroundFile, LitigationRequestGroundFileCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestMeetingReconciliationItem, LitigationRequestMeetingReconciliationItemCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestNote, LitigationRequestNoteCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestNoteFile, LitigationRequestNoteFileCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestReconciliationMeeting, LitigationRequestReconciliationMeetingCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestRequest, LitigationRequestRequestCreateUpdateDto>().ReverseMap();
            CreateMap<LitigationRequestRequestFile, LitigationRequestRequestFileCreateUpdateDto>().ReverseMap();
            CreateMap<SysFileManager, SysFileManagerCreateUpdateDto>().ReverseMap();
            CreateMap<SysLitigationRequestAmount, SysLitigationRequestAmountCreateUpdateDto>().ReverseMap();
            CreateMap<SysLitigationRequestMeetingConfiguration, SysLitigationRequestMeetingConfigurationCreateUpdateDto>().ReverseMap();
            CreateMap<SysSetting, SysSettingCreateUpdateDto>().ReverseMap();
            CreateMap<SysRegistrationRequest, SysRegistrationRequestCreateUpdateDto>().ReverseMap();

            #region Litigation Contract
            CreateMap<RentCreateUpdateDto, LitigationRequestContract>()
                 .ForMember(des => des.LitigationRequestContractTypeId, opt => opt.MapFrom(src => 1 /*LitigationRequestContractType.Rent*/))
                  .ForMember(des => des.SecondPartyCapacity, opt => opt.MapFrom(src => ""))
                 .ForMember(des => des.FirstPartyCitizenId, opt => opt.MapFrom(src => "c87f3f80-6475-482d-937f-b0d0629ae3ef"));

            CreateMap<RealEstateBrokerageCreateUpdateDto, LitigationRequestContract>()
                 .ForMember(des => des.LitigationRequestContractTypeId, opt => opt.MapFrom(src => 4 /*LitigationRequestContractType.RealEstateBrokerage*/))
                  .ForMember(des => des.SecondPartyCapacity, opt => opt.MapFrom(src => ""))
                 .ForMember(des => des.FirstPartyCitizenId, opt => opt.MapFrom(src => "c87f3f80-6475-482d-937f-b0d0629ae3ef"));

            #endregion

            #region Litigation Defendant
            CreateMap<DefendantCreateUpdateDto, LitigationRequestContract>();
            CreateMap<LitigationRequestStatus, SysLitigationRequestStatus>();
            #endregion

            #region Litigation 
            CreateMap<LitigationCreateUpdateDto, LitigationRequest>()
                 .ForMember(des => des.CreatedOn, opt => opt.MapFrom(src => DateTime.Now))
                 .ForMember(des => des.InternalStatusId, opt => opt.MapFrom(src => LitigationRequestStatus.SaveAsDraft))
                 .ForMember(des => des.ExternalStatusId, opt => opt.MapFrom(src => LitigationRequestStatus.SaveAsDraft))
                 .ForMember(des => des.ApplicantCitizenId, opt => opt.MapFrom(src => "c87f3f80-6475-482d-937f-b0d0629ae3ef"))
                 .ForMember(des => des.IsPaid, opt => opt.MapFrom(src => false));
            CreateMap<LitigationRequestCategory, SysLitigationRequestCategory>();
            CreateMap<int, SysLitigationRequestStatus>();

            #endregion

            CreateMap<FileParameterDto, SysFileManager>();
            CreateMap<LitigationRequestRequestCreateUpdateDto, LitigationRequestRequest>();
            CreateMap<LitigationRequestRequestFileCreateUpdateDto, LitigationRequestRequestFile>();
            CreateMap<LitigationRequestGroundCreateUpdateDto, LitigationRequestGround>();
            CreateMap<LitigationRequestGroundFileCreateUpdateDto, LitigationRequestGroundFile>();

            CreateMap<SysLitigationRequestContractType, SysLitigationRequestContractTypeGetDto>();
            CreateMap<SysLitigationRequestContractType, SysLitigationRequestContractTypeDto>();

        }
    }
}
