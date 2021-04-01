using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class BiddingController : Controller
    {
        public ActionResult Index(int deviceNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            Bidding bidding = new Bidding(deviceNumber, table);
            if (device.Direction == "North")
            {
                ViewData["Buttons"] = ButtonOptions.Skip;
            }
            else
            {
                ViewData["Buttons"] = ButtonOptions.None;
            }
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {device.SectionTableString} - Round {table.RoundNumber} - Board {table.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", table.BoardNumber, $"{table.PairNumber[0]}+{table.PairNumber[2]}")} v {Utilities.ColourPairByVulnerability("EW", table.BoardNumber, $"{table.PairNumber[1]}+{table.PairNumber[3]}")} - Dealer {bidding.Dealer}";
            }
            else
            {
                ViewData["Header"] = $"Table {device.SectionTableString} - Round {table.RoundNumber} - Board {table.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", table.BoardNumber, $"NS {table.PairNumber[0]}")} v {Utilities.ColourPairByVulnerability("EW", table.BoardNumber, $"EW {table.PairNumber[1]}")} - Dealer {bidding.Dealer}";
            }
            ViewData["Title"] = $"Bidding - {device.SectionTableString}:{device.Direction}";
            return View(bidding);
        }

        public JsonResult SendBid(int deviceNumber, int boardNumber, int lastBidLevel, string lastBidSuit, string lastBidX, bool alert, string lastBidDirection, int passCount, int bidCounter)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            Bid bid = new Bid(device.Direction, lastBidLevel, lastBidSuit, lastBidX, alert, lastBidDirection, passCount, bidCounter);
            bid.UpdateDB(deviceNumber, boardNumber);
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            table.LastBid = bid;
            if ((lastBidLevel > 0 && passCount > 2) || passCount > 3)
            {
                if (passCount > 3) 
                {
                    // Passed out, so no play
                    Result result = new Result(deviceNumber, boardNumber, 0);
                    result.UpdateDB("ReceivedData");
                    table.PlayComplete = true;
                }
                else
                {
                    Result result = new Result(deviceNumber, boardNumber, lastBidLevel, lastBidSuit, lastBidX);
                    result.UpdateDB("IntermediateData");
                }
                table.BiddingComplete = true;
            }
            return Json(bid, JsonRequestBehavior.AllowGet);
        }

        public EmptyResult Skip(int deviceNumber, int boardNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            Result result = new Result(deviceNumber, boardNumber, -1);
            result.UpdateDB("ReceivedData");
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            table.PlayComplete = true;
            table.LastBid.LastBidLevel = -1;
            table.BiddingComplete = true;
            return new EmptyResult();
        }

        public JsonResult PollBid(int deviceNumber, int bidCounter)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            if (table.BiddingComplete)
            {
                return Json(new { Status = "BiddingComplete", table.LastBid.LastBidLevel }, JsonRequestBehavior.AllowGet);
            }
            else if (table.LastBid.BidCounter > bidCounter)
            {
                return Json(table.LastBid, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "None" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
