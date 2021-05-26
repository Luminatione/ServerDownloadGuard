using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Model
{
	public class NetworkState
	{
		public enum NetworkStateType
		{
			Nothing = 0, Downloading = 1, AskingToNotDownload = 2
		}
		[Key] public int Id { get; set; }
		[Required, ForeignKey("User")] public int UserId { get; set; }
		public NetworkStateType Type { get; set; }
		public string Description { get; set; }
	}
}
