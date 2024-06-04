using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.Application.Contracts.Dtos.Wpf.System.WorkFlowStepEvent;

public class WorkFlowStepEventForActorShortDto : BaseEntity<Guid>
{
    public bool? AllowAssign { get; set; }

    public bool? AllowReAssign { get; set; }

    public List<WorkFlowStepEventShortDto> WorkFlowStepEventShortDtos { get; set; } = new List<WorkFlowStepEventShortDto>();
}
public class WorkFlowStepEventShortDto : BaseEntity<Guid>
{
    public string EventName { get; set; } = null!;

    public bool AllowFileUpload { get; set; }

    public bool AllowNotes { get; set; }

    public Guid ProcessingLogId { get; set; }


}
