using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AuctionTracker.Models
{
	public class AuctionTrackerContext : DbContext
	{
		public DbSet<Team> Teams { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<NflTeam> NflTeams { get; set; }
	}
}