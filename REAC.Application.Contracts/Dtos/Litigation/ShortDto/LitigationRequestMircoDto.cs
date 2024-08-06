using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Litigation;

public partial class LitigationRequestMircoDto : BaseEntity<Guid>
{
    public string? RequestCode { get; set; }
    public DateTime RequestDate { get; set; }
    public string FirstPartyName { get; set; } = null!;
    public string SecondPartyName { get; set; } = null!;
    public string RequestTypeName { get; set; } = null!;
    public string? Status { get; set; } = null!;
    public string IsDone { get; set; } = null!;

}

public partial class LitigationRequestMircoForArbitratorsCountDto : BaseEntity<Guid>
{
    public string? RequestCode { get; set; }
    public DateTime RequestDate { get; set; }
    public string FirstPartyName { get; set; } = null!;
    public string SecondPartyName { get; set; } = null!;
    public string RequestTypeName { get; set; } = null!;
    public string? Status { get; set; } = null!;
    public int? ArbitratorsCount { get; set; }
    public string IsDone { get; set; } = null!;
}
public partial class LitigationRequestMircoForAllArbitratorsCountDto : BaseEntity<Guid>
{
    public string? RequestCode { get; set; }
    public DateTime RequestDate { get; set; }
    public string FirstPartyName { get; set; } = null!;
    public string SecondPartyName { get; set; } = null!;
    public string RequestTypeName { get; set; } = null!;
    public string? Status { get; set; } = null!;
    public int? FirstPartyArbitratorsCount { get; set; }
    public int? SecondPartyArbitratorsCount { get; set; }
    public int? ArbitratorsCount { get; set; }
    public string IsDone { get; set; } = null!;
}
public partial class LitigationRequestMircoForArbitratorsSelectionDurationDto : BaseEntity<Guid>
{
    public string? RequestCode { get; set; }
    public DateTime RequestDate { get; set; }
    public string FirstPartyName { get; set; } = null!;
    public string SecondPartyName { get; set; } = null!;
    public string RequestTypeName { get; set; } = null!;
    public string? Status { get; set; } = null!;
    public int? ArbitratorsSelectionDuration { get; set; }
    public string IsDone { get; set; } = null!;
}
public partial class LitigationRequestMircoForRequestSessionDto : BaseEntity<Guid>
{
    public string? RequestCode { get; set; }
    public DateTime RequestDate { get; set; }
    public string FirstPartyName { get; set; } = null!;
    public string SecondPartyName { get; set; } = null!;
    public string RequestTypeName { get; set; } = null!;
    public DateTime SessionDatetime { get; set; }
    public string? Url { get; set; }
}

public partial class LitigationRequestMircoForRequestReformerDto : BaseEntity<Guid>
{
    public string? RequestCode { get; set; }
    public DateTime RequestDate { get; set; }
    public string FirstPartyName { get; set; } = null!;
    public string SecondPartyName { get; set; } = null!;
    public string RequestTypeName { get; set; } = null!;
    public Guid? ReformerId { get; set; }

    public string? ReformerName { get; set; }
}
