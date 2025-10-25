using Arabeya.Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Arabeya.Shared.Responses
{
	public class ResponseHandler
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ResponseHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public Response<T> Success<T>(T entity, string message = "Success", object meta = null!)
		{
			var context = _httpContextAccessor.HttpContext;
			context.Response.StatusCode = (int)HttpStatusCode.OK;

			return Response<T>.Success(entity, null, message, meta);
		}

		public Response<T> Fail<T>(string message, ErrorType errorType = ErrorType.Unexpected, object meta = null!)
		{
			HttpStatusCode statusCode = errorType switch
			{
				ErrorType.NotFound => HttpStatusCode.NotFound,
				ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
				ErrorType.Validation => HttpStatusCode.BadRequest,
				ErrorType.Forbidden => HttpStatusCode.Forbidden,
				ErrorType.Conflict => HttpStatusCode.Conflict,
				_ => HttpStatusCode.BadRequest
			};

			var context = _httpContextAccessor.HttpContext;
			context.Response.StatusCode = (int)statusCode;

			return Response<T>.Fail(statusCode, errorType.ToString(), message, new List<string> { message }, meta);
		}
	}
}
