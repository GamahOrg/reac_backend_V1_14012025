using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.GenericRepository.Shared.Pagging;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices
{

    public class CitizenService : BasicService<Citizen, Guid>, ICitizenService
    {
        private readonly IMapper _mapper;
        private readonly IJwtReader _jwtReader;
        public CitizenService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {
            _mapper = mapper;
            _jwtReader = jwtReader;
        }
        public async Task<DateTime?> GetIdentityExpirationDate(Guid? userId)
        {
            var quarable = await GetQueryableAsync();
            var identityExpirationDate = quarable.FirstOrDefault(x => x.Id == userId)?.IdentityExpiryDate;
            return identityExpirationDate;
        }
        public async Task<BaseResponse> GetDelegatedPersons()
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var delegatorPersons = (await _repository.GetQueryable<Delegator, Guid>())
                            .Include(x => x.Citizen)
                            .Include(x => x.Facility)
                         .Where(x => x.DelegatorCitizenId == referenceId).ToList();

            List<DelegatorItem> persons = new List<DelegatorItem>();

            foreach (var delegatorPerson in delegatorPersons)
            {
                if (delegatorPerson.Facility == null)
                {
                    if (delegatorPerson.CitizenId.HasValue)
                        persons.Add(new DelegatorItem() { Id = delegatorPerson.CitizenId.Value, Name = delegatorPerson.Citizen.FullName });
                }
            }

            return new DataResponse<List<DelegatorItem>>()
            {
                IsDone = true,
                Data = persons
            };
        }
        public async Task<BaseResponse> GetDelegatedFacilities()
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var delegatorPersons = (await _repository.GetQueryable<Delegator, Guid>())
                            .Include(x => x.Citizen)
                            .Include(x => x.Facility)
                         .Where(x => x.DelegatorCitizenId == referenceId).ToList();
            List<DelegatorItem> facilities = new List<DelegatorItem>();

            foreach (var delegatorPerson in delegatorPersons)
            {
                if (delegatorPerson.Facility != null)
                {
                    facilities.Add(new DelegatorItem() { Id = delegatorPerson.FacilityId.Value, Name = delegatorPerson.Facility.ArabicFacilityName });
                }
            }

            return new DataResponse<List<DelegatorItem>>()
            {
                IsDone = true,
                Data = facilities
            };
        }
        public async Task<ICollection<Facility>> GetRepresentativeCitizens(Guid? userId) // بجيب هنا كل المدعي عليهم اللي انا ممثل عنهم
        {
            var quarable = await GetQueryableAsync();
            var result = quarable.Where(x => x.LitigationCompanyRepresentativeId == userId).SelectMany(x => x.Facilities).ToList();
            return result;
        }
        public async Task<(Guid, bool)> CheckCitizenExist(string identityNumber)
        {
            var quarable = await GetQueryableAsync();
            Guid citizenId = new Guid("00000000-0000-0000-0000-000000000000");
            bool citizenExist = true;
            var exist = quarable.Where(x => x.IdentityNumber == identityNumber).FirstOrDefault();
            if (exist != null)
            {
                citizenId = exist.Id;
            }
            else
                citizenExist = false;

            return (citizenId, citizenExist);
        }
        public async Task<BaseResponse> GetCitizenBasicData()
        {

            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);


            var citizen = (await _repository.GetQueryable<Citizen, Guid>())
                    .Include(x => x.Gender)
                    .Include(x => x.IdentityFile)
                    .Include(x => x.LawLicenseFile)
                    .Include(x => x.Nationality)
                    .AsNoTracking().Where(x => x.Id == referenceId)
                              .ToList();


            if (citizen.ToList().Count == 0) return new BaseResponse() { IsDone = false, Message = "Not Found" };


            var citizendto = _mapper.Map<GetCitizenBasicDataProfileDto>(citizen.FirstOrDefault());
            return new DataResponse<GetCitizenBasicDataProfileDto>()
            {
                IsDone = true,
                Data = citizendto
            };

        }
        public async Task<BaseResponse> UpdateCitizenBasicData(SaveCitizenBasicDataProfileDto citizenBasicDataProfileDto)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var response = await _repository.GetByIdAsync<Citizen, Guid>(referenceId, default);

            if (response.IsDone != true)
                return response;

            var citizen = ((DataResponse<Citizen>)response).Data;
            citizen.NationalAddress = citizenBasicDataProfileDto.NationalAddress;
            citizen.ShortName = citizenBasicDataProfileDto.ShortName;
            citizen.Email = citizenBasicDataProfileDto.Email;

            citizen.Employer = citizenBasicDataProfileDto.Employer;
            citizen.Mobile = citizenBasicDataProfileDto.Mobile;
            citizen.Fax = citizenBasicDataProfileDto.Fax;
            citizen.Email = citizenBasicDataProfileDto.Email;
            citizen.AreaId = citizenBasicDataProfileDto.AreaId;
            citizen.CityId = citizenBasicDataProfileDto.CityId;
            citizen.District = citizenBasicDataProfileDto.District;
            citizen.Street = citizenBasicDataProfileDto.Street;
            citizen.PostalCode = citizenBasicDataProfileDto.PostalCode;
            citizen.SubNumber = citizenBasicDataProfileDto.SubNumber;
            citizen.BuildingNumber = citizenBasicDataProfileDto.BuildingNumber;
            citizen.UnitNumber = citizenBasicDataProfileDto.UnitNumber;
            citizen.IsLawyer = citizenBasicDataProfileDto.IsLawyer;
            citizen.LawyerLicenseNumber = citizenBasicDataProfileDto.LawyerLicenseNumber;
            citizen.LawyerLicenseExpirationDate = citizenBasicDataProfileDto.LawyerLicenseExpirationDate;
            citizen.LawLicenseFileId = citizenBasicDataProfileDto.LawLicenseFileId;

            citizen.IdentityFileId = citizenBasicDataProfileDto.IdentityFileId;
            citizen.HomeTelephone = citizenBasicDataProfileDto.HomeTelephone;
            citizen.WorkTelephone = citizenBasicDataProfileDto.WorkTelephone;

            var saveResponse = _repository.SaveChangesAsync().Result;
            if (saveResponse.IsDone != true)
                return response;

            var updatedResponse = await _repository.GetByIdAsync<Citizen, Guid>(referenceId, default);
            if (updatedResponse.IsDone != true)
                return response;

            return await GetCitizenBasicData();
        }
        public async Task<BaseResponse> GetReformersLookup(Guid? citizenId)
        {
            if (citizenId == null | citizenId == Guid.Empty)
            {
                var citizens = (await _repository.GetQueryable<Citizen, Guid>())
                             .Include(x => x.ReformerLitigationRequests)
                             .Where(x => x.IsReformer == true & x.ReformerStatusId == ReformerStatus.Certified).ToList();
                List<CitizenReformer> citizenReformers = new List<CitizenReformer>();
                citizens.ForEach(t =>
                {
                    citizenReformers.Add(new CitizenReformer()
                    {
                        FullName = t.FullName,
                        Id = t.Id,
                        RequestsCount = t.ReformerLitigationRequests.Where(x => x.IsFinished == false).Count()
                    });
                });
                return new DataResponse<List<CitizenReformer>>()
                {
                    IsDone = true,
                    Data = citizenReformers.OrderBy(x => x.RequestsCount).ToList()
                };
            }
            else
            {
                var citizens = (await _repository.GetQueryable<Citizen, Guid>())
                                                 .Include(x => x.ReformerLitigationRequests)
                            .Where(x => x.IsReformer == true & x.Id != citizenId).ToList();
                List<CitizenReformer> citizenReformers = new List<CitizenReformer>();
                citizens.ForEach(t =>
                {
                    citizenReformers.Add(new CitizenReformer()
                    {
                        FullName = t.FullName,
                        Id = t.Id,
                        RequestsCount = t.ReformerLitigationRequests.Where(x => x.IsFinished == false).Count()
                    });
                });

                return new DataResponse<List<CitizenReformer>>()
                {
                    IsDone = true,
                    Data = citizenReformers
                };
            }
        }
        public Task<BaseResponse> GetArbitratorsLookup(Guid? citizenId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddArbitratorFreeUpDecision(ArbitratorFreeUpDecisionCreateDto arbitratorFreeUpDecisionCreateDto)
        {
            try
            {

                var citizen = (await _repository.GetQueryable<Citizen, Guid>())?
                     .Where(x => x.Id == arbitratorFreeUpDecisionCreateDto.CitizenId)
                     .SingleOrDefault();

                if (citizen == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "محكم غير موجود"
                    };

                citizen.ArbitratorStatusId = ArbitratorStatus.Certified;

                ArbitratorStopFreeUpDecision arbitratorStopFreeUpDecision = new ArbitratorStopFreeUpDecision();
                arbitratorStopFreeUpDecision.CitizenId = arbitratorFreeUpDecisionCreateDto.CitizenId;
                arbitratorStopFreeUpDecision.ArbitratorStatusId = ArbitratorStatus.Certified;
                arbitratorStopFreeUpDecision.DecisionDate = arbitratorFreeUpDecisionCreateDto.DecisionDate;
                await _repository.AddAsync<ArbitratorStopFreeUpDecision, int>(arbitratorStopFreeUpDecision);
                var saveResponse = await _repository.SaveChangesAsync();
                if (!saveResponse.IsDone)
                    return saveResponse;
                return await GetArbitratorsDecisions(arbitratorFreeUpDecisionCreateDto.CitizenId);
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.ToString(),
                    Message = ex.Message
                };
            }
        }
        public async Task<BaseResponse> AddArbitratorStopDecision(ArbitratorStopDecisionCreateDto arbitratorStopDecisionCreateDto)
        {
            try
            {
                var citizen = (await _repository.GetQueryable<Citizen, Guid>())?
                      .Where(x => x.Id == arbitratorStopDecisionCreateDto.CitizenId)
                      .SingleOrDefault();

                if (citizen == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "محكم غير موجود"
                    };

                citizen.ArbitratorStatusId = ArbitratorStatus.Stopped;


                ArbitratorStopFreeUpDecision arbitratorStopFreeUpDecision = new ArbitratorStopFreeUpDecision();
                arbitratorStopFreeUpDecision.CitizenId = citizen.Id;
                arbitratorStopFreeUpDecision.ArbitratorStatusId = ArbitratorStatus.Stopped;
                arbitratorStopFreeUpDecision.DecisionDate = arbitratorStopDecisionCreateDto.DecisionDate;
                arbitratorStopFreeUpDecision.LateLitigationRequestNumber = arbitratorStopDecisionCreateDto.LateLitigationRequestNumber;
                arbitratorStopFreeUpDecision.StoppingReason = arbitratorStopDecisionCreateDto.StoppingReason;
                await _repository.AddAsync<ArbitratorStopFreeUpDecision, int>(arbitratorStopFreeUpDecision);
                var saveResponse = await _repository.SaveChangesAsync();
                if (!saveResponse.IsDone)
                    return saveResponse;

                return await GetArbitratorsDecisions(arbitratorStopDecisionCreateDto.CitizenId);
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.ToString(),
                    Message = ex.Message
                };
            }


        }
        public async Task<BaseResponse> AddReformerFreeUpDecision(ReformerFreeUpDecisionCreateDto reformerFreeUpDecisionCreateDto)
        {
            try
            {
                var citizen = (await _repository.GetQueryable<Citizen, Guid>())?
                    .Where(x => x.Id == reformerFreeUpDecisionCreateDto.CitizenId)
                    .SingleOrDefault();

                if (citizen == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "محكم غير موجود"
                    };

                citizen.ReformerStatusId = ReformerStatus.Certified;

                ReformerStopFreeUpDecision ReformerStopFreeUpDecision = new ReformerStopFreeUpDecision();
                ReformerStopFreeUpDecision.CitizenId = reformerFreeUpDecisionCreateDto.CitizenId;
                ReformerStopFreeUpDecision.ReformerStatusId = ReformerStatus.Certified;
                ReformerStopFreeUpDecision.DecisionDate = reformerFreeUpDecisionCreateDto.DecisionDate;
                await _repository.AddAsync<ReformerStopFreeUpDecision, int>(ReformerStopFreeUpDecision);
                var saveResponse = await _repository.SaveChangesAsync();
                if (!saveResponse.IsDone)
                    return saveResponse;
                return await GetReformersDecisions(reformerFreeUpDecisionCreateDto.CitizenId);
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.ToString(),
                    Message = ex.Message
                };
            }
        }
        public async Task<BaseResponse> AddReformerStopDecision(ReformerStopDecisionCreateDto reformerStopDecisionCreateDto)
        {
            try
            {
                var citizen = (await _repository.GetQueryable<Citizen, Guid>())?
                    .Where(x => x.Id == reformerStopDecisionCreateDto.CitizenId)
                    .SingleOrDefault();

                if (citizen == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "محكم غير موجود"
                    };

                citizen.ReformerStatusId = ReformerStatus.Stopped;

                ReformerStopFreeUpDecision ReformerStopFreeUpDecision = new ReformerStopFreeUpDecision();
                ReformerStopFreeUpDecision.CitizenId = reformerStopDecisionCreateDto.CitizenId;
                ReformerStopFreeUpDecision.ReformerStatusId = ReformerStatus.Stopped;
                ReformerStopFreeUpDecision.DecisionDate = reformerStopDecisionCreateDto.DecisionDate;
                ReformerStopFreeUpDecision.LateLitigationRequestNumber = reformerStopDecisionCreateDto.LateLitigationRequestNumber;
                ReformerStopFreeUpDecision.StoppingReason = reformerStopDecisionCreateDto.StoppingReason;
                await _repository.AddAsync<ReformerStopFreeUpDecision, int>(ReformerStopFreeUpDecision);
                var saveResponse = await _repository.SaveChangesAsync();
                if (!saveResponse.IsDone)
                    return saveResponse;

                return await GetReformersDecisions(reformerStopDecisionCreateDto.CitizenId);
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.ToString(),
                    Message = ex.Message
                };
            }

        }

        public async Task<BaseResponse> GetReformersDecisions(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            PagedResult<Citizen> citizens = null;

            var entitiesQuerable = (await _repository.GetQueryable<Citizen, Guid>())
                                     .Include(x => x.ReformerLitigationRequests)
                         .Include(x => x.ReformerStopFreeUpDecisions)
                         .Include(x => x.SysReformerStatus);


            citizens = entitiesQuerable.Where(x => x.IsReformer == true)
                                       .OrderByDescending(x => x.FullName)
                                       .AsSplitQuery()
                                       .AsNoTracking()
                                       .GetPaged(pageIndex, pageSize);

            List<CitizenReformerShortDto> citizenReformers = new List<CitizenReformerShortDto>();

            foreach (var citizen in citizens.Results)
            {
                CitizenReformerShortDto citizenReformer = new CitizenReformerShortDto();
                citizenReformer.Id = citizen.Id;
                citizenReformer.FullName = citizen.FullName;
                citizenReformer.Mobile = citizen.Mobile;
                citizenReformer.IdentityNumber = citizen.IdentityNumber;
                citizenReformer.ReformerStatus = citizen.SysReformerStatus?.StatusName;
                citizenReformer.Email = citizen.Email;
                citizenReformer.RequestsCount = citizen.ReformerLitigationRequests.Count(x => x.IsFinished == false);
                citizenReformer.ReformerStatusDate = citizen.ReformerStopFreeUpDecisions?.OrderBy(x => x.DecisionDate).LastOrDefault()?.DecisionDate;
                citizenReformers.Add(citizenReformer);

            }

            return new DataResponse<PagedResult<CitizenReformerShortDto>>
            {
                Data =
                   new PagedResult<CitizenReformerShortDto>
                   {
                       Results = citizenReformers,
                       CurrentPage = citizens.CurrentPage,
                       PageCount = citizens.PageCount,
                       PageSize = citizens.PageSize,
                       TotalCount = citizens.TotalCount
                   },
                IsDone = true
            };

        }
        public async Task<BaseResponse> GetReformersDecisions(Guid citizenId)
        {
            var citizen = (await _repository.GetQueryable<Citizen, Guid>())
                                         .Include(x => x.SysReformerStatus)
                         .Include(x => x.ReformerLitigationRequests)
                         .Include(x => x.ReformerStopFreeUpDecisions)
                         .ThenInclude(x => x.SysReformerStatus)
                         .Include(x => x.CitizenRequests)
                         .Where(x => x.Id == citizenId).SingleOrDefault();

            if (citizen == null)
                return new BaseResponse() { IsDone = false, Message = "حساب غير موجود" };

            CitizenReformerDetailShortDto citizenReformer = new CitizenReformerDetailShortDto();
            citizenReformer.Id = citizen.Id;
            citizenReformer.FullName = citizen.FullName;
            citizenReformer.Mobile = citizen.Mobile;
            citizenReformer.AcademicDegreeName = citizen.CitizenRequests?
                .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Reformer)?
                .OrderBy(x => x.RequestOn).LastOrDefault()?.AcademicDegreeName;
            citizenReformer.IsVolunteerApproval = citizen.CitizenRequests?
               .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Reformer)?
               .OrderBy(x => x.RequestOn).LastOrDefault()?.IsVolunteerApproval;
            citizenReformer.Fax = citizen.Fax;
            citizenReformer.HomeTelephone = citizen.HomeTelephone;
            citizenReformer.WorkTelephone = citizen.WorkTelephone;
            citizenReformer.IdentityNumber = citizen.IdentityNumber;
            citizenReformer.ReformerStatus = citizen.SysReformerStatus?.StatusName;
            citizenReformer.ReformerStatusId = citizen.ReformerStatusId;
            citizenReformer.Email = citizen.Email;
            citizenReformer.RequestsCount = citizen.ReformerLitigationRequests.Count(x => x.IsFinished == false);
            citizenReformer.ReformerStatusDate = citizen.ReformerStopFreeUpDecisions?.OrderBy(x => x.DecisionDate).LastOrDefault()?.DecisionDate;
            if (citizen.ReformerStopFreeUpDecisions != null)
            {
                foreach (var decision in citizen.ReformerStopFreeUpDecisions)
                {
                    ReformerStopFreeUpDetailDecisionDto reformerStopFreeUpDetailDecisionDto = new ReformerStopFreeUpDetailDecisionDto();
                    reformerStopFreeUpDetailDecisionDto.DecisionDate = decision.DecisionDate;
                    reformerStopFreeUpDetailDecisionDto.ReformerStatus = decision.SysReformerStatus?.StatusName;
                    reformerStopFreeUpDetailDecisionDto.StoppingReason = decision.StoppingReason;
                    reformerStopFreeUpDetailDecisionDto.LateLitigationRequestNumber = decision.LateLitigationRequestNumber;
                    citizenReformer.ReformerStopFreeUpDetailDecisionDtos.Add(reformerStopFreeUpDetailDecisionDto);
                }
            }
            return new DataResponse<CitizenReformerDetailShortDto>()
            {
                IsDone = true,
                Data = citizenReformer
            };

        }

        public async Task<BaseResponse> GetArbitratorsDecisions(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            PagedResult<Citizen> citizens = null;

            var entitiesQuerable = (await _repository.GetQueryable<Citizen, Guid>())
                                      .Include(x => x.ArbitratorStopFreeUpDecisions)
                       .Include(x => x.SysArbitratorStatus);


            citizens = entitiesQuerable.Where(x => x.IsArbitrator == true)
                                        .OrderByDescending(x => x.FullName)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .GetPaged(pageIndex, pageSize);


            List<CitizenArbitratorShortDto> citizenArbitrators = new List<CitizenArbitratorShortDto>();

            foreach (var citizen in citizens.Results)
            {
                CitizenArbitratorShortDto citizenArbitrator = new CitizenArbitratorShortDto();
                citizenArbitrator.Id = citizen.Id;
                citizenArbitrator.FullName = citizen.FullName;
                citizenArbitrator.Mobile = citizen.Mobile;
                citizenArbitrator.IdentityNumber = citizen.IdentityNumber;
                citizenArbitrator.ArbitratorStatus = citizen.SysArbitratorStatus?.StatusName;
                citizenArbitrator.ArbitratorClass = citizen.CitizenRequests?.OrderBy(x => x.RequestOn).LastOrDefault()?.SysArbitratorClass?.ArbitratorClassName;
                citizenArbitrator.ExperienceYears = citizen.CitizenRequests?.OrderBy(x => x.RequestOn).LastOrDefault()?.ExperienceYears;
                citizenArbitrator.Email = citizen.Email;
                citizenArbitrator.RequestsCount = 0;
                citizenArbitrator.ArbitratorStatusDate = citizen.ArbitratorStopFreeUpDecisions?.OrderBy(x => x.DecisionDate).LastOrDefault()?.DecisionDate;
                citizenArbitrators.Add(citizenArbitrator);

            }


            return new DataResponse<PagedResult<CitizenArbitratorShortDto>>
            {
                Data =
                   new PagedResult<CitizenArbitratorShortDto>
                   {
                       Results = citizenArbitrators,
                       CurrentPage = citizens.CurrentPage,
                       PageCount = citizens.PageCount,
                       PageSize = citizens.PageSize,
                       TotalCount = citizens.TotalCount
                   },
                IsDone = true
            };






            return new DataResponse<List<CitizenArbitratorShortDto>>()
            {
                IsDone = true,
                Data = citizenArbitrators.OrderBy(x => x.RequestsCount).ToList()
            };

        }
        public async Task<BaseResponse> GetArbitratorsDecisions(Guid citizenId)
        {
            var citizen = (await _repository.GetQueryable<Citizen, Guid>())
                         .Include(x => x.SysArbitratorStatus)
                         .Include(x => x.ArbitratorStopFreeUpDecisions)
                        .ThenInclude(x => x.SysArbitratorStatus)
                         .Include(x => x.CitizenRequests)
                         .ThenInclude(x => x.SysArea)
                            .Include(x => x.CitizenRequests)
                         .ThenInclude(x => x.SysArbitratorClass)
                         .Where(x => x.Id == citizenId).SingleOrDefault();

            if (citizen == null)
                return new BaseResponse() { IsDone = false, Message = "حساب غير موجود" };

            CitizenArbitratorDetailShortDto citizenArbitrator = new CitizenArbitratorDetailShortDto();
            citizenArbitrator.Id = citizen.Id;
            citizenArbitrator.FullName = citizen.FullName;
            citizenArbitrator.Mobile = citizen.Mobile;
            citizenArbitrator.AcademicDegreeName = citizen.CitizenRequests?
                .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator)?
                .OrderBy(x => x.RequestOn).LastOrDefault()?.AcademicDegreeName;
            citizenArbitrator.IsVolunteerApproval = citizen.CitizenRequests?
               .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator)?
               .OrderBy(x => x.RequestOn).LastOrDefault()?.IsVolunteerApproval;
            citizenArbitrator.Fax = citizen.Fax;
            citizenArbitrator.HomeTelephone = citizen.HomeTelephone;
            citizenArbitrator.AreaName = citizen.CitizenRequests?.OrderBy(x => x.RequestOn).LastOrDefault()?.SysArea?.AreaName;
            citizenArbitrator.ArbitratorClass = citizen.CitizenRequests?.OrderBy(x => x.RequestOn).LastOrDefault()?.SysArbitratorClass?.ArbitratorClassName;
            citizenArbitrator.ExperienceYears = citizen.CitizenRequests?.OrderBy(x => x.RequestOn).LastOrDefault()?.ExperienceYears;
            citizenArbitrator.Brief = citizen.CitizenRequests?.OrderBy(x => x.RequestOn).LastOrDefault()?.Brief;
            citizenArbitrator.WorkTelephone = citizen.WorkTelephone;
            citizenArbitrator.IdentityNumber = citizen.IdentityNumber;
            citizenArbitrator.ArbitratorStatus = citizen.SysArbitratorStatus?.StatusName;
            citizenArbitrator.ArbitratorStatusId = citizen.ArbitratorStatusId;
            citizenArbitrator.Email = citizen.Email;
            citizenArbitrator.RequestsCount = 0;
            citizenArbitrator.ArbitratorStatusDate = citizen.ArbitratorStopFreeUpDecisions?.OrderBy(x => x.DecisionDate).LastOrDefault()?.DecisionDate;
            if (citizen.ArbitratorStopFreeUpDecisions != null)
            {
                foreach (var decision in citizen.ArbitratorStopFreeUpDecisions)
                {
                    ArbitratorStopFreeUpDetailDecisionDto ArbitratorStopFreeUpDetailDecisionDto = new ArbitratorStopFreeUpDetailDecisionDto();
                    ArbitratorStopFreeUpDetailDecisionDto.DecisionDate = decision.DecisionDate;
                    ArbitratorStopFreeUpDetailDecisionDto.ArbitratorStatus = decision.SysArbitratorStatus?.StatusName;
                    ArbitratorStopFreeUpDetailDecisionDto.StoppingReason = decision.StoppingReason;
                    ArbitratorStopFreeUpDetailDecisionDto.LateLitigationRequestNumber = decision.LateLitigationRequestNumber;
                    citizenArbitrator.ArbitratorStopFreeUpDetailDecisionDtos.Add(ArbitratorStopFreeUpDetailDecisionDto);
                }
            }
            return new DataResponse<CitizenArbitratorDetailShortDto>()
            {
                IsDone = true,
                Data = citizenArbitrator
            };

        }
    }



}
