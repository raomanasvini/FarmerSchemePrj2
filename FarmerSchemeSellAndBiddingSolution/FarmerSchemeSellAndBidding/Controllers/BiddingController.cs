﻿using FarmerSchemeSellAndBidding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FarmerSchemeSellAndBidding.Controllers
{
    public class BiddingController : ApiController
    {
        private FarmerSchemeDBEntities4 db = new FarmerSchemeDBEntities4();
       //GET: api/Bidding
        public IHttpActionResult GetFarmerCropdetils()
        {
            //var data=(db.MarketViewForBidder()).ToList();
            return Ok(db.MarketViewForBidderwithMaxBidAmount());
        }
        [HttpPost]
        //public IHttpActionResult PostBidAmount(string FarmerSellID,string amount,string userid)
        public IHttpActionResult PostBidAmount(dynamic bidclass)
        {
            BiddingTable biddingTable = new BiddingTable();
            int bidamount;
            int farsellid;
            int userid;
            foreach (var item in bidclass)
            {
                int i = 0;
                foreach(var ite in item)
                {
                    if(i==0)
                    {
                        foreach (var it in ite)
                        {
                            farsellid = it;
                            biddingTable.FarmerSellID = farsellid;
                            i++;
                        }
                    }
                    else if(i==1)
                    {
                        foreach (var it in ite)
                        {
                            bidamount = it;
                            biddingTable.bidAmount = bidamount;
                            i++;
                        }

                    }
                     else
                    {
                        foreach (var it in ite)
                        {
                            userid = it;
                            biddingTable.UserID = userid;
                        }

                    }

                }
            }

            //biddingTable.bidAmount = bidamount;
            //biddingTable.FarmerSellID = farsellid;
            //biddingTable.UserID= userid;
            db.BiddingTables.Add(biddingTable);
            db.SaveChanges();

            return Ok("You applied for bid.");
        }
    }
}
