#nullable enable
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Utility;
using System.Data.Entity;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WebApplication1.Controllers
{
	[Route("api/sql")]
	[ApiController]
	public class SqlController : DefaultController
	{
		public SqlController(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
		{
			commandName = "Sql";
		}
		
		public IActionResult Index(string? authKey, string? query)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state = string.Empty;
			string? value = null;
			if (authKey == null || query == null)
			{
				state = ErrorResultsDescriptions.InvalidCall;
			}
			else if (HavePermission(authKey))
			{

				//we have to connect to db using SqlConnection in order to execute raw sql
				using NpgsqlConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
				using NpgsqlCommand command = new NpgsqlCommand(query, dbConnection);
				try
				{
					dbConnection.Open();
					using NpgsqlDataReader dataReader = command.ExecuteReader();
					value = string.Empty;
					while (dataReader.Read())
					{
						value += ReadRowAsCSV(dataReader);
					}
					state = ErrorResultsDescriptions.Success;

				}
				catch (Exception e)
				{
					value = e.Message;
					state = ErrorResultsDescriptions.ExceptionThrown;
				}

			}
			else
			{
				state = ErrorResultsDescriptions.InsufficientPermissions;
			}
			return Json(new { state, value });
		}

		private static string ReadRowAsCSV(NpgsqlDataReader dataReader)
		{
			string value = string.Empty;
			for (int i = 0; i < dataReader.FieldCount; i++)
			{
				value += dataReader.GetFieldValue<dynamic>(i).ToString() + ';';
			}

			value = value.Remove(value.Length - 1);
			value += '\n';
			return value;
		}
	}
}
