namespace REAC.EntityFrameworkCore.Entities.System;

public partial class FacilityDelegatorCreateUpdateDto
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid FacilityId { get; set; }

    public virtual CitizenCreateUpdateDto Account { get; set; } = null!;

    public virtual FacilityCreateUpdateDto Facility { get; set; } = null!;
}
