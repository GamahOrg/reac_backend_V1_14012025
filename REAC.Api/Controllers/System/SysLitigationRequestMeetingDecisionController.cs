using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.System.LitigationRequestMeetingDecisionDtos;
using REAC.Domain.Shared;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.IAppServices.System;

namespace REAC.Api.Controllers.System;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.System)]
[PermissionRequired(Modules.LitigationRequestMeetingDecision)]

public class SysLitigationRequestMeetingDecisionController : BaseController<SysLitigationRequestMeetingDecision,
       SysLitigationRequestMeetingDecisionCreateUpdateDto, SysLitigationRequestMeetingDecisionDto
       , LitigationRequestMeetingDecision>
{

    public SysLitigationRequestMeetingDecisionController(ISysLitigationRequestMeetingDecisionService sysLitigationRequestMeetingDecisionService) :
        base(sysLitigationRequestMeetingDecisionService)
    {

    }
}
