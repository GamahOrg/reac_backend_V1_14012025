using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Constants;

namespace REAC.Api.Controllers.Litigation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Public)]
[PermissionRequired(Modules.Citizen)]

public class CitizenController : ControllerBase
{
    private readonly ICitizenBankAccountService _citizenBankAccountService;

    private readonly ICitizenService _citizenService;
    public CitizenController(ICitizenService citizenService, ICitizenBankAccountService citizenBankAccountService)
    {
        _citizenService = citizenService;
        _citizenBankAccountService = citizenBankAccountService;
    }

    [HttpGet(nameof(GetCitizenBasicData))]
    public async Task<ActionResult> GetCitizenBasicData()
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetCitizenBasicData();
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpPost(nameof(UpdateCitizenBasicData))]
    public async Task<ActionResult> UpdateCitizenBasicData(SaveCitizenBasicDataProfileDto citizenBasicDataProfileDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.UpdateCitizenBasicData(citizenBasicDataProfileDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpPost(nameof(UpdateCitizenBankAccount))]
    public async Task<ActionResult> UpdateCitizenBankAccount(CitizenBankAccountCreateUpdateDto citizenBankAccountCreateUpdateDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenBankAccountService.UpdateCitizenBankAccount(citizenBankAccountCreateUpdateDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpPost(nameof(AddArbitratorFreeUpDecision))]
    public async Task<ActionResult> AddArbitratorFreeUpDecision(ArbitratorFreeUpDecisionCreateDto arbitratorFreeUpDecisionCreateDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.AddArbitratorFreeUpDecision(arbitratorFreeUpDecisionCreateDto);
            return Ok(returnValue);
        }
        return BadRequest();
    }
    [HttpPost(nameof(AddArbitratorStopDecision))]
    public async Task<ActionResult> AddArbitratorStopDecision(ArbitratorStopDecisionCreateDto arbitratorStopDecisionCreateDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.AddArbitratorStopDecision(arbitratorStopDecisionCreateDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpPost(nameof(AddReformerFreeUpDecision))]
    public async Task<ActionResult> AddReformerFreeUpDecision(ReformerFreeUpDecisionCreateDto reformerFreeUpDecisionCreateDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.AddReformerFreeUpDecision(reformerFreeUpDecisionCreateDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpPost(nameof(AddReformerStopDecision))]
    public async Task<ActionResult> AddReformerStopDecision(ReformerStopDecisionCreateDto reformerStopDecisionCreateDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.AddReformerStopDecision(reformerStopDecisionCreateDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }


    [HttpGet(nameof(GetCitizenBankAccount))]
    public async Task<ActionResult> GetCitizenBankAccount()
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenBankAccountService.GetCitizenBankAccount();
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetDelegatedPersons))]
    public async Task<ActionResult> GetDelegatedPersons()
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetDelegatedPersons();
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetReformersLookup))]
    public async Task<ActionResult> GetReformersLookup(Guid? citizenId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetReformersLookup(citizenId);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetArbitratorsLookup))]
    public async Task<ActionResult> GetArbitratorsLookup(Guid? citizenId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetArbitratorsLookup(citizenId);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetReformersDecisions))]
    public async Task<ActionResult> GetReformersDecisions(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetReformersDecisions(pageIndex, pageSize, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetReformersDecisionsByCitizenId))]
    public async Task<ActionResult> GetReformersDecisionsByCitizenId(Guid citizenId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetReformersDecisions(citizenId);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetArbitratorsDecisions))]
    public async Task<ActionResult> GetArbitratorsDecisions(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetArbitratorsDecisions(pageIndex, pageSize, cancellationToken);
            return Ok(returnValue);
        }

        return BadRequest();
    }
    [HttpGet(nameof(GetArbitratorsDecisionsByCitizenId))]
    public async Task<ActionResult> GetArbitratorsDecisionsByCitizenId(Guid citizenId)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetArbitratorsDecisions(citizenId);
            return Ok(returnValue);
        }

        return BadRequest();
    }

    [HttpGet(nameof(GetDelegatedFacilities))]
    public async Task<ActionResult> GetDelegatedFacilities()
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.GetDelegatedFacilities();
            return Ok(returnValue);
        }

        return BadRequest();
    }
}