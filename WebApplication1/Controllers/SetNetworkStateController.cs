using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SetNetworkStateController : DefaultController
	{
		public SetNetworkStateController(ApplicationDbContext dbContext) : base(dbContext)
		{
		}
		public IActionResult Index(string? authKey)
		{
			return View();
		}
	}
}
