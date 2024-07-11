using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Domain.Shared.Constants;

namespace REAC.Api.Controllers.Integration
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = APISections.Integration)]
    [AllowAnonymous]

    public class GEELoginController : ControllerBase
    {
        private readonly IGEELoginService _gEELoginService;

        public GEELoginController(IGEELoginService gEELoginService)
        {
            _gEELoginService = gEELoginService;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login()
        => Ok(await _gEELoginService.Login());

    }
}
