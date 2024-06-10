using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class Facility
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public Guid CitizenId { get; set; }

    public string Facility700Number { get; set; } = null!;

    public string? CommercialRegisterNumber { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public string City { get; set; } = null!;

    public string District { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string? PostalCode { get; set; }

    public string? SubNumber { get; set; }

    public string BuildingNumber { get; set; } = null!;

    public string UnitNumber { get; set; } = null!;

    public virtual Citizen Citizen { get; set; } = null!;

    public virtual ICollection<FacilityDelegator> FacilityDelegators { get; set; } = new List<FacilityDelegator>();

    public virtual ICollection<LitigationRequestContract> LitigationRequestContractFirstPartyFacilities { get; set; } = new List<LitigationRequestContract>();

    public virtual ICollection<LitigationRequestContract> LitigationRequestContractSecondPartyFacilities { get; set; } = new List<LitigationRequestContract>();
}
