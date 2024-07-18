using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class CitizenRequest : BaseEntity<Guid>
{
    //توع الطلب
    public CitizenRequestType CitizenRequestTypeId { get; set; }

    public Guid CitizenId { get; set; }
    public Guid AreaId { get; set; }
    public bool IsVolunteerRepairmanEndorsement { get; set; }
    public string AcademicDegreeName { get; set; } = null!;
    public int ExperienceYears { get; set; }
    public bool? IsApproved { get; set; }
    public Guid? ArbitratorClassId { get; set; }
    public RequestStatus InternalStatusId { get; set; }

    public RequestStatus ExternalStatusId { get; set; }
    public string? Brief { get; set; }
    public virtual SysCitizenRequestType SysCitizenRequestType { get; set; } = null!;
    public virtual SysArbitratorClass? SysArbitratorClass { get; set; }

    public virtual Citizen Citizen { get; set; } = null!;
    public virtual SysArea SysArea { get; set; } = null!;
    public virtual ICollection<CitizenRequestAcademicDegreeFile> CitizenRequestAcademicDegreeFiles { get; set; } = new List<CitizenRequestAcademicDegreeFile>();
    public virtual ICollection<CitizenRequestFreelanceWorkDocumentFile> CitizenRequestFreelanceWorkDocumentFiles { get; set; } = new List<CitizenRequestFreelanceWorkDocumentFile>();
    public virtual ICollection<CitizenRequestExperienceCertificate> CitizenRequestExperienceCertificates { get; set; } = new List<CitizenRequestExperienceCertificate>();





}



