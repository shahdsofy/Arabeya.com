using Arabeya.Core.Application.Abstraction.Sevices;
using Arabeya.Core.Application.Abstraction.Sevices.Auth;

namespace Arabeya.Core.Application.Services
{
    internal class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _AuthService;

        public ServiceManager(Func<IAuthService>AuthserviceFactory)
        {
            _AuthService =new Lazy<IAuthService>(AuthserviceFactory,LazyThreadSafetyMode.ExecutionAndPublication);
        }


        public IAuthService authService => _AuthService.Value;
    }
}
