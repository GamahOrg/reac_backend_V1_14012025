using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class LitigationRequestContract : BaseEntity<Guid>
{
    public LitigationRequestContractType LitigationRequestContractTypeId { get; set; }

    public string ContractKey { get; set; } = null!; // contract number

    public string? VersionNumber { get; set; }

    public Guid LeaseContractFileId { get; set; } // contract file Id 

    public Guid FirstPartyCitizenId { get; set; }

    public Guid? FirstPartyFacilityId { get; set; }

    public string FirstPartyCapacity { get; set; } = null!;

    public Guid? SecondPartyCitizenId { get; set; }

    public Guid? SecondPartyFacilityId { get; set; }

    public string? SecondPartyName { get; set; }

    public string SecondPartyCapacity { get; set; } = null!;

    public string? SecondPartyIdentityNumber { get; set; }

    public string? SecondPartyEmail { get; set; }

    public string? SecondPartyMobile { get; set; }


    public Guid? SecondPartyRegistrationRequestId { get; set; }

    public virtual Citizen FirstPartyCitizen { get; set; } = null!;

    public virtual Facility FirstPartyFacility { get; set; } = null!;

    public virtual SysFileManager LeaseContractFile { get; set; } = null!;

    public virtual SysLitigationRequestContractType LitigationRequestContractType { get; set; } = null!;

    public virtual ICollection<LitigationRequest> LitigationRequests { get; set; } = new List<LitigationRequest>();

    public virtual SysRegistrationRequest? SecondPartyRegistrationRequest { get; set; }

    public virtual Citizen? SecondPartyCitizen { get; set; }

    public virtual Facility? SecondPartyFacility { get; set; }
}
