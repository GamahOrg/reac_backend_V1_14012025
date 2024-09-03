using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class SysSettingCreateUpdateDto : BaseEntity<Guid>
{
    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;
}
