using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.EntityFrameworkCore.Entities;

namespace REAC.Application.Contracts.IAppServices
{
    public interface ICitizenService
    {
        Task<DateTime?> GetIdentityExpirationDate(Guid? userId);
        Task<ICollection<Facility>> GetRepresentativeCitizens(Guid? userId);
        Task<(Guid, bool)> CheckCitizenExist(string identityNumber);
        Task<BaseResponse> GetCitizenBasicData();
        Task<BaseResponse> UpdateCitizenBasicData(CitizenBasicDataProfileDto citizenBasicDataProfileDto);


    }
}
