using Arabeya.APIs.Controllers.Controllers.Base;
using Arabeya.Core.Application.Abstraction.Models.Auth;
using Arabeya.Core.Application.Abstraction.Sevices;
using Microsoft.AspNetCore.Mvc;

namespace Arabeya.APIs.Controllers.Controllers.Account
{
    public class AccountController(IServiceManager serviceManager):BaseController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto model)
        {
            var result=await serviceManager.authService.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var result = await serviceManager.authService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("Confirm Email")]
        public async Task<ActionResult<string>> ConfirmEmail(ConfirmEmailDto model)
        {
            var result = await serviceManager.authService.ConfirmUserEmail(model);
            return Ok(result);
        }
    }
}
