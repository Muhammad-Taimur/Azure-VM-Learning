using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobsAuctions.Models
{
    public class AuctionItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float StartingAmount { get; set; }

 
        public string Donor { get; set; }

 
        public virtual Auction Auction { get; set; }

        [ForeignKey("Auction")]
        [Column("Auction_Id")]
        public int AuctionId { get; set; }

        public  virtual ICollection<Bid> Bids { get; set; }


    }
}