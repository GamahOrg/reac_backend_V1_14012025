using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Api.Controllers.System;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.System)]
[PermissionRequired(Modules.LitigationRequestMeetingConfiguration)]

public class SysLitigationRequestMeetingConfigurationController : BaseController<SysLitigationRequestMeetingConfiguration,
       SysLitigationRequestMeetingConfigurationCreateUpdateDto, SysLitigationRequestMeetingConfigurationCreateUpdateDto
       , Guid>
{

    public SysLitigationRequestMeetingConfigurationController(ISysLitigationRequestMeetingConfigurationService sysLitigationRequestMeetingConfigurationService) :
        base(sysLitigationRequestMeetingConfigurationService)
    {

    }
}