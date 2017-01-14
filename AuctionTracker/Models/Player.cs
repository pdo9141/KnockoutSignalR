using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AuctionTracker.Models
{
	public class Player
	{
		[JsonProperty("id")]
		public int ID { get; set; }

		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("nflTeam")]
		public string NflTeam { get; set; }

		[JsonProperty("teamId")]
		public int TeamID { get; set; }

		[JsonIgnore()]
		public Team Team { get; set;  }

		[JsonProperty("position")]
		public string Position { get; set; }

		[JsonProperty("auctionAmount")]
		public int AuctionAmount { get; set; }

		[NotMapped]
		[JsonProperty("team")]
		public string TeamName { get; set; }
	}
}