using AuctionTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuctionTracker.Controllers
{
    public class NflTeamsController : ApiController
    {
		public List<NflTeam> Get()
		{
			return new AuctionTrackerContext().NflTeams.ToList();
		}
    }
}
