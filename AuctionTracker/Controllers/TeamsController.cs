using AuctionTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;

namespace AuctionTracker.Controllers
{
    public class TeamsController : ApiController
    {
		public List<Team> Get()
		{
			return new AuctionTrackerContext().Teams.OrderBy(t => t.Name).ToList();
        }

		[Route("api/Teams/Players")]
		[HttpGet]
		public IHttpActionResult Players()
		{
			var dc = new AuctionTrackerContext();
			var teams = dc.Teams.Include(t => t.Players).ToList();

			return Ok(teams);
		}

		[Route("api/Teams/AuctionOrder")]
		[HttpGet]
		public IHttpActionResult AuctionOrder()
		{
			return Ok(new AuctionTrackerContext().Teams.OrderBy(t => t.LastBid).ToList());
		}
	}

}
