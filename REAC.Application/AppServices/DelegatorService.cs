using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Delegator;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Seed.Integration;

namespace REAC.Application.AppServices
{
    public class DelegatorService : IDelegatorService
    {
        protected IEntityRepository _repository;
        protected IMapper _mapper;
        protected ILoggerManager _loggerManager;
        private readonly IJwtReader _jwtReader;
        public DelegatorService(IEntityRepository repository,
            IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader)
        {
            _repository = repository;
            _mapper = mapper;
            _jwtReader = jwtReader;
            _loggerManager = loggerManager;
        }


        public async Task<BaseResponse> GetDelegators()
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

            return new DataResponse<List<DelegatorShortDataDto>>()
            {
                IsDone = true,
                Data = fullDelegators
            };
        }
        public async Task<BaseResponse> GetDelegatorById(Guid delegatorId)
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
                    Message = "Not found"
                };
            }
        }
        public async Task<BaseResponse> SaveDelegator(DelegatorDto delegatorDto)
        {
            if (delegatorDto.DelegationRelationshipTypeId == DelegationRelationshipType.Lawyer)
            {
                // delegatorDto.DelegateNumber
                // Implement Lawyer Integration
            }
            else
            {
                // delegatorDto.DelegateNumber
                // Implement Agent Integration
            }
            if (delegatorDto.Id != Guid.Empty)
            {
                var updatedCurrentCitizen = _mapper.Map<Citizen>(delegatorDto);
                var saveCurrentCitizen = _repository.Update<Citizen, Guid>(updatedCurrentCitizen);
                return saveCurrentCitizen;
            }
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var sourceCitizen = (await _repository.GetQueryable<Citizen, Guid>())
                          .Where(x => x.Id == referenceId)
                          .ToList();

            if (sourceCitizen.Count == 0)
                return new BaseResponse() { IsDone = false, Message = "Source Employee Not Found" };
            var citizen = (await _repository.GetQueryable<Citizen, Guid>())
                          .Where(x => x.IdentityNumber == delegatorDto.IdentityNumber)
                          .ToList();

            if (citizen.Count == 0)
            {
                var integrationCitizen = CitizenIntegration.GetCitizen().Where(t => t.Id == delegatorDto.IdentityNumber);
                if (!integrationCitizen.Any())
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "Identity Number Not Found"
                    };

                var nationality = (await _repository.GetQueryable<SysNationality, Guid>())
                          .Where(x => x.NationalityCode == integrationCitizen.FirstOrDefault().Nationality)
                          .ToList();
                if (!nationality.Any())
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "nationality Not Found"
                    };

                Citizen citizenFromintegration = new Citizen();
                citizenFromintegration.Id = Guid.NewGuid();
                citizenFromintegration.FullName = integrationCitizen.FirstOrDefault().FullNameAr;
                citizenFromintegration.ShortName = integrationCitizen.FirstOrDefault().FirstNameAr;
                citizenFromintegration.IdentityNumber = delegatorDto.IdentityNumber;
                citizenFromintegration.GenderId = (integrationCitizen.FirstOrDefault().Gender == "") ? Gender.Male : Gender.Female;
                citizenFromintegration.BirthDate = integrationCitizen.FirstOrDefault().DobG;
                citizenFromintegration.IdentityExpiryDate = integrationCitizen.FirstOrDefault().IdExpiryDateG;
                citizenFromintegration.NationalityId = nationality.FirstOrDefault().Id;

                if (delegatorDto.DelegationRelationshipTypeId == DelegationRelationshipType.Lawyer)
                {
                    citizenFromintegration.IsLawyer = true;
                    citizenFromintegration.LawyerLicenseNumber = delegatorDto.LawyerLicenseNumber;
                    citizenFromintegration.LawLicenseFileId = delegatorDto.LawLicenseFileId;
                }

                var response = await _repository.AddAsync<Citizen, Guid>(citizenFromintegration);
                if (!response.IsDone)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "Identity Number Not Found"
                    };
                var saveCitizenFromintegrationResponse = await _repository.SaveChangesAsync();
                if (!saveCitizenFromintegrationResponse.IsDone)
                    return saveCitizenFromintegrationResponse;

                delegatorDto.DelegatorCitizenId = citizenFromintegration.Id;
            }
            else
            {
                delegatorDto.DelegatorCitizenId = citizen.FirstOrDefault().Id;
            }

            var updatedCitizen = _mapper.Map<Delegator>(delegatorDto);
            updatedCitizen.CitizenId = referenceId;
            await _repository.AddAsync<Delegator, Guid>(updatedCitizen);
            var saveResponse = await _repository.SaveChangesAsync();
            return saveResponse;
        }
    }
}
