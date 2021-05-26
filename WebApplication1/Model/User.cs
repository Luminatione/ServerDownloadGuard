using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Model
{
	public class User
	{
		[Key] 
		public int Id { get; set; }
		[Required] public string AuthKey { get; set; }
		[Required] public string Login { get; set; }
		[Required] public string Password { get; set; }
		[Required, ForeignKey("Role")] public int Role { get; set; }
	}
}
