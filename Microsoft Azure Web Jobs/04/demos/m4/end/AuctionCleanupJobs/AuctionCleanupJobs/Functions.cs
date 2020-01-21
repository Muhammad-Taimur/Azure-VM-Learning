using System;
using System.IO;
using System.Linq;

namespace AuctionCleanupJobs
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void CheckForAuctionsToBeDeleted(TextWriter log)
        {
            bool itemsRemoved = false;

            log.WriteLine("Checking for auctions that can be deleted");

            var ctx = new Models.AuctionDbContext();
            var query = from auct in ctx.Auctions
                        where auct.ClosingTime < DateTime.Now
                        select auct;

            foreach (var item in query)
            {
                itemsRemoved = true;
                ctx.Auctions.Remove(item);
            }

            if (itemsRemoved)
            {
                ctx.SaveChanges();
            }
            
        }
    }
}
