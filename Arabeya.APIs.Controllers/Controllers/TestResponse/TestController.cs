using Arabeya.APIs.Controllers.Controllers.Base;
using Arabeya.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Arabeya.APIs.Controllers.Controllers.TestResponse
{
	public class TestController : BaseController
	{
		[HttpGet("{statusCode}")]
		public ActionResult<Response<object>> GetReponse([FromRoute] int statusCode)
		{
			return statusCode switch
			{
				200 => StatusCode(200, Response<string>.Success("This is a successful response.")),
				201 => StatusCode(201, Response<string>.Success("This is a created response.")),
				400 => StatusCode(400, Response<string>.Fail(HttpStatusCode.BadRequest, "BadRequest", "This is a bad request response.")),
				401 => StatusCode(401, Response<string>.Fail(HttpStatusCode.Unauthorized, "Unauthorized", "This is an unauthorized response.")),
				403 => StatusCode(403, Response<string>.Fail(HttpStatusCode.Forbidden, "Forbidden", "This is a forbidden response.")),
				404 => StatusCode(404, Response<string>.Fail(HttpStatusCode.NotFound, "NotFound", "This is a not found response.")),
				500 => StatusCode(500, Response<string>.Fail(HttpStatusCode.InternalServerError, "InternalServerError", "This is an internal server error response.")),
				_ => StatusCode(400, Response<string>.Fail(HttpStatusCode.BadRequest, "BadRequest", "Invalid status code provided.")),
			};
		}
	}
}
