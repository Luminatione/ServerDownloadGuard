using System.Linq;

namespace WebApplication1.Utility.Release
{
	public class LoginValidator : IValidator
	{
		private int minLength = 6;
		private int maxLength = 20;
		private string charSet =
			"AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789-_";
		public bool IsValid(string toValidate)
		{
			return toValidate.Length >= minLength && toValidate.Length <= maxLength && toValidate.All(e => charSet.Contains(e));
		}
	}
}