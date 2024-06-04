namespace REAC.Application.Contracts.IAppServices
{
    public interface ICitizenService
    {
        Task<DateTime?> GetIdentityExpirationDate(Guid? userId);
        Task<List<string>> GetRepresentativeCitizens(Guid? userId);
    }
}
