using AuctionTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuctionTracker.Controllers
{
    public class PlayersController : ApiController
    {
		public IHttpActionResult Get()
		{
			var dc = new AuctionTrackerContext();

			var result = (from p in dc.Players
						  orderby p.ID descending
						  select new
						  {
							  ID = p.ID,
							  NflTeam = p.NflTeam,
							  Team = p.Team.Name,
							  TeamId = p.TeamID,
							  FullName = p.FullName,
							  Position = p.Position,
							  AuctionAmount = p.AuctionAmount,
						  }).Take(3);
			return Ok(result.ToList());
		}
    }
}
