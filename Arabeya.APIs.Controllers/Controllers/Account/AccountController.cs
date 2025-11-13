using Arabeya.APIs.Controllers.Controllers.Base;
using Arabeya.Core.Application.Abstraction.Models.Auth;
using Arabeya.Core.Application.Abstraction.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arabeya.APIs.Controllers.Controllers.Account
{
	public class AccountController(IServiceManager serviceManager) : BaseController
	{
		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var result = await serviceManager.authService.LoginAsync(model);
			return StatusCode((int)result.StatusCode, result);
		}

		[HttpPost("Register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var result = await serviceManager.authService.RegisterAsync(model);
			return StatusCode((int)result.StatusCode, result);
		}

		[HttpPost("Confirm Email")]
		public async Task<ActionResult<string>> ConfirmEmail(ConfirmEmailDto model)
		{
			var result = await serviceManager.authService.ConfirmUserEmail(model);
			return StatusCode((int)result.StatusCode,result);
		}

		[HttpPost("Forget Password")]
		public async Task<ActionResult> ForgetPassword(string email)
		{
			var result = await serviceManager.authService.ForgetPasswordAsync(email);
			return StatusCode((int)result.StatusCode, result);
		}



		[HttpPost("Reset Password")]
		public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
		{
			var result = await serviceManager.authService.ResetPasswordAsync(model);
			return StatusCode((int)result.StatusCode, result);
		}
	}
}
