using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.Application.Contracts.IAppServices;
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
                    .Include(x => x.Nationality)
                    .AsNoTracking().Where(x => x.Id == referenceId)
                              .ToList();


            if (citizen.ToList().Count == 0) return new BaseResponse() { IsDone = false, Message = "Not Found" };


            var citizendto = _mapper.Map<CitizenBasicDataProfileDto>(citizen.FirstOrDefault());
            return new DataResponse<CitizenBasicDataProfileDto>()
            {
                IsDone = true,
                Data = citizendto
            };

        }

        public async Task<BaseResponse> UpdateCitizenBasicData(CitizenBasicDataProfileDto citizenBasicDataProfileDto)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var response = await _repository.GetByIdAsync<Citizen, Guid>(referenceId, default);

            if (response.IsDone != true)
                return response;

            var citizen = ((DataResponse<Citizen>)response).Data;
            citizen.NationalAddress = citizenBasicDataProfileDto.NationalAddress;
            citizen.ShortName = citizenBasicDataProfileDto.ShortName;
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
            citizen.HomeTelephone = citizenBasicDataProfileDto.HomeTelephone;
            citizen.WorkTelephone = citizenBasicDataProfileDto.WorkTelephone;

            var saveResponse = _repository.SaveChangesAsync().Result;
            if (saveResponse.IsDone != true)
                return response;

            var updatedResponse = await _repository.GetByIdAsync<Citizen, Guid>(referenceId, default);
            if (updatedResponse.IsDone != true)
                return response;

            var updatedCitizen = _mapper.Map<CitizenBasicDataProfileDto>(((DataResponse<Citizen>)updatedResponse).Data);
            return new DataResponse<CitizenBasicDataProfileDto>()
            {
                IsDone = true,
                Data = updatedCitizen
            };

        }
    }
}
