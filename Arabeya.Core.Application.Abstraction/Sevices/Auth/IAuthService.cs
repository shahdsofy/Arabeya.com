using Arabeya.Core.Application.Abstraction.Models.Auth;
using Arabeya.Shared.Responses;

namespace Arabeya.Core.Application.Abstraction.Sevices.Auth
{
    public interface IAuthService
    {
        Task<Response<UserDto>> LoginAsync(LoginDto model);
        Task<Response<UserDto>> RegisterAsync (RegisterDto model);
    }
}
