using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Utility
{
	public static class ErrorResultsDescriptions
	{
		public static readonly string Success = "Success";
		public static readonly string Failure = "Failure";

		public static readonly string AuthenticationFailed = "Authentication failed";
		public static readonly string InsufficientPermissions = "Insufficient permissions";
		public static readonly string InvalidQuery = "Statement is not valid SQL query";
		public static readonly string InvalidCall = "Invalid API call";
		public static readonly string ExceptionThrown = "Exception thrown";
		public static readonly string LoginAlreadyExist = "Login already exist";
	}
}
