using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.System
{
    public class SysBank : BaseEntity<Guid>
    {
        public string BankName { get; set; } = null!;

        public virtual ICollection<CitizenBankAccount> CitizenBankAccounts { get; set; } = new List<CitizenBankAccount>();

    }
}
