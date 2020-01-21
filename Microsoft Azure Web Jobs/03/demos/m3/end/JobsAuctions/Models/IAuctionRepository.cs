using System;
using System.Linq;
namespace JobsAuctions.Models
{
    public interface IAuctionRepository:IDisposable
    {
        //create
        Auction Create(Auction item);
        AuctionItem Create(AuctionItem item);
        

        //retrieve
        Auction GetAuction(int id);

        System.Linq.IQueryable<Auction> GetAuctions(bool expandItems = false);

        //update
        Auction UpdateItem(Auction item);

        AuctionItem UpdateItem(AuctionItem item);

        //delete
        void Delete<T>(int id) where T : class;
        
        void DeleteAuction(int id);

        void DeleteItem(int id);


        //bids
        bool TryAddBid(Bid model);

        IQueryable<Bid> GetBids();

        Auction GetAuctionForItem(int itemId);
    }
}
