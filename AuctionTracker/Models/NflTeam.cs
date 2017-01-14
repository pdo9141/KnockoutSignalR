using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionTracker.Models
{
	public class NflTeam
	{
		[JsonProperty("id")]
		public int ID { get; set; }
		public string Name { get; set; }
	}
}