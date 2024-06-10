using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.Application.Contracts.Dtos.Wpf.Business.WorkFlowProcessingLogFile;

public partial class WorkFlowProcessingLogFileCreateUpdateDto : BaseEntity<Guid>
{
    public Guid FileId { get; set; }

}
