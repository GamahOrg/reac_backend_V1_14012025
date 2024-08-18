using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Delegator;
using REAC.Application.Contracts.Dtos.Integration.GEE;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Actor;
using REAC.EntityFrameworkCore.Entities.System;
using System.Globalization;
using static REAC.Application.Contracts.IAppServices.Litigation.ILitigationRequestService;

namespace REAC.Application.AppServices
{
    public class DelegatorService : IDelegatorService
    {
        protected IEntityRepository _repository;
        protected IMapper _mapper;
        protected ILoggerManager _loggerManager;
        private readonly IJwtReader _jwtReader;
        private readonly IMOJService _mOJService;
        public DelegatorService(IEntityRepository repository,
            IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, IMOJService mOJService)
        {
            _repository = repository;
            _mapper = mapper;
            _jwtReader = jwtReader;
            _loggerManager = loggerManager;
            _mOJService = mOJService;
        }


        public async Task<BaseResponse> GetDelegators(int pageIndex, int pageSize, string? searchKey)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var delegators = (await _repository.GetQueryable<Delegator, Guid>())
                    .Include(x => x.Citizen).Include(x => x.Facility).Include(x => x.DelegatorCitizen)
                    .Include(x => x.DelegationFile)
                    .Include(x => x.SysDelegationRelationshipType)
                .Where(x => x.CitizenId == referenceId)
                .ToList();

                var fullDelegators = _mapper.Map<List<DelegatorShortDataDto>>(delegators);

                var result = await PaginatedList<DelegatorShortDataDto>.CreateAsync(fullDelegators, pageIndex, pageSize);
                if (result.Items.Count == 0) return new BaseResponse() { IsDone = true, Message = "غير موجود" };

                return new DataResponse<PaginatedList<DelegatorShortDataDto>>()
                {
                    IsDone = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsDone = false, Message = "خطأ داخلي" };

            }

        }
        public async Task<BaseResponse> GetDelegatorById(Guid delegatorId)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var delegators = (await _repository.GetQueryable<Delegator, Guid>())
                    .Include(x => x.Citizen).Include(x => x.Facility).Include(x => x.DelegatorCitizen)
                    .Include(x => x.DelegationFile)
                              .Where(x => x.Id == delegatorId)
                              .ToList();

                var fullDelegators = _mapper.Map<List<DelegatorFullDataDto>>(delegators);
                if (fullDelegators.Count > 0)
                {
                    return new DataResponse<DelegatorFullDataDto>()
                    {
                        IsDone = true,
                        Data = fullDelegators.FirstOrDefault()
                    };
                }
                else
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "غير موجود"
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse() { IsDone = false, Message = "خطأ داخلي" };
            }

        }

        public async Task<BaseResponse> SaveDelegator(DelegatorDto delegatorDto)
        {

            // Edit Delegator
            if (delegatorDto.Id != Guid.Empty)
            {
                return await EditDelagator(delegatorDto);
            }

            // Add new Delegator
            return await AddDelagator(delegatorDto);

        }

        public async Task<BaseResponse> ValidateDelegator(ValidateDelegatorDto validateDelegatorDto)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var citizen = (await _repository.GetQueryable<Citizen, Guid>())
                        .AsNoTracking().FirstOrDefault(x => x.Id == referenceId);

                var validationResult = await _mOJService.ExternalAttorneyInquiry(new MOJRequestDto()
                {
                    AttorneyCode = validateDelegatorDto.DelegateNumber,
                    FirstParties = citizen.IdentityNumber,
                    SecondParties = validateDelegatorDto.IdentityNumber
                });

                if (validationResult is null || !validationResult.IsDone || !validationResult.Data.IsValid)
                    return new BaseResponse() { IsDone = false, Message = "المحامي أو الوكيل غير صالح" };

                return new BaseResponse() { IsDone = true };
            }
            catch (Exception ex)
            {
                return new BaseResponse() { IsDone = false, Message = "خطأ داخلي" };

            }


        }

        public async Task<DataResponse<PartiesDto>> GetDelegatorCitizenData(GetDeligatorCitizenData getDelegatorDataDto)
        {

            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var citizen = (await _repository.GetQueryable<Citizen, Guid>())
                    .AsNoTracking().FirstOrDefault(x => x.Id == referenceId);

            var integrationDataResult = await _mOJService.ExternalAttorneyInquiry(new MOJRequestDto()
            {
                AttorneyCode = getDelegatorDataDto.DelegateNumber,
                FirstParties = citizen.IdentityNumber,
                SecondParties = getDelegatorDataDto.DelegatorIdentityNumber
            });


            if (integrationDataResult is null || !integrationDataResult.IsDone || integrationDataResult.Data?.Error is not null || !integrationDataResult.Data.IsValid)
                return new DataResponse<PartiesDto>() { IsDone = false, Message = "المحامي أو الوكيل غير صالح" };


            var secondParties = integrationDataResult.Data.Data?.responseObject.Results?
                .Select(r => r.Model.SecondParties).FirstOrDefault();


            var citizenDataResult = secondParties?.FirstOrDefault(m => m.IdentityNo == getDelegatorDataDto.DelegatorIdentityNumber);

            return new DataResponse<PartiesDto>()
            {
                IsDone = true,
                Data = citizenDataResult
            };

        }

        private async Task<BaseResponse> EditDelagator(DelegatorDto delegatorDto)
        {
            try
            {
                // Get Delegator
                var delegator = (await _repository.GetQueryable<Delegator, Guid>()).Where(f => f.Id == delegatorDto.Id).FirstOrDefault();
                if (delegator is null) return new BaseResponse() { IsDone = false, Message = "غير موجود" };

                delegator.DelegationFileId = delegatorDto.DelegationFileId;
                delegator.DelegationRelationshipTypeId = (DelegationRelationshipType)delegatorDto.DelegationRelationshipTypeId;
                delegator.FacilityId = delegatorDto.FacilityId;
                delegator.IsActive = delegatorDto.IsActive;

                // Get Delegator Citizen Data
                var result = await _repository.GetByIdAsync<Citizen, Guid>(delegator.DelegatorCitizenId);
                if (!result.IsDone) return new BaseResponse() { IsDone = false, Message = "غير موجود" };

                var delegatorCitizen = ((DataResponse<Citizen>)result).Data;

                delegatorCitizen.IsLawyer = delegatorDto.DelegationRelationshipTypeId == DelegationRelationshipType.Lawyer ? true : false;


                var saveCurrentCitizen = _repository.Update<Citizen, Guid>(delegatorCitizen);
                var saveCurrentDelegator = _repository.Update<Delegator, Guid>(delegator);

                var saveResponse = await _repository.SaveChangesAsync();
                if (!saveResponse.IsDone) return saveResponse;

                var updatedDelegator = await GetDelegatorById(delegatorDto.Id);
                if (!updatedDelegator.IsDone) return updatedDelegator;

                updatedDelegator.Message = "تم التعديل بنجاح";
                return updatedDelegator;
            }
            catch (Exception ex)
            {
                return new BaseResponse() { IsDone = false, Message = "خطأ داخلي" };
            }

        }

        private async Task<BaseResponse> AddDelagator(DelegatorDto delegatorDto)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var sourceCitizen = (await _repository.GetQueryable<Citizen, Guid>()).FirstOrDefault(x => x.Id == referenceId);

                if (sourceCitizen is null)
                    return new BaseResponse() { IsDone = false, Message = "المستخدم غير موجود" };

                var lawyerOrAgentCitizen = (await _repository.GetQueryable<Citizen, Guid>()).FirstOrDefault(x => x.IdentityNumber == delegatorDto.IdentityNumber);

                if (lawyerOrAgentCitizen is null)
                {
                    var integrationCitizen = await GetDelegatorCitizenData(new GetDeligatorCitizenData()
                    {
                        DelegatorIdentityNumber = delegatorDto.IdentityNumber,
                        DelegateNumber = string.IsNullOrEmpty(delegatorDto.DelegateNumber) ? 0 : int.Parse(delegatorDto.DelegateNumber),
                        UserIdentityNumber = sourceCitizen.IdentityNumber,
                    });

                    if (integrationCitizen.Data is null || !integrationCitizen.IsDone)
                        return new BaseResponse() { IsDone = false, Message = "خطأ في رقم الهوية" };

                    CultureInfo provider = new CultureInfo("en-US");
                    CultureInfo.CurrentCulture = provider;

                    Citizen citizenFromintegration = new Citizen()
                    {
                        Id = Guid.NewGuid(),
                        FullName = integrationCitizen.Data.FullName,
                        ShortName = integrationCitizen.Data.FirstName,
                        IdentityNumber = delegatorDto.IdentityNumber,
                        GenderId = (integrationCitizen.Data.GenderId == Gender.Male.GetHashCode()) ? Gender.Male : Gender.Female,
                        BirthDate = DateTime.Parse(integrationCitizen.Data.BirthDate),
                        IdentityTypeId = (IdentityType?)(int)(integrationCitizen.Data.IdentityNo.StartsWith("1") ? IdentityType.NationalIdentity : integrationCitizen.Data.IdentityNo.StartsWith("2") ? IdentityType.Residence : IdentityType.GulfIdentity),
                        IsDraftAccount = true,
                        Code = await GenerateCitizenCode()
                    };


                    if (delegatorDto.DelegationRelationshipTypeId == DelegationRelationshipType.Lawyer)
                    {
                        citizenFromintegration.IsLawyer = true;
                        citizenFromintegration.LawyerLicenseNumber = delegatorDto.LawyerLicenseNumber;
                        citizenFromintegration.LawLicenseFileId = delegatorDto.LawLicenseFileId;
                    }

                    var response = await _repository.AddAsync<Citizen, Guid>(citizenFromintegration);
                    if (!response.IsDone) return new BaseResponse() { IsDone = false, Message = "خطأ في الاضافة" };

                    delegatorDto.DelegatorCitizenId = citizenFromintegration.Id;
                }
                else
                {
                    delegatorDto.DelegatorCitizenId = lawyerOrAgentCitizen.Id;
                }

                var updatedCitizen = _mapper.Map<Delegator>(delegatorDto);
                updatedCitizen.CitizenId = referenceId;

                await _repository.AddAsync<Delegator, Guid>(updatedCitizen);
                var saveResponse = await _repository.SaveChangesAsync();
                if (!saveResponse.IsDone) return saveResponse;

                return new BaseResponse() { IsDone = true, Message = "تم الاضافة بنجاح" };
            }
            catch (Exception ex)
            {
                return new BaseResponse() { IsDone = false, Message = "خطأ داخلي" };

            }


        }

        private async Task<string> GenerateCitizenCode()
        {
            var citizenRequestList = (await _repository.GetQueryable<Citizen, Guid>()).Where(x => x.Code != null).ToList();


            string code = "CZ-0001";

            if (citizenRequestList is null)
                code = "CZ-0001";
            else
                code = "CZ-" + (citizenRequestList.Count() + 1).ToString("D4");

            return code;
        }
    }
}
