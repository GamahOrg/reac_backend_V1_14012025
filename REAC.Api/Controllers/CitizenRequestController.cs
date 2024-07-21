using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.CitizenRequests;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Constants;

namespace REAC.Api.Controllers.Litigation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Public)]
[PermissionRequired(Modules.Delegator)]

public class CitizenRequestController : ControllerBase
{


    private readonly ICitizenRequestService _citizenRequestService;
    public CitizenRequestController(ICitizenRequestService citizenRequestService)
    {
        _citizenRequestService = citizenRequestService;
    }

    [HttpGet(nameof(GetCitizenRequest))]
    public async Task<ActionResult> GetCitizenRequest()
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenRequestService.GetCitizenRequest();
            return Ok(returnValue);
        }
        return BadRequest();

    }
    [HttpGet(nameof(GetCitizenRequestById))]
    public async Task<ActionResult> GetCitizenRequestById(Guid requestById)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenRequestService.GetCitizenRequestById(requestById);
            return Ok(returnValue);
        }
        return BadRequest();

    }
    [HttpPost(nameof(SaveCitizenRequest))]
    public async Task<ActionResult> SaveCitizenRequest(CitizenRequestCreateUpdateDto citizenRequestCreateUpdateDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenRequestService.SaveCitizenRequest(citizenRequestCreateUpdateDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpPost(nameof(ExecuteAction))]
    public async Task<ActionResult> ExecuteAction(Domain.Shared.Enum.CitizenRequestAction citizenRequestAction, Guid requestId, string? Notes)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenRequestService.ExecuteAction(citizenRequestAction, requestId, Notes);
            return Ok(returnValue);
        }

        return BadRequest();
    }
}