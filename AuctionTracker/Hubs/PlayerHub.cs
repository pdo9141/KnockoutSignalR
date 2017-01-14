using AuctionTracker.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace AuctionTracker.Hubs
{
	public class PlayerHub : Hub
	{
		public void AuctionPlayer(Player player)
		{
			var dc = new AuctionTrackerContext();
			dc.Players.Add(player);
			dc.SaveChanges();
			var team = dc.Teams.Where(t => t.Players.Count() < 16).OrderBy(t => t.LastBid).First();
			team.LastBid = DateTime.Now;
            dc.SaveChanges();

			player.TeamName = team.Name;

			Clients.All.playerAuctioned(player);
		}

		public void UpdatePlayer(Player player, int originalTeamId)
		{
			var dc = new AuctionTrackerContext();
			dc.Players.Attach(player);
			dc.Entry<Player>(player).State = System.Data.Entity.EntityState.Modified;
			dc.SaveChanges();
			player.TeamName = dc.Teams.Single(t => t.ID == player.TeamID).Name;

			Clients.All.playerUpdated(player, originalTeamId);
		}

		public void DeletePlayer(Player player)
		{
			var dc = new AuctionTrackerContext();
			dc.Players.Attach(player);
			dc.Entry<Player>(player).State = System.Data.Entity.EntityState.Deleted;
			dc.SaveChanges();
			Clients.All.playerDeleted(player);
		}
	}
}