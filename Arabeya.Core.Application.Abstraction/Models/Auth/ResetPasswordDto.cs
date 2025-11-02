using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arabeya.Core.Application.Abstraction.Models.Auth
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }=null!;

        [EmailAddress]
        public string Email { get; set; }= null!;

        public string Token { get; set; } = null!;
    }
}
