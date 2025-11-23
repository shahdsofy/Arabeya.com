using Arabeya.Core.Application.Abstraction;
using System.Security.Claims;

namespace Arabeya.APIs
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor _context;

        public LoggedInUserService(IHttpContextAccessor context)
        {
            _context = context;
            UserId = _context.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public string? UserId { get; }

        
    }
}
