using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Application.ModuleServices;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.Api.Controllers.Reconciliation.Process;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Reconciliation)]
[PermissionRequired(Modules.Reconciliation)]
[AllowAnonymous]

public class ReconciliationController : ControllerBase
{
    private readonly IReconciliationService _reconciliationService;
    public ReconciliationController(IReconciliationService reconciliationService)
    {
        _reconciliationService = reconciliationService;
    }


    #region Session
    [HttpGet(nameof(GetLitigationRequestForSession))]
    public async Task<IActionResult> GetLitigationRequestForSession(Guid litigationRequestId, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.GetLitigationRequestForSession(litigationRequestId, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetReconciliationRequestForSession))]
    public async Task<IActionResult> GetReconciliationRequestForSession(CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.GetLitigationRequestForSession(cancellationToken);
            return Ok(returnValue);
        }
        return BadRequest();
    }
    [HttpPost(nameof(AddReconciliationSession))]
    public async Task<IActionResult> AddReconciliationSession([FromBody] ScheduleSessionProcessRequestPayload scheduleSessionProcessRequestPayload, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.AddReconciliationSession(scheduleSessionProcessRequestPayload, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    #endregion

    #region Report
    [HttpGet(nameof(GetLitigationRequestForReport))]
    public async Task<IActionResult> GetLitigationRequestForReport(Guid litigationRequestId, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.GetLitigationRequestForReport(litigationRequestId, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetReconciliationRequestForReport))]
    public async Task<IActionResult> GetReconciliationRequestForReport(CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.GetLitigationRequestForReport(cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpPost(nameof(AddReconciliationReport))]
    public async Task<IActionResult> AddReconciliationReport([FromBody] WritingMinutesOfTheMeetingRequestPayload scheduleSessionProcessRequestPayload, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.AddReconciliationReport(scheduleSessionProcessRequestPayload, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    #endregion

    #region Citizen Replay
    [HttpGet(nameof(AddCitizenLitigationRequestForReportReply))]
    public async Task<IActionResult> AddCitizenLitigationRequestForReportReply(Guid requestId, MinutesOfMeetingApprovalStatus minutesOfMeetingApprovalStatus, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.AddCitizenLitigationRequestForReportReply(requestId, minutesOfMeetingApprovalStatus, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetCitizenLitigationRequestForReportReply))]
    public async Task<IActionResult> GetCitizenLitigationRequestForReportReply(CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.GetCitizenLitigationRequestForReportReply(cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetCitizenLitigationRequestForReportReplyByRequestId))]
    public async Task<IActionResult> GetCitizenLitigationRequestForReportReplyByRequestId(Guid requestId, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.GetCitizenLitigationRequestForReportReply(requestId, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    #endregion

    #region Extend Period
    [HttpGet(nameof(AddReconciliationExtendPeriod))]
    public async Task<IActionResult> AddReconciliationExtendPeriod([FromBody] LitigationRequestExtendPeriodRequestCreateUpdateDto litigationRequestExtendPeriodRequestCreateUpdateDto, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.AddReconciliationExtendPeriod(litigationRequestExtendPeriodRequestCreateUpdateDto, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetLitigationRequestExtendPeriod))]
    public async Task<IActionResult> GetLitigationRequestExtendPeriod(CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.GetLitigationRequestExtendPeriod(cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetLitigationRequestExtendPeriodByRequestId))]
    public async Task<IActionResult> GetLitigationRequestExtendPeriodByRequestId(Guid requestId, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _reconciliationService.GetLitigationRequestExtendPeriod(requestId, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    #endregion
}