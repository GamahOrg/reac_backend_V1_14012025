using Core.Security.Attributes;
using Core.Security.Authorization;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Integration;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Contract;
using REAC.Application.Contracts.Dtos.Litigation.Create.Defendant;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Contract;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.Api.Controllers.Litigation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Litigation)]
[PermissionRequired(Modules.LitigationRequestContract)]
public class LitigationRequestContractController : BaseController<LitigationRequestContract,
       LitigationRequestContractCreateUpdateDto, LitigationRequestContractCreateUpdateDto
       , Guid>
{
    private readonly IUserProviderService _contractService;
    public LitigationRequestContractController(IUserProviderService litigationRequestContractService) :
        base(litigationRequestContractService)
    {
        _contractService = litigationRequestContractService;
    }
    [HttpPost(nameof(CreateRent))]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRent([FromBody] RentCreateUpdateDto model,
       CancellationToken cancellationToken) // عقد ايجار
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.AddRentContract(model);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(CreateProperties))]
    [AllowAnonymous]
    public async Task<IActionResult> CreateProperties([FromBody] PropertiesCreateUpdateDto model,
   CancellationToken cancellationToken) // عقد ملاك
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.AddPropertiesContract(model);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(CreateRealState))]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRealState([FromBody] RealEstateBrokerageCreateUpdateDto model,
       CancellationToken cancellationToken) // عقد ايجار
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.AddRealEstateContract(model);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(CreateWafi))]
    [AllowAnonymous]
    public async Task<IActionResult> CreateWafi([FromBody] WafiCreateUpdateDto model,
      CancellationToken cancellationToken) // عقد وافي
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.AddWafiContract(model);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(CreateOther))]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOther([FromBody] OtherCreateUpdateDto model,
   CancellationToken cancellationToken) // عقد اخرى
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.AddOtherContract(model);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(CreateDefendant))]
    [AllowAnonymous]
    public async Task<IActionResult> CreateDefendant([FromBody] DefendantCreateUpdateDto model,
      CancellationToken cancellationToken)    // مدعي عليه
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.AddDefendant(model);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(RentIntegrate))]
    [AllowAnonymous]
    public async Task<IActionResult> RentIntegrate(LitigationRequestOwnerType FirstPartyCapacity, string contractKey = "", string versionNumber = "") // عقد ايجار
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.RentIntegrate(FirstPartyCapacity, contractKey, versionNumber);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(RealStateIntegrate))]
    [AllowAnonymous]
    public async Task<IActionResult> RealStateIntegrate(LitigationRequestOwnerType FirstPartyCapacity, string contractKey = "") // عقد ايجار
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.RealStateIntegrate(FirstPartyCapacity, contractKey);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(PropertiesIntegrate))]
    [AllowAnonymous]
    public async Task<IActionResult> PropertiesIntegrate(LitigationRequestOwnerType FirstPartyCapacity, string associationCode = "", string associationName = "") // عقد ملاك
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.PropertiesIntegrate(FirstPartyCapacity, associationCode, associationName);
            return Ok(returnValue);
        }
        return BadRequest();
    }

    [HttpPost(nameof(WafiIntegrate))]
    [AllowAnonymous]
    public async Task<IActionResult> WafiIntegrate(LitigationRequestOwnerType FirstPartyCapacity, string contractKey = "") 
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.WafiIntegrate(FirstPartyCapacity, contractKey);
            return Ok(returnValue);
        }
        return BadRequest();
    }


}
