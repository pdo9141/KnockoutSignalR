using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AuctionTracker.Models
{
	public class Team
	{
		[JsonProperty("id")]
		public int ID { get; set; }
		public string Owner { get; set; }
		public string Name { get; set; }

		public List<Player> Players { get; set; }

		public DateTime LastBid { get; set; }
	}
}