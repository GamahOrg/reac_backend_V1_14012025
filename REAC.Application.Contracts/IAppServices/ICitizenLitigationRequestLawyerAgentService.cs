namespace REAC.Application.Contracts.IAppServices
{
    public interface ICitizenLitigationRequestLawyerAgentService
    {
        Task<List<string>> GetAgentCitizens(Guid? userId);
        Task<List<string>> GetLawyerCitizens(Guid? userId);
    }
}
