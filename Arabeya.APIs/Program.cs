
using Arabeya.APIs.Extensions;
using Arabeya.APIs.Middlewares;
using Arabeya.Core.Application;
using Arabeya.Core.Application.Abstraction;
using Arabeya.Infrastructure;
using Arabeya.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace Arabeya.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

			builder.Services.RegesteredPresestantLayer();
			builder.Services.AddInfrastructureService();
			builder.Services.AddPersistenceServices(builder.Configuration);
			builder.Services.AddApplicationServices(builder.Configuration);
			builder.Services.AddIdentityService(builder.Configuration);
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped(typeof(ILoggedInUserService),typeof(LoggedInUserService));



            var app = builder.Build();
            await app.DatabaseAsync();

			app.UseErrorHandlerMiddleware();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

            app.UseHttpsRedirection();

			app.UseAuthorization();
			app.UseAuthentication();
			app.UseStaticFiles();

			app.MapControllers();

			app.Run();
		}
	}
}
