using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Integration.GEE;
using REAC.Application.Contracts.Dtos.UserManage;
using REAC.Application.Contracts.IAppServices.UserManage;
using REAC.Domain.Shared.Constants;

namespace REAC.Api.Controllers.UserManage
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = APISections.Litigation)]
    [AllowAnonymous]

    public class UserRegisterController : ControllerBase
    {
        private readonly IUserRegisterService _userRegister;

        public UserRegisterController(IUserRegisterService userRegister)
        {
            _userRegister = userRegister;
        }

        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
         => Ok(await _userRegister.Register(userRegisterDto));
    }
}
