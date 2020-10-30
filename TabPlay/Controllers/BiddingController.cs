using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class BiddingController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction, int boardNumber)
        {
            Bidding bidding= new Bidding(sectionID, tableNumber, roundNumber, direction, boardNumber);
            if (direction == "North")
            {
                ViewData["Buttons"] = ButtonOptions.Skip;
            }
            else
            {
                ViewData["Buttons"] = ButtonOptions.None;
            }
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {section.SectionLetter + tableNumber.ToString()} - Round {roundNumber} - Board {boardNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{bidding.PairNS}+{bidding.South}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{bidding.PairEW}+{bidding.West}")} - Dealer {bidding.Dealer}";
            }
            else
            {
                ViewData["Header"] = $"Table {section.SectionLetter + tableNumber.ToString()} - Round {roundNumber} - Board {boardNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {bidding.PairNS}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {bidding.PairEW}")} - Dealer {bidding.Dealer}";
            }
            ViewData["Title"] = $"Bidding - {section.SectionLetter + tableNumber.ToString()} {direction}";
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
                tableStatus.BiddingComplete = true;
                if (passCount > 3) {
                    // Passed out, so no play
                    new Result(sectionID, tableNumber, roundNumber, boardNumber, 0).UpdateDB("ReceivedData");
                    tableStatus.PlayComplete = true;
                }
                else
                {
                    new Result(sectionID, tableNumber, roundNumber, boardNumber, lastBidLevel, lastBidSuit, lastBidX).UpdateDB("IntermediateData");
                }
            }
            return Json(bid, JsonRequestBehavior.AllowGet);
        }

        public EmptyResult Skip(int sectionID, int tableNumber, int roundNumber, int boardNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            new Result(sectionID, tableNumber, roundNumber, boardNumber, -1).UpdateDB("ReceivedData");
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            tableStatus.BiddingComplete = true;
            tableStatus.PlayComplete = true;
            tableStatus.LastBid.LastBidLevel = -1;
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
