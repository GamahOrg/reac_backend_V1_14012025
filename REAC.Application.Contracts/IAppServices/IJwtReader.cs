namespace REAC.Application.Contracts.IAppServices
{
    public interface IJwtReader
    {
        public bool GetCustomerId(ref Guid customerId);
        public bool GetEmployeeId(ref Guid employeeId);
        public bool GetUserId(ref Guid? userId);
        public Guid? GetUserId();
    }
}
