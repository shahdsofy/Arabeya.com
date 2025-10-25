using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Arabeya.Shared.Responses
{
	public class Response<T>
	{
		public bool Succeeded { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; } = string.Empty;
		public string ErrorType { get; set; } = "None";
		public List<string> Errors { get; set; } = new();
		public T? Data { get; set; }
		public int? Count { get; set; }
		public object? Meta { get; set; }

		public static Response<T> Success(T data, int? count = null, string message = "Success", object meta = null!)
		{
			return new Response<T>
			{
				Succeeded = true,
				StatusCode = HttpStatusCode.OK,
				Data = data,
				Count = count,
				Message = message,
				ErrorType = "None",
				Errors = new List<string>(),
				Meta = meta
			};
		}

		public static Response<T> Fail(HttpStatusCode statusCode, string errorType, string message, List<string> errors = null!, object meta = null!)
		{
			return new Response<T>
			{
				Succeeded = false,
				StatusCode = statusCode,
				Data = default!,
				Count = null,
				Message = message,
				ErrorType = errorType,
				Errors = errors ?? new List<string> { message },
				Meta = meta
			};
		}
	}
}
