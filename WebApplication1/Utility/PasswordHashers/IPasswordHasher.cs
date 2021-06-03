#nullable enable

namespace WebApplication1.Utility.PasswordHashers
{
	public interface IPasswordHasher
	{
		public string Hash(string password, string? salt = null, int? iterations = null);
	}
}