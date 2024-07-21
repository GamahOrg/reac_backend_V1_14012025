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
    private readonly ICitizenService _citizenService;
    public CitizenController(ICitizenService citizenService)
    {
        _citizenService = citizenService;
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
    public async Task<ActionResult> UpdateCitizenBasicData(CitizenBasicDataProfileDto citizenBasicDataProfileDto)
    {
        if (ModelState.IsValid)
        {
            var returnValue = await _citizenService.UpdateCitizenBasicData(citizenBasicDataProfileDto);
            return Ok(returnValue);
        }

        return BadRequest();
    }
}