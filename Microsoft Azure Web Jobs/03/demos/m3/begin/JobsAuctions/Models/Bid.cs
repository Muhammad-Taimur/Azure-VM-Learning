using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobsAuctions.Models
{
    public class Bid
    {
        public int Id { get; set; }

        public float Amount { get; set; }

        public virtual AuctionItem Item { get; set; }

        [ForeignKey("Item")]
        [Column("AuctionItem_Id")]
        public int AuctionItemId { get; set; }

        public  string Bidder { get; set; }
    }
}