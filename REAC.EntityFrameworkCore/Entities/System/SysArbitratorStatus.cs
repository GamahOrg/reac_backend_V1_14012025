using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;

namespace REAC.EntityFrameworkCore.Entities.System
{
    public class SysArbitratorStatus : BaseEntity<ArbitratorStatus>
    {
        public string StatusName { get; set; } = null!;
        public virtual ICollection<SysArbitratorStatusTranslation> Translations { get; set; } = new List<SysArbitratorStatusTranslation>();
        public virtual ICollection<Citizen> Citizens { get; set; } = new List<Citizen>();
        public virtual ICollection<ArbitratorStopFreeUpDecision> ArbitratorStopFreeUpDecisions { get; set; } = new List<ArbitratorStopFreeUpDecision>();

    }
}
