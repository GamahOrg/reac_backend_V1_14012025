using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.Wpf.Business;

public partial class WorkFlowProcessingLogFile : BaseEntity<Guid>
{
    public Guid WorkFlowProcessingLogId { get; set; }

    public Guid FileId { get; set; }

    public virtual WorkFlowProcessingLog WorkFlowProcessingLog { get; set; } = null!;
}
