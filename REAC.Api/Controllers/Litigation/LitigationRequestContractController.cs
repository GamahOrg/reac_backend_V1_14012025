using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Contract;
using REAC.Application.Contracts.Dtos.Litigation.Create.Defendant;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Domain.Shared.Constants;
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
    public  async Task<IActionResult> CreateRent([FromBody] RentCreateUpdateDto model,
       CancellationToken cancellationToken) // عقد ايجار
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.AddRentContract(model);
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

    [HttpPost(nameof(IntegrationVerification))]
    [AllowAnonymous]
    public async Task<IActionResult> IntegrationVerification() // عقد ايجار
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _contractService.IntegrationVerification();
            return Ok(returnValue);
        }
        return BadRequest();
    }


}
