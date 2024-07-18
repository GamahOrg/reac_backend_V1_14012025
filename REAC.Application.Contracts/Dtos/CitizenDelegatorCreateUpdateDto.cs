using REAC.Domain.Shared.Enum;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class CitizenDelegatorCreateUpdateDto
{
    public Guid Id { get; set; }

    public string? DelegateNumber { get; set; }

    public DelegationRelationshipType DelegationRelationshipTypeId { get; set; }

    public Guid? CitizenId { get; set; }

    public Guid DelegatorCitizenId { get; set; }

    public Guid DelegationFileId { get; set; }

    public bool IsActive { get; set; }

    public virtual CitizenCreateUpdateDto? Citizen { get; set; }

    public virtual CitizenCreateUpdateDto DelegatorCitizen { get; set; } = null!;

}
