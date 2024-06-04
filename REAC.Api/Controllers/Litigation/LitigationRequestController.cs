using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Domain.Shared;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.Api.Controllers.Litigation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Litigation)]
//[PermissionRequired(Modules.LitigationRequest)]
[AllowAnonymous]

public class LitigationRequestController : BaseController<LitigationRequest,
       LitigationCreateUpdateDto, LitigationCreateUpdateDto
       , Guid>
{
    private readonly ILitigationRequestService _litigationService;
    public LitigationRequestController( ILitigationRequestService litigationRequestService, ILitigationRequestService litigationService) :
        base(litigationRequestService)
    {
        _litigationService = litigationService;
    }
    [HttpPost("CreateLitigation")]
    public override async Task<IActionResult> Create([FromBody]LitigationCreateUpdateDto model,
          CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.Add(model);
            return Ok(returnValue);
        }

        return BadRequest();
    }
}