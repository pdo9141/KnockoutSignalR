namespace AuctionTracker.Migrations
{
	using Models;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<AuctionTracker.Models.AuctionTrackerContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(AuctionTracker.Models.AuctionTrackerContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//

			context.Teams.AddOrUpdate(
				t => t.Name,
				new Team() { Name = "The Believers", Owner = "April", LastBid = DateTime.Now.AddSeconds(1) },
				new Team() { Name = "Wisconsin Wrath", Owner = "Skye", LastBid = DateTime.Now.AddSeconds(2) },
				new Team() { Name = "Waldo's Wearsome...", Owner = "Paul", LastBid = DateTime.Now.AddSeconds(3) },
				new Team() { Name = "*Backfield Not Included", Owner = "Jersey", LastBid = DateTime.Now.AddSeconds(4) },
				new Team() { Name = "Papa Fresh", Owner = "Craig", LastBid = DateTime.Now.AddSeconds(5) },
				new Team() { Name = "Minnesota Diehard", Owner = "Brian", LastBid = DateTime.Now.AddSeconds(6) },
				new Team() { Name = "Mike Likes Vikes", Owner = "Mike", LastBid = DateTime.Now.AddSeconds(7) },
				new Team() { Name = "Shenzy's Revenge", Owner = "Steve", LastBid = DateTime.Now.AddSeconds(8) },
				new Team() { Name = "Pistol Shrimp All Stars", Owner = "Brennan", LastBid = DateTime.Now.AddSeconds(9) },
				new Team() { Name = "OmaHAHA CLINTON-...", Owner = "KJ", LastBid = DateTime.Now.AddSeconds(10) },
				new Team() { Name = "Sweed Emotion", Owner = "Joe", LastBid = DateTime.Now.AddSeconds(11) },
				new Team() { Name = "Saginaw Sasquatch", Owner = "Dave", LastBid = DateTime.Now.AddSeconds(12) }
			);

			context.NflTeams.AddOrUpdate(
				n => n.Name,
				new NflTeam() { Name = "Arizona Cardinals" },
				new NflTeam() { Name = "Atlanta Falcons" },
				new NflTeam() { Name = "Baltimore Ravens" },
				new NflTeam() { Name = "Buffalo Bills" },
				new NflTeam() { Name = "Carolina Panthers" },
				new NflTeam() { Name = "Chicago Bears" },
				new NflTeam() { Name = "Cincinnati Bengals" },
				new NflTeam() { Name = "Cleveland Browns" },
				new NflTeam() { Name = "Dallas Cowboys" },
				new NflTeam() { Name = "Denver Broncos" },
				new NflTeam() { Name = "Detroit Lions" },
				new NflTeam() { Name = "Green Bay Packers" },
				new NflTeam() { Name = "Houston Texans" },
				new NflTeam() { Name = "Indianapolis Colts" },
				new NflTeam() { Name = "Jacksonville Jaguars" },
				new NflTeam() { Name = "Kansas City Chiefs" },
				new NflTeam() { Name = "Miami Dolphins" },
				new NflTeam() { Name = "Minnesota Vikings" },
				new NflTeam() { Name = "New England Patriots" },
				new NflTeam() { Name = "New Orleans Saints" },
				new NflTeam() { Name = "New York Giants" },
				new NflTeam() { Name = "New York Jets" },
				new NflTeam() { Name = "Oakland Raiders" },
				new NflTeam() { Name = "Philadelphia Eagles" },
				new NflTeam() { Name = "Pittsburgh Steelers" },
				new NflTeam() { Name = "San Diego Chargers" },
				new NflTeam() { Name = "San Francisco 49ers" },
				new NflTeam() { Name = "Seattle Seahawks" },
				new NflTeam() { Name = "St. Louis Rams" },
				new NflTeam() { Name = "Tampa Bay Buccaneers" },
				new NflTeam() { Name = "Tennessee Titans" },
				new NflTeam() { Name = "Washington Redskins" }
			);
			context.SaveChanges();
		}
	}
}
