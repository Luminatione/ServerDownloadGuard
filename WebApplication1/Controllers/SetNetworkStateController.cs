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

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SetNetworkStateController : DefaultController
	{
		public SetNetworkStateController(ApplicationDbContext dbContext, ILogger<DefaultController> logger) : base(dbContext, logger: logger)
		{
			commandName = "GetNetworkState";
		}
		public IActionResult Index(string? authKey, int? type, string? description)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state;
			string? value = null;
			if (authKey == null || type == null)
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
					NetworkState networkState = dbContext.NetworkState.First();
					User user = dbContext.Users.First(e => e.AuthKey == authKey);
					networkState.UserId = user.Id;
					networkState.Description = description ?? string.Empty;
					networkState.Type = (NetworkState.NetworkStateType) type;
					dbContext.SaveChanges();
					state = ErrorResultsDescriptions.Success;
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
