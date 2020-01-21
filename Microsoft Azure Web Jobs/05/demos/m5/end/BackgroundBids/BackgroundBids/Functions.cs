using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace BackgroundBids
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("bids")] Bid message, 
            [Blob("bids/bidlog.txt", FileAccess.Write)] TextWriter bidWriter, TextWriter log)
        {
            bidWriter.WriteLine($"New bid arrived for {message.ItemId} in the amount of {message.Amount} from bidder {message.Bidder}");
            log.WriteLine(message);
        }
    }
}
