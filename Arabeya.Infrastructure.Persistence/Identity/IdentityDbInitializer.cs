using Arabeya.Core.Domain.Contracts.Persistence.DbInitializer;
using Arabeya.Core.Domain.Entities.Identity;
using Arabeya.Infrastructure.Persistence.Common;
using Microsoft.AspNetCore.Identity;

namespace Arabeya.Infrastructure.Persistence.Identity
{
    internal class IdentityDbInitializer(IdentityContext dbContext,UserManager<ApplicationUser>userManager
        ,RoleManager<IdentityRole>roleManager
        ) :DbInitializer(dbContext), IDbIdenitityInitializer
    {
      
        public override async  Task SeedAsync()
        {
            if(!dbContext.Roles.Any())
            {
                var roles = new List<string>()
                {
                    "User","Admin","Owner"
                };

                foreach (var role in roles)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

        }
        
    }
}
