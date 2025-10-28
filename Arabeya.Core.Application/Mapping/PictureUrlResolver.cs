using Arabeya.Core.Application.Abstraction.Models.Auth;
using Arabeya.Core.Domain.Entities.Identity;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Arabeya.Core.Application.Mapping
{
    public class PictureUrlResolver(IConfiguration configuration) : IValueResolver<ApplicationUser, RegisterDto, string?>
    {
        public string? Resolve(ApplicationUser source, RegisterDto destination, string? destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.DrivingLicence))
            {
                return $"{configuration["URLs:BaseURL"]}/{source.DrivingLicence}";
            }
            return string.Empty;
        }
    }
}
