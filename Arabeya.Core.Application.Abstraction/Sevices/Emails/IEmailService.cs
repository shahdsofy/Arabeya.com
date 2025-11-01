using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arabeya.Core.Application.Abstraction.Models.Emails;

namespace Arabeya.Core.Application.Abstraction.Sevices.Emails
{
    public interface IEmailService
    {
        public Task SendEmail(Email email);
    }
}
