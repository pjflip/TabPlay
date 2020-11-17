using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class BiddingController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, string direction)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            Bidding bidding = new Bidding(tableStatus, direction);
            if (direction == "North")
            {
                ViewData["Buttons"] = ButtonOptions.Skip;
            }
            else
            {
                ViewData["Buttons"] = ButtonOptions.None;
            }
            string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {tableStatus.RoundNumber} - Board {tableStatus.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.BoardNumber, $"{tableStatus.PairNumber[0]}+{tableStatus.PairNumber[2]}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.BoardNumber, $"{tableStatus.PairNumber[1]}+{tableStatus.PairNumber[3]}")} - Dealer {bidding.Dealer}";
            }
            else
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {tableStatus.RoundNumber} - Board {tableStatus.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.BoardNumber, $"NS {tableStatus.PairNumber[0]}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.BoardNumber, $"EW {tableStatus.PairNumber[1]}")} - Dealer {bidding.Dealer}";
            }
            ViewData["Title"] = $"Bidding - {sectionLetter + tableNumber.ToString()} {direction}";
            return View(bidding);
        }

        public JsonResult SendBid(int sectionID, int tableNumber, int roundNumber, string callDirection, int boardNumber, int lastBidLevel, string lastBidSuit, string lastBidX, bool alert, string lastBidDirection, int passCount, int bidCounter)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Bid bid = new Bid(callDirection, lastBidLevel, lastBidSuit, lastBidX, alert, lastBidDirection, passCount, bidCounter);
            bid.UpdateDB(sectionID, tableNumber, roundNumber, boardNumber);
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            tableStatus.LastBid = bid;
            if ((lastBidLevel > 0 && passCount > 2) || passCount > 3)
            {
                if (passCount > 3) 
                {
                    // Passed out, so no play
                    Result result = new Result(sectionID, tableNumber, roundNumber, boardNumber, 0);
                    result.UpdateDB("ReceivedData");
                    tableStatus.PlayComplete = true;
                }
                else
                {
                    Result result = new Result(sectionID, tableNumber, roundNumber, boardNumber, lastBidLevel, lastBidSuit, lastBidX);
                    result.UpdateDB("IntermediateData");
                }
                tableStatus.BiddingComplete = true;
            }
            return Json(bid, JsonRequestBehavior.AllowGet);
        }

        public EmptyResult Skip(int sectionID, int tableNumber, int roundNumber, int boardNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Result result = new Result(sectionID, tableNumber, roundNumber, boardNumber, -1);
            result.UpdateDB("ReceivedData");
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            tableStatus.PlayComplete = true;
            tableStatus.LastBid.LastBidLevel = -1;
            tableStatus.BiddingComplete = true;
            return new EmptyResult();
        }

        public JsonResult PollBid(int sectionID, int tableNumber, int bidCounter)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.BiddingComplete)
            {
                return Json(new { Status = "BiddingComplete", tableStatus.LastBid.LastBidLevel }, JsonRequestBehavior.AllowGet);
            }
            else if (tableStatus.LastBid.BidCounter > bidCounter)
            {
                return Json(tableStatus.LastBid, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "None" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
