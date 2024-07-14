namespace REAC.Application.Contracts.IAppServices.Integration.Hubs.Contracts
{
    public class IdentityStatus
    {
        public string TransactionId { get; set; }
        public bool IsValid { get; set; }
    }
}
