namespace WebApplication1.Utility.PasswordHashers
{
	public class DefaultPasswordComparer : IPasswordComparer
	{
		public bool Compare(string hashedPassword, string password, IPasswordHasher hasher)
		{
			var splitHash = hasher.Split(hashedPassword);
			string newHash = hasher.Hash(password, splitHash.salt, splitHash.iterations);
			return hashedPassword == newHash;
		}
	}
}