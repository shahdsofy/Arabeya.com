using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arabeya.Shared.Errors
{
	public enum ErrorType
	{
		None,
		Validation,
		NotFound,
		BadRequest,
		Conflict,
		Unauthorized,
		Forbidden,
		Unexpected
	}
}
