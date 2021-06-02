#nullable enable
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Model;
using WebApplication1.Utility;
using WebApplication1.Utility.Development;

namespace WebApplication1.Controllers
{
	[Route("api/register")]
	[ApiController]
	public class RegisterController : DefaultController
	{
		private readonly IValidator _loginValidator = new DefaultValidatorDev();
		private readonly IValidator _passwordValidator = new DefaultValidatorDev();
		private readonly IValidator _reCaptchaValidator = new DefaultValidatorDev();
		private int _defaultRole = 2;
		public RegisterController(ApplicationDbContext dbContext) : base(dbContext)
		{
			commandName = "Register";
		}

		public IActionResult Index(string? login, string? password, string? recaptcha)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state = string.Empty;
			string? value = null;
			if (login == null || password == null || recaptcha == null)
			{
				state = ErrorResultsDescriptions.InvalidCall;
			}
			else if (_loginValidator.IsValid(login) && _passwordValidator.IsValid(password) && _reCaptchaValidator.IsValid(recaptcha))
			{
				try
				{
					string authKey = Guid.NewGuid().ToString();
					dbContext.Users.Add(new User { AuthKey = authKey, Role = _defaultRole, Login = login, Password = password});
					dbContext.SaveChanges();
					state = ErrorResultsDescriptions.Success;
				}
				catch (Exception e)
				{
					state = ErrorResultsDescriptions.ExceptionThrown;
					value = e.Message;
				}
				
			}
			else
			{
				state = ErrorResultsDescriptions.InvalidCall;
			}
			return Json(new { state, value });
		}
	}
}
