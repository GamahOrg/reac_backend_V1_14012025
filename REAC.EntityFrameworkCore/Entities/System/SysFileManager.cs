using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class SysFileManager : CreationBaseEntity<Guid>
{
    public string? FileDescription { get; set; }

    public string FilePath { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public virtual ICollection<Citizen> Citizens { get; set; } = new List<Citizen>();

    public virtual ICollection<DelegationRequest> DelegationRequests { get; set; } = new List<DelegationRequest>();

    public virtual ICollection<LitigationRequestContract> LitigationRequestContracts { get; set; } = new List<LitigationRequestContract>();

    public virtual ICollection<LitigationRequestCorrespondenceFile> LitigationRequestCorrespondenceFiles { get; set; } = new List<LitigationRequestCorrespondenceFile>();

    public virtual ICollection<LitigationRequestExtendPeriodRequestFile> LitigationRequestExtendPeriodRequestFiles { get; set; } = new List<LitigationRequestExtendPeriodRequestFile>();

    public virtual ICollection<LitigationRequestGroundFile> LitigationRequestGroundFiles { get; set; } = new List<LitigationRequestGroundFile>();

    public virtual ICollection<LitigationRequestNoteFile> LitigationRequestNoteFiles { get; set; } = new List<LitigationRequestNoteFile>();

    public virtual ICollection<LitigationRequestRequestFile> LitigationRequestRequestFiles { get; set; } = new List<LitigationRequestRequestFile>();
}
