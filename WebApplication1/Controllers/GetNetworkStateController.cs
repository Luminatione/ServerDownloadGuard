#nullable enable
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Model;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
	[Route("/api/[controller]")]
	[ApiController]
	public class GetNetworkStateController : DefaultController
	{
		public GetNetworkStateController(ApplicationDbContext dbContext) : base(dbContext)
		{
			commandName = "GetNetworkState";
		}
		public IActionResult Index(string? authKey)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state;
			string? value;
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
					NetworkState networkState = dbContext.NetworkState.First();
					state = ErrorResultsDescriptions.Success;
					networkState.Id = 0;
					User user = dbContext.Users.Find(networkState.UserId);
					return Json(new {state, value = new{networkState.Type, networkState.Description, user?.Login}});
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
