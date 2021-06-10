using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Model;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
	[Route("/api/[controller]")]
	[ApiController]
	public class ListCommandsController : DefaultController
	{
		public ListCommandsController(ApplicationDbContext dbContext) : base(dbContext)
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
			}
			else if (!HavePermission(authKey))
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.InsufficientPermissions;
			}
			else
			{
				try
				{

					var result = dbContext.Permissions.ToList().Select(e => new { e.Command, e.MinimalPermissionsLevel });
					state = ErrorResultsDescriptions.Success;
					return Json(new { state, result });
				}
				catch (Exception e)
				{
					state = ErrorResultsDescriptions.Failure;
					value = $"{ErrorResultsDescriptions.ExceptionThrown}: {e.Message}";
				}
			}
			return Json(new { state, value });
		}
	}
}
