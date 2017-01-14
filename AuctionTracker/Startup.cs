using Microsoft.Owin;
using Owin;
using AuctionTracker;

[assembly: OwinStartup(typeof(AuctionTracker.Startup))]
namespace AuctionTracker
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}
