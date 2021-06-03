using System;
using System.Security.Cryptography;

#nullable enable

namespace WebApplication1.Utility.PasswordHashers
{
	public class DefaultPasswordHasher : IPasswordHasher
	{
		private readonly int _defaultSaltSize = 16;
		private readonly int _defaultIterationCount = 1000;
		private readonly int _defaultKeySize = 32;
		public string Hash(string password, string? salt = null, int? iterations = null)
		{
			iterations ??= _defaultIterationCount;
			string hash;
			if (salt == null)
			{
				using Rfc2898DeriveBytes hashingAlgorithm = new Rfc2898DeriveBytes(password, _defaultSaltSize, iterations.Value, HashAlgorithmName.SHA512);
				hash = Convert.ToBase64String(hashingAlgorithm.GetBytes(_defaultKeySize));
				salt = Convert.ToBase64String(hashingAlgorithm.Salt);
			}
			else
			{
				using Rfc2898DeriveBytes hashingAlgorithm = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations.Value, HashAlgorithmName.SHA512);
				hash = Convert.ToBase64String(hashingAlgorithm.GetBytes(_defaultKeySize));
			}
			return $"{hash}.{salt}.{iterations}";
		}

		public (string? hash, string? salt, int? iterations) Split(string hashedPassword)
		{
			int firstSeparator = hashedPassword.IndexOf('.');
			int secondSeparator = hashedPassword.IndexOf('.', firstSeparator + 1);
			string hash = new string(hashedPassword.ToCharArray(), 0, firstSeparator);
			string salt = new string(hashedPassword.ToCharArray(), firstSeparator + 1, secondSeparator - firstSeparator - 1);
			int iterations = int.Parse(new string(hashedPassword.ToCharArray(), secondSeparator + 1, hashedPassword.Length - secondSeparator - 1));

			return (hash, salt, iterations);
		}
	}
}