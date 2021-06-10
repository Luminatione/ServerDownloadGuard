#nullable enable
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
		public RegisterController(ApplicationDbContext dbContext, ILogger<DefaultController> logger) : base(dbContext, logger: logger)
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
				logger.LogError($"{commandName} - {state}: {value}");
			}
			else if (_loginValidator.IsValid(login) && _passwordValidator.IsValid(password))
			{
				try
				{
					if (!dbContext.Users.Any(e => e.Login == login))
					{
						string authKey = Guid.NewGuid().ToString();
						string hashedPassword = _passwordHasher.Hash(password);
						User user = new User {AuthKey = authKey, Role = _defaultRole, Login = login, Password = hashedPassword};
						dbContext.Users.Add(user);
						dbContext.SaveChanges();
						state = ErrorResultsDescriptions.Success;
						logger.LogInformation($"{commandName} - {state}: User: {user.Id}");
					}
					else
					{
						state = ErrorResultsDescriptions.Failure;
						value = ErrorResultsDescriptions.LoginAlreadyExist;
						logger.LogError($"{commandName} - {state}: {value}");
					}
				}
				catch (Exception e)
				{
					state = ErrorResultsDescriptions.Failure;
					value = $"{ErrorResultsDescriptions.ExceptionThrown}: {e.Message}";
					logger.LogError($"{commandName} - {state}: {value}");
				}
			}
			else
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.ValidationError;
				logger.LogError($"{commandName} - {state}: {value}");
			}
			return Json(new { state, value });
		}
	}
}
