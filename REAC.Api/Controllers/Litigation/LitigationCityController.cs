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

public class LitigationCityController : BaseController<LitigationCity,
       LitigationCityDto, LitigationCityDto
       , Guid>
{
    private readonly ILitigationCityService _litigationCityService;
    public LitigationCityController(ILitigationCityService litigationRequestService, ILitigationCityService litigationService) :
         base(litigationRequestService)
    {
        _litigationCityService = litigationService;
    }
    [HttpGet("Get")]
    public override Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return base.Get(cancellationToken);
    }
}