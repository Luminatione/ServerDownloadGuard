using System.Linq;

namespace WebApplication1.Utility.Release
{
	public class PasswordValidator : IValidator
	{
		private int minLength = 8;
		private int maxLength = 32;
		private string charSet =
			"AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789~!@#$%^&*()-_=+";
		public bool IsValid(string toValidate)
		{
			return toValidate.Length >= minLength && toValidate.Length <= maxLength && toValidate.All(e => charSet.Contains(e));
		}
	}
}