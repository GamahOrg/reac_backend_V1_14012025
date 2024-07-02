using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.Contracts.IAppServices
{
    public interface ICitizenService
    {
        Task<DateTime?> GetIdentityExpirationDate(Guid? userId);
        Task<ICollection<Facility>> GetRepresentativeCitizens(Guid? userId);
        Task<(Guid, bool)> CheckCitizenExist(string identityNumber);
    }
}
