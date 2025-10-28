using Arabeya.Core.Application.Abstraction.Sevices.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arabeya.Core.Application.Abstraction.Sevices
{
    public interface IServiceManager
    {

        public IAuthService authService { get; }
    }
}
