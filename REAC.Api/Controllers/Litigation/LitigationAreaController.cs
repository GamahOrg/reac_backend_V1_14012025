using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.AppServices.Litigation;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.List;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.Api.Controllers.Litigation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Litigation)]
//[PermissionRequired(Modules.LitigationRequest)]
[AllowAnonymous]

public class LitigationAreaController : BaseController<LitigationArea,
       LitigationAreaDto, LitigationAreaDto
       , Guid>
{
    private readonly ILitigationAreaService _litigationAreaService;
    public LitigationAreaController(ILitigationAreaService litigationRequestService, ILitigationAreaService litigationService) :
         base(litigationRequestService)
    {
        _litigationAreaService = litigationService;
    }
    [HttpGet("Get")]
    public override Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return base.Get(cancellationToken);
    }
   
   
}