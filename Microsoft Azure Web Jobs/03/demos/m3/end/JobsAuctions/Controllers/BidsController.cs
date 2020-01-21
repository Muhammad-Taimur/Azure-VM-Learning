using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsAuctions.Models;
using Microsoft.AspNet.SignalR;

namespace JobsAuctions.Controllers
{
    public class BidsController : Controller
    {
        private AuctionDbContext db = new AuctionDbContext();

        // GET: Bids
        public ActionResult Index(int id)
        {
            return View(db.Bids.Where(b=> b.AuctionItemId == id).ToList());
        }

        

        // GET: Bids/Create
        public ActionResult Create(int id)
        {
            ViewBag.AuctionItemId = new SelectList(db.AuctionItems, "Id", "Name");
            return View(new Bid { AuctionItemId = id });
        }

        // POST: Bids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Amount,AuctionItemId,Bidder")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                db.Bids.Add(bid);
                db.SaveChanges();

                var context = GlobalHost.ConnectionManager.GetHubContext<Hubs.AuctionHub>();
                context.Clients.All.newBid(bid.AuctionItemId, bid.Amount);

                return RedirectToAction("Index", new { id = bid.AuctionItemId });
            }

            return View(bid);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
