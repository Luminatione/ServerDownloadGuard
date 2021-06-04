#nullable enable
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Model;
using WebApplication1.Utility;
using WebApplication1.Utility.Development;
using WebApplication1.Utility.PasswordHashers;
using WebApplication1.Utility.Release;

namespace WebApplication1.Controllers
{
	[Route("api/register")]
	[ApiController]
	public class RegisterController : DefaultController
	{
		private readonly IValidator _loginValidator = new LoginValidator();
		private readonly IValidator _passwordValidator = new PasswordValidator();
		private readonly IPasswordHasher _passwordHasher = new DefaultPasswordHasher();
		private int _defaultRole = 2;
		public RegisterController(ApplicationDbContext dbContext) : base(dbContext)
		{
			commandName = "Register";
		}

		public IActionResult Index(string? login, string? password)
		{
			
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state;
			string? value = null;
			if (login == null || password == null)
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.InvalidCall;
			}
			else if (_loginValidator.IsValid(login) && _passwordValidator.IsValid(password))
			{
				try
				{
					if (!dbContext.Users.Any(e => e.Login == login))
					{
						string authKey = Guid.NewGuid().ToString();
						string hashedPassword = _passwordHasher.Hash(password);
						dbContext.Users.Add(new User { AuthKey = authKey, Role = _defaultRole, Login = login, Password = hashedPassword});
						dbContext.SaveChanges();
						state = ErrorResultsDescriptions.Success;
					}
					else
					{
						state = ErrorResultsDescriptions.Failure;
						value = ErrorResultsDescriptions.LoginAlreadyExist;
					}
				}
				catch (Exception e)
				{
					state = ErrorResultsDescriptions.Failure;
					value = $"{ErrorResultsDescriptions.ExceptionThrown}: {e.Message}";
				}
			}
			else
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.ValidationError;
			}
			return Json(new { state, value });
		}
	}
}
