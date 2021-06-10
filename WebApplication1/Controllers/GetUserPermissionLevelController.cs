#nullable enable

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApplication1.Model;
using WebApplication1.Utility;
using WebApplication1.Utility.PasswordHashers;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GetUserPermissionLevelController : DefaultController
	{
		public GetUserPermissionLevelController(ApplicationDbContext dbContext, ILogger<DefaultController> logger) : base(dbContext, logger: logger)
		{
			commandName = "GetUserPermissionLevel";
		}

		public IActionResult Index(string? authKey)
		{
			
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state;
			string? value = null;
			if (authKey == null)
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.InvalidCall;
				logger.LogError($"{commandName} - {state}: {value}");
			}
			else
			{
				try
				{
					if (!HavePermission(authKey))
					{
						state = ErrorResultsDescriptions.Failure;
						value = ErrorResultsDescriptions.InsufficientPermissions;
						logger.LogError($"{commandName} - {state}: {value}");
					}
					else
					{
						User user = dbContext.Users.First(e => e.AuthKey == authKey);
						state = ErrorResultsDescriptions.Success;
						value = dbContext.Roles.Find(user.Role).PermissionLevel.ToString();
						logger.LogInformation($"{commandName} - {state}: User: {user.Id}");

					}
				}
				catch (Exception e)
				{
					state = ErrorResultsDescriptions.Failure;
					value = $"{ErrorResultsDescriptions.ExceptionThrown}: {e.Message}";
					logger.LogError($"{commandName} - {state}: {value}");
				}
			}
			return Json(new { state, value });
		}
	}
}
