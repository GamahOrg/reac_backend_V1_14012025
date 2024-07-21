using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.Application.Contracts.Dtos.Delegator
{

    public class DelegatorShortDataDto : BaseEntity<Guid>
    {
        public string? DelegateNumber { get; set; }

        public bool IsActive { get; set; }

        public virtual string DelegatorName { get; set; } = null!;

        public virtual string DelegationRelationshipTypeName { get; set; } = null!;

        public virtual string FacilityName { get; set; }

    }
}
