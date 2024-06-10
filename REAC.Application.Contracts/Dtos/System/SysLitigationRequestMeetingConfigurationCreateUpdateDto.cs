using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class SysLitigationRequestMeetingConfigurationCreateUpdateDto : BaseEntity<Guid>
{
    public DateTime From { get; set; }

    public DateTime To { get; set; }

}
