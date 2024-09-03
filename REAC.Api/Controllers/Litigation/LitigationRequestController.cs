using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Get;
using REAC.Application.Contracts.Dtos.Litigation.List;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Application.Contracts.IAppServices.Litigation;
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
    public LitigationRequestController(ILitigationRequestService litigationRequestService, ILitigationRequestService litigationService) :
        base(litigationRequestService)
    {
        _litigationService = litigationService;
    }
    [HttpPost("CreateLitigation")]
    public override async Task<IActionResult> Create([FromBody] LitigationCreateUpdateDto model,
          CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.Add(model);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpPost(nameof(CreateLitigationRequestManual))]
    public async Task<IActionResult> CreateLitigationRequestManual([FromBody] CreateLitigationRequestNoIntegrationDto model,
          CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.CreateLitigationRequestManual(model);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(List))]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LitigationListDto>>> List([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? searchKey, [FromQuery] LitigationListDto model)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.List(pageIndex, pageSize, searchKey, model);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(ReformerList))]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LitigationListDto>>> ReformerList([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? searchKey, [FromQuery] LitigationListDto model)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.ReformerList(pageIndex, pageSize, searchKey, model);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(AuditngList))]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AuditingListDto>>> AuditngList([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? searchKey, [FromQuery] AuditingListDto model)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.AuditngList(pageIndex, pageSize, searchKey, model);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(AuditngListReconciliation))]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AuditingListDto>>> AuditngListReconciliation([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? searchKey, [FromQuery] AuditingListDto model)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.AuditngListReconciliation(pageIndex, pageSize, searchKey, model);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetLitigationArbitrators))]
    [AllowAnonymous]
    public async Task<IActionResult> GetLitigationArbitrators(Guid requestId)

    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.GetLitigationArbitrators(requestId);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(AuditngListArbitration))]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AuditingListDto>>> AuditngListArbitration([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? searchKey, [FromQuery] AuditingListDto model)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.AuditngListArbitration(pageIndex, pageSize, searchKey, model);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetLitigationById))]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LitigationRequest>>> GetLitigationById(Guid litigationId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.Get(litigationId);

            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetLitigationAcceptanceOrRejectDocumentData))]
    [AllowAnonymous]
    public async Task<ActionResult<LitigationAcceptOrRejectDocumentDto>> GetLitigationAcceptanceOrRejectDocumentData(Guid litigationId, Guid requestId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.GetLitigationAcceptanceOrRejectDocumentData(litigationId, requestId);

            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetLitigationPartiesDetailsById))]
    [AllowAnonymous]
    public async Task<ActionResult<LitigationPartiesDetailsDto>> GetLitigationPartiesDetailsById(Guid litigationId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.GetLitigationPartiesDetails(litigationId);

            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetLitigationDetailsById))]
    [AllowAnonymous]
    public async Task<ActionResult<LitigationDetailsDto>> GetLitigationDetailsById(Guid litigationId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.GetLitigationDetails(litigationId);

            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpPost(nameof(CreateLitigationRequest))]

    public async Task<ActionResult<LitigationDetailsDto>> CreateLitigationRequest(LitigationRequestDto litigationRequestDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.CreateLitigationRequest(litigationRequestDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    #region AgreementRequest
    [HttpPost(nameof(AddCitizenLitigationRequestForAgreementRequestReply))]
    public async Task<IActionResult> AddCitizenLitigationRequestForAgreementRequestReply([FromBody] AgreementRequestPayload agreementRequestPayload, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.AddCitizenLitigationRequestForAgreementRequestReply(agreementRequestPayload.RequestId, agreementRequestPayload.agreementRequestStatus);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetCitizenLitigationRequestForAgreementRequestReplyByRequestId))]
    public async Task<IActionResult> GetCitizenLitigationRequestForAgreementRequestReplyByRequestId(Guid requestId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.GetCitizenLitigationRequestForAgreementRequestReply(requestId);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpGet(nameof(GetCitizenLitigationRequestForAgreementRequestReply))]
    public async Task<IActionResult> GetCitizenLitigationRequestForAgreementRequestReply()
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _litigationService.GetCitizenLitigationRequestForAgreementRequestReply();
            return Ok(returnValue);
        }
        return BadRequest();
    }
    #endregion

}