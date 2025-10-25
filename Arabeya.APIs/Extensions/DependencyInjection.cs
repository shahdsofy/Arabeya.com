using Arabeya.APIs.Controllers;
using Arabeya.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Arabeya.APIs.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection RegesteredPresestantLayer(this IServiceCollection services)
		{

			#region Swagger
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			#endregion

			// To Access HttpContext in Services
			services.AddHttpContextAccessor();

			// Customize ModelState Invalid Response
			services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.SuppressModelStateInvalidFilter = false;
					options.InvalidModelStateResponseFactory = (actionContext =>
					{
						var Errors = actionContext.ModelState.Where(e => e.Value!.Errors.Count() > 0)
													.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage);

						return new BadRequestObjectResult(new Response<object>() { Errors = Errors.ToList() });

					});
				}
			)
				// Allow to see Controllers in another Assembly (Arabeya.APIs.Controllers)
				.AddApplicationPart(typeof(AssemblyInformation).Assembly);


			// Allow User Access ==> (Frontend, Flutter, etc..)

			//services.AddCors(o =>
			//{
			//	o.AddPolicy("default", p =>
			//	{
			//		p.AllowAnyOrigin()
			//		 .AllowAnyHeader()
			//		 .AllowAnyMethod();
			//	});
			//});

			return services;
		}
	}
}
