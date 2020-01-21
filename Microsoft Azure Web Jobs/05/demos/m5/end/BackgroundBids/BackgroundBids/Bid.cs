using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundBids
{
    public class Bid
    {
        public double Amount { get; set; }

        public int ItemId { get; set; }

        public string Bidder { get; set; }
    }
}
