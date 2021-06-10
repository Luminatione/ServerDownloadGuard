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
			}
			else if (!HavePermission(authKey))
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.InsufficientPermissions;
			}
			else
			{
				User user = dbContext.Users.First(e => e.AuthKey == authKey);

				state = ErrorResultsDescriptions.Success;
				value = dbContext.Roles.Find(user.Role).PermissionLevel.ToString();
			}
			return Json(new { state, value });
		}
	}
}
