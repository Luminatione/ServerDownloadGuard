using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Model
{
	public class Permissions
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Command { get; set; }
		[Required]
		public int MinimalPermissionsLevel { get; set; }
	}
}
