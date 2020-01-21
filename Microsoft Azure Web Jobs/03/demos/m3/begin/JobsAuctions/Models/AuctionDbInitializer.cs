using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JobsAuctions.Models
{
    public class AuctionDbInitializer : DropCreateDatabaseIfModelChanges<AuctionDbContext>
    {
        protected override void Seed(AuctionDbContext context)
        {
            base.Seed(context);

            if(context.Auctions.Count() != 1)
            {
                var auction = new Auction
                {
                    Title = "Nostalgic Items",
                    Description = "",
                    OpeningTime = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                    ClosingTime = DateTime.Now.AddDays(1),
                    Items = new List<AuctionItem>
                    {
                        new AuctionItem
                        {
                            Name = "Clash of the Titans Lunchbox",
                            Description = "Stop motion magic for lunch",
                            StartingAmount = 25,
                            Donor = "Matt Milner"
                        }
                    }
                };

                context.Auctions.Add(auction);
                context.SaveChanges();
            }
        }
    }
}