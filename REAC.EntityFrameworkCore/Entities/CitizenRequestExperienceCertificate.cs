using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class CitizenRequestExperienceCertificate : BaseEntity<Guid>
{
    public Guid CitizenRequestId { get; set; }

    public string CertificateName { get; set; } = null!;
    public DateTime AcquisitionDate { get; set; }
    public string IssuingEntityName { get; set; } = null!;
    public virtual CitizenRequest CitizenRequest { get; set; } = null!;
    public virtual ICollection<CitizenRequestExperienceCertificateFile> CitizenRequestExperienceCertificateFiles { get; set; } = new List<CitizenRequestExperienceCertificateFile>();


}
