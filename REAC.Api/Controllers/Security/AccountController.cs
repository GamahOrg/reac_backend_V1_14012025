using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Integration.GEE;
using REAC.Application.Contracts.Dtos.UserManage;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Application.Contracts.IAppServices.UserManage;
using REAC.Domain.Shared.Constants;

namespace REAC.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = APISections.Security)]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly INafathService _nafathService;
        private readonly IUserRegisterService _userRegister;
        public AccountController(INafathService nafathService, IUserRegisterService userRegister)
        {
            _nafathService = nafathService;
            _userRegister = userRegister;
        }

        [HttpPost(nameof(VerifyUser))]
        public async Task<IActionResult> VerifyUser(NafathRequestDto nafathRequestDto, CancellationToken cancellationToken)
        {
            return Ok(await _nafathService.Nafath(nafathRequestDto, cancellationToken));
        }

        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            return Ok(await _userRegister.Register(userRegisterDto));
        }
    }
}
