using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Transactions;

namespace JobsAuctions.Models
{
    public class AuctionRepository : IAuctionRepository
    {
        AuctionDbContext ctx;

        public AuctionRepository(AuctionDbContext context)
        {
            ctx = context;
        }
        public IQueryable<Auction> GetAuctions(bool expandItems = false)
        {
            if (expandItems)
            {
                return ctx.Auctions.Include(a=>a.Items);
            }
            else
            {
                return ctx.Auctions;
            }
        }

        public Auction GetAuction(int id)
        {
            var auction = ctx.Auctions.Include(
                auc=>auc.Items.Select(i=>i.Bids)).Include(
                    auc=>auc.Items).Where(
                        a=>a.Id == id).FirstOrDefault();
 
            return auction;
        }

        public Auction Create(Auction item)
        {
            ctx.Auctions.Add(item);
            ctx.SaveChanges();
            return item;
        }

        public AuctionItem Create(AuctionItem item)
        {
            ctx.AuctionItems.Add(item);

            ctx.SaveChanges();
            return item;
        }

       
        public Auction UpdateItem(Auction item)
        {
            ctx.Auctions.Attach(item);
            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
            return item;
        }

        public AuctionItem UpdateItem(AuctionItem item)
        {
            ctx.AuctionItems.Attach(item);
            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
            return item;
        }

       

        public IQueryable<Bid> GetBids()
        {
            return ctx.Bids.Include(b => b.Item).Include(b=>b.Bidder);
        }

        public void DeleteAuction(int id)
        {
            var item = ctx.Auctions.Find(id);
            if(item == null)
            {
                throw new KeyNotFoundException();
            }
            else{
                ctx.Auctions.Remove(item);
                ctx.SaveChanges();
            }
        }

        public void DeleteItem(int id)
        {
            var item = ctx.AuctionItems.Find(id);
            if (item == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                ctx.AuctionItems.Remove(item);
                ctx.SaveChanges();
            } 
        }

        
        public void Delete<T>(int id)  where T: class
        {
            var item = ctx.Set<T>().Find(id);
            if (item == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                ctx.Set<T>().Remove(item);
                ctx.SaveChanges();
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    ctx.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool TryAddBid(Bid newBid)
        {  
            try
            {
                using (var scope = new TransactionScope())
                {
                    var maxCurrentBid = ctx.Bids.Where(b => b.Item.Id == newBid.Item.Id).Max(b => (int?)b.Amount);
                    var item = ctx.AuctionItems.Where(i=>i.Id == newBid.Item.Id).FirstOrDefault();
                    newBid.Item = item;

                    if (((maxCurrentBid == null || maxCurrentBid == 0) && newBid.Amount < item.StartingAmount) ||
                         newBid.Amount < maxCurrentBid + 5)
                    {
                        return false;
                    }
                    else
                    {
                        ctx.Bids.Add(newBid);
                        ctx.Entry(newBid.Bidder).State = EntityState.Unchanged;

                        ctx.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: log error
                return false;
            }
        }

        public Auction GetAuctionForItem(int itemId)
        {
            var item = ctx.AuctionItems.Where(i => i.Id == itemId).Include(it => it.Auction).FirstOrDefault();
            return item.Auction;
        }
    }
}