namespace REAC.EntityFrameworkCore.Entities.System;

public partial class FacilityDelegator
{
    public Guid Id { get; set; }

    public Guid CitizenId { get; set; }

    public Guid FacilityId { get; set; }

    public bool IsActive { get; set; }

    public virtual Citizen Citizen { get; set; } = null!;

    public virtual Facility Facility { get; set; } = null!;
}
