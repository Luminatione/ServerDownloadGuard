namespace WebApplication1.Utility.PasswordHashers
{
	public interface IPasswordComparer
	{
		public bool Compare(string hashedPassword, string password, IPasswordHasher hasher);
	}
}