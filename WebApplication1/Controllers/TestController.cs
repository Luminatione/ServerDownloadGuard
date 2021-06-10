using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
	[Route("api/test")]
	public class TestController : DefaultController
	{
		public IActionResult Index()
		{
			logger.LogInformation("Test visited");
			return Json(new { state = "success" });
		}

		public TestController(ApplicationDbContext dbContext, ILogger<DefaultController> logger = null) : base(dbContext, logger: logger)
		{
		}
	}
}
