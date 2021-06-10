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
	public class DefaultController : Controller
	{
		protected readonly ApplicationDbContext dbContext;
		protected string commandName;
		protected readonly IConfiguration configuration;
		protected readonly ILogger<DefaultController> logger;
		public DefaultController(ApplicationDbContext dbContext, IConfiguration configuration = null, ILogger<DefaultController> logger = null)
		{
			this.dbContext = dbContext;
			this.configuration = configuration;
			this.logger = logger;
		}
		protected int? GetPermissionsLevelOfUser(string authKey)
		{
			return dbContext.Roles.Find(dbContext.Users.First(e => e.AuthKey == authKey).Role).PermissionLevel;
		}

		protected int? GetMinimalPermissionsLevelOfCommand()
		{
			return dbContext.Permissions.First(e => e.Command == commandName).MinimalPermissionsLevel;
		}
		protected bool HavePermission(string authKey)
		{
			int permissionsLevelOfUser = GetPermissionsLevelOfUser(authKey) ?? throw new Exception(ErrorResultsDescriptions.AuthenticationFailed);
			int minimalPermissionLevelOfCommand = GetMinimalPermissionsLevelOfCommand() ?? throw new Exception(ErrorResultsDescriptions.InvalidCall);
			return permissionsLevelOfUser >= minimalPermissionLevelOfCommand;
		}
	}
}
