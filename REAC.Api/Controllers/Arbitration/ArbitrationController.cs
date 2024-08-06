using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Arbitration;
using REAC.Application.ModuleServices;
using REAC.Domain.Shared.Constants;

namespace REAC.Api.Controllers.Arbitration.Process;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Arbitration)]
[PermissionRequired(Modules.Arbitration)]
[AllowAnonymous]

public class ArbitrationController : ControllerBase
{
    private readonly IArbitrationService _ArbitrationService;
    public ArbitrationController(IArbitrationService ArbitrationService)
    {
        _ArbitrationService = ArbitrationService;
    }


    #region ArbitratorsCount
    [HttpPost(nameof(AddArbitratorsCount))]
    public async Task<IActionResult> AddArbitratorsCount(ArbitratorCountRequestPayload arbitratorCountRequestPayload, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _ArbitrationService.AddArbitratorsCount(arbitratorCountRequestPayload, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetLitigationRequestForAddArbitratorsCount))]
    public async Task<IActionResult> GetLitigationRequestForAddArbitratorsCount(CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _ArbitrationService.GetLitigationRequestForAddArbitratorsCount(cancellationToken);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpGet(nameof(GetLitigationRequestForAddArbitratorsCountByRequestId))]
    public async Task<IActionResult> GetLitigationRequestForAddArbitratorsCountByRequestId(Guid requestId, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _ArbitrationService.GetLitigationRequestForAddArbitratorsCount(requestId, cancellationToken);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(AddArbitratorsCount))]
    public async Task<IActionResult> AddCitizenLitigationRequestForReportArbitratorsCountReply(ArbitratorCountRequestPayload arbitratorCountRequestPayload, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _ArbitrationService.AddCitizenLitigationRequestForReportArbitratorsCountReply(arbitratorCountRequestPayload, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetCitizenLitigationRequestForArbitratorsCountReply))]
    public async Task<IActionResult> GetCitizenLitigationRequestForArbitratorsCountReply(CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _ArbitrationService.GetCitizenLitigationRequestForArbitratorsCountReply(cancellationToken);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpGet(nameof(GetCitizenLitigationRequestForArbitratorsCountReplyRequestId))]
    public async Task<IActionResult> GetCitizenLitigationRequestForArbitratorsCountReplyRequestId(Guid requestId, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _ArbitrationService.GetCitizenLitigationRequestForArbitratorsCountReply(requestId, cancellationToken);
            return Ok(returnValue);
        }
        return BadRequest();
    }
    #endregion


}