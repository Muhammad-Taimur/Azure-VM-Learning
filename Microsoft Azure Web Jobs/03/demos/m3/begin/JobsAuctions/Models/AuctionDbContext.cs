using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JobsAuctions.Models
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext()
        {
            Database.SetInitializer(new AuctionDbInitializer());
        }
        public DbSet<Auction> Auctions { get; set; }

        public DbSet<AuctionItem> AuctionItems { get; set; }

        public DbSet<Bid> Bids { get; set; }


    }
}