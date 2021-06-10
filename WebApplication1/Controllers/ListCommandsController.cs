#nullable enable

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApplication1.Model;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
	[Route("/api/[controller]")]
	[ApiController]
	public class ListCommandsController : DefaultController
	{
		public ListCommandsController(ApplicationDbContext dbContext, ILogger<DefaultController> logger) : base(dbContext, logger: logger)
		{
			commandName = "ListCommands";
		}
		public IActionResult Index(string? authKey)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state;
			string value;
			if (authKey == null)
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.InvalidCall;
				logger.LogError($"{commandName} - {state}: {value}");
			}
			else if (!HavePermission(authKey))
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.InsufficientPermissions;
				logger.LogError($"{commandName} - {state}: {value}");
			}
			else
			{
				try
				{

					var result = dbContext.Permissions.ToList().Select(e => new { e.Command, e.MinimalPermissionsLevel });
					state = ErrorResultsDescriptions.Success;
					logger.LogInformation($"{commandName} - {state}: User: {dbContext.Users.First(e=>e.AuthKey==authKey).Id}");
					return Json(new { state, result });
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
