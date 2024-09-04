using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Delegator;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Constants;

namespace REAC.Api.Controllers.Litigation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Public)]
[PermissionRequired(Modules.Delegator)]

public class DelegatorController : ControllerBase
{
    private readonly IDelegatorService _delegatorService;
    public DelegatorController(IDelegatorService delegatorService)
    {
        _delegatorService = delegatorService;
    }

    [HttpGet(nameof(GetDelegators))]
    public async Task<ActionResult> GetDelegators([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? searchKey)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _delegatorService.GetDelegators(pageIndex, pageSize, searchKey);
            return Ok(returnValue);
        }
        return BadRequest();

    }
    [HttpGet(nameof(GetDelegatorById))]
    public async Task<ActionResult> GetDelegatorById(Guid delegatorId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _delegatorService.GetDelegatorById(delegatorId);
            return Ok(returnValue);
        }
        return BadRequest();

    }
    [HttpPost(nameof(SaveDelegator))]
    public async Task<ActionResult> SaveDelegator(DelegatorDto delegatorDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _delegatorService.SaveDelegator(delegatorDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpPost(nameof(ValidateDelegator))]
    public async Task<ActionResult> ValidateDelegator(ValidateDelegatorDto validateDelegatorDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _delegatorService.ValidateDelegator(validateDelegatorDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }
}