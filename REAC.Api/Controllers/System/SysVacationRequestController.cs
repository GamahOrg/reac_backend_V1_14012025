using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Security.Attributes;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Api.Controllers.System;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.System)]
[PermissionRequired(Modules.VacationRequest)]

public class SysVacationRequestController : BaseController<SysVacationRequest,
       SysVacationRequestCreateUpdateDto, SysVacationRequestCreateUpdateDto
       , Guid>
{
    public SysVacationRequestController(ISysVacationRequestService sysSettingService) :
       base(sysSettingService)
    {


    }
}