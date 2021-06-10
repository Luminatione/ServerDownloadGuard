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
using Microsoft.Extensions.Logging;
using Npgsql;

namespace WebApplication1.Controllers
{
	[Route("api/sql")]
	[ApiController]
	public class SqlController : DefaultController
	{
		public SqlController(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<DefaultController> logger) : base(dbContext, configuration, logger)
		{
			commandName = "Sql";
		}

		public IActionResult Index(string? authKey, string? query)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state;
			string? value = null;
			if (authKey == null || query == null)
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.InvalidCall;
				logger.LogError($"{commandName} - {state}: {value}");
			}
			else
			{
				//we have to connect to db using SqlConnection in order to execute raw sql
				try
				{
					if (HavePermission(authKey))
					{
						using NpgsqlConnection dbConnection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
						using NpgsqlCommand command = new NpgsqlCommand(query, dbConnection);
						dbConnection.Open();
						using NpgsqlDataReader dataReader = command.ExecuteReader();
						value = string.Empty;
						while (dataReader.Read())
						{
							value += ReadRowAsCSV(dataReader);
						}
						state = ErrorResultsDescriptions.Success;
						logger.LogInformation($"{commandName} - {state}: User: {dbContext.Users.First(e => e.AuthKey == authKey).Id}, Query: {query}");
					}
					else
					{
						state = ErrorResultsDescriptions.Failure;
						value = ErrorResultsDescriptions.InsufficientPermissions;
						logger.LogError($"{commandName} - {state}: {value}");
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
