using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace JobsAuctions.Hubs
{
    public class AuctionHub : Hub
    {
      public void ExpiringAuction(string title, DateTime closingTime)
        {
            Clients.All.expiringAuction(title, closingTime);
        }
    }
}