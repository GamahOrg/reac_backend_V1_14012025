using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Reconciliation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class LitigationRequest : BaseEntity<Guid>
{
    public string? RequestCode { get; set; }

    public Guid ContractId { get; set; }
    public Guid? CityId { get; set; }
    public Guid? AreaId { get; set; }
    public string? ContractAddress { get; set; } // العنوان الوطني
    public Guid ApplicantCitizenId { get; set; }

    public bool IsAllowEdit { get; set; }

    public int? FirstPartyArbitratorsCount { get; set; }

    public int? SecondPartyArbitratorsCount { get; set; }
    public int? ArbitratorsCount { get; set; }
    public Guid? ReformerCitizenId { get; set; }
    public LitigationRequestCategory? LitigationRequestCategoryId { get; set; }

    public LitigationRequestOwnerType LitigationRequestOwnerTypeId { get; set; }

    public RequestStatus InternalStatusId { get; set; }

    public RequestStatus ExternalStatusId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool? IsFinancialLitigation { get; set; }

    public Guid? LitigationRequestAmountId { get; set; }

    public decimal Fees { get; set; } = 0;

    public bool IsPaid { get; set; } = false;

    public DateTime? PaidOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public bool IsSent { get; set; }

    public DateTime? SentOn { get; set; }
    public bool? IsActive { get; set; }

    public bool? IsFinished { get; set; }

    public bool? IsExemptionReduction { get; set; }

    public Guid? ExemptionReductionCategoryId { get; set; }

    public Guid? ExemptionReductionFileId { get; set; }

    public virtual SysFileManager? ExemptionReductionFile { get; set; }


    public int? PretenderRating { get; set; }
    public int? DefendantRating { get; set; }

    public virtual Citizen ApplicantCitizen { get; set; } = null!;
    public virtual SysCity City { get; set; } = null!;
    public virtual SysArea Area { get; set; } = null!;
    public virtual Citizen? ReformerCitizen { get; set; }

    public virtual LitigationRequestContract Contract { get; set; } = null!;

    public virtual SysLitigationRequestStatus ExternalStatus { get; set; } = null!;

    public virtual SysLitigationRequestStatus InternalStatus { get; set; } = null!;

    public virtual SysLitigationRequestAmount LitigationRequestAmount { get; set; } = null!;

    public virtual SysLitigationRequestCategory? LitigationRequestCategory { get; set; }

    public virtual SysExemptionReductionCategory? SysExemptionReductionCategory { get; set; }

    public virtual ICollection<LitigationRequestCorrespondence> LitigationRequestCorrespondences { get; set; } = new List<LitigationRequestCorrespondence>();

    public virtual ICollection<LitigationRequestExtendPeriodRequest> LitigationRequestExtendPeriodRequests { get; set; } = new List<LitigationRequestExtendPeriodRequest>();

    public virtual ICollection<LitigationRequestGround> LitigationRequestGrounds { get; set; } = new List<LitigationRequestGround>();

    public virtual ICollection<LitigationRequestReconciliationMeeting> LitigationRequestReconciliationMeetings { get; set; } = new List<LitigationRequestReconciliationMeeting>();

    public virtual ICollection<LitigationRequestNote> LitigationRequestNotes { get; set; } = new List<LitigationRequestNote>();

    public virtual SysLitigationRequestOwnerType LitigationRequestOwnerType { get; set; } = null!;

    public virtual ICollection<LitigationRequestRequest> LitigationRequestRequests { get; set; } = new List<LitigationRequestRequest>();

}
