using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class Citizen : BaseEntity<Guid>
{

    public string IdentityNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public Guid NationalityId { get; set; }
    public string ShortName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public Gender GenderId { get; set; }
    public DateTime IdentityExpiryDate { get; set; }



    public string? Code { get; set; }

    public IdentityType? IdentityTypeId { get; set; }

    public Guid? UserId { get; set; }

    public string? NationalAddress { get; set; }


    public Guid? PassCitizenId { get; set; }

    public string? Otp { get; set; }



    public Guid? IdentityFileId { get; set; }

    public string? Employer { get; set; } = null!;

    public string? Mobile { get; set; } = null!;

    public string? HomeTelephone { get; set; }

    public string? WorkTelephone { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; } = null!;

    public Guid? AreaId { get; set; }

    public Guid? CityId { get; set; }

    public string? District { get; set; } = null!;

    public string? Street { get; set; } = null!;

    public string? PostalCode { get; set; }

    public string? SubNumber { get; set; }

    public string? BuildingNumber { get; set; } = null!;

    public string? UnitNumber { get; set; } = null!;

    public bool? IsLawyer { get; set; } = false;

    public bool? IsReformer { get; set; } = false;

    public bool? IsArbitrator { get; set; } = false;

    public string? LawyerLicenseNumber { get; set; }

    public DateTime? LawyerLicenseExpirationDate { get; set; }

    public Guid? LawLicenseFileId { get; set; }

    public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    public virtual ICollection<Delegator> FromDelegators { get; set; } = new List<Delegator>();

    public virtual ICollection<Delegator> ToDelegators { get; set; } = new List<Delegator>();

    public virtual SysGender Gender { get; set; } = null!;

    public virtual SysCity City { get; set; } = null!;

    public virtual SysArea Area { get; set; } = null!;

    public virtual SysNationality Nationality { get; set; } = null!;

    public virtual SysFileManager? IdentityFile { get; set; }

    public virtual SysFileManager? LawLicenseFile { get; set; }
    public virtual ICollection<LitigationRequest> ReformerLitigationRequests { get; set; } = new List<LitigationRequest>();

    public virtual SysIdentityType? IdentityType { get; set; }

    public virtual ICollection<LitigationRequestContract> LitigationRequestContractFirstPartyCitizens { get; set; } = new List<LitigationRequestContract>();

    public virtual ICollection<LitigationRequestContract> LitigationRequestContractSecondPartyCitizens { get; set; } = new List<LitigationRequestContract>();

    public virtual ICollection<LitigationRequest> LitigationRequests { get; set; } = new List<LitigationRequest>();

    public virtual ICollection<SysRegistrationRequest> SysRegistrationRequestCitizens { get; set; } = new List<SysRegistrationRequest>();

    public virtual ICollection<SysRegistrationRequest> SysRegistrationRequestSourceCitizens { get; set; } = new List<SysRegistrationRequest>();

    // Foreign key for LitigationCompanyRepresentative
    public Guid? LitigationCompanyRepresentativeId { get; set; }
    public virtual LitigationCompanyRepresentative LitigationCompanyRepresentative { get; set; }

}
