// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class PlayingController : Controller
    {
        public ActionResult Index(int deviceNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            Playing playing = new Playing(deviceNumber, table);

            ViewData["Buttons"] = ButtonOptions.Claim;
            string sectionLetter = AppData.SectionList.Find(x => x.SectionID == device.SectionID).SectionLetter;
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {device.SectionTableString} - Round {device.RoundNumber} - Board {table.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", table.BoardNumber, $"{table.PairNumber[0]}+{table.PairNumber[2]}")} v {Utilities.ColourPairByVulnerability("EW", table.BoardNumber, $"{table.PairNumber[1]}+{table.PairNumber[3]}")}";
            }
            else
            {
                ViewData["Header"] = $"Table {device.SectionTableString} - Round {table.RoundNumber} - Board {table.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", table.BoardNumber, $"NS {table.PairNumber[0]}")} v {Utilities.ColourPairByVulnerability("EW", table.BoardNumber, $"EW {table.PairNumber[1]}")}";
            }
            ViewData["Title"] = $"Playing - {device.SectionTableString}:{device.Direction}";
            return View(playing);
        }

        public EmptyResult SendPlay(int deviceNumber, int boardNumber, string direction, int cardNumber, string cardString, int playCounter)
        {
            // direction might be different to device.Direction as could be a play from dummy
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            Play play = new Play(direction, cardNumber, cardString, playCounter);
            play.UpdateDB(deviceNumber, boardNumber);
            AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber).LastPlay = play;
            if (playCounter == 0)  // First play, so update opening lead in results table
            {
                Result result = new Result(deviceNumber, boardNumber, "IntermediateData");
                result.LeadCard = cardString;
                result.UpdateDB("IntermediateData");
            }
            return new EmptyResult();
        }

        public EmptyResult SendOpeningLead(int deviceNumber, int cardNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            // PlayCounter = -1 indicates opening lead tabled but not played
            AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber).LastPlay = new Play(device.Direction, cardNumber, "", -1);
            return new EmptyResult();
        }

        public EmptyResult SendResult(int deviceNumber, int boardNumber, int tricks)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            Result result = new Result(deviceNumber, boardNumber, "IntermediateData");
            result.TricksTakenNumber = tricks;
            result.UpdateDB("ReceivedData");
            AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber).PlayComplete = true;
            return new EmptyResult();
        }

        public EmptyResult SendClaimExpose(int deviceNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            table.ClaimExpose = true;
            table.ClaimDirection = device.Direction;
            return new EmptyResult();
        }

        public JsonResult PollPlay(int deviceNumber, int playCounter)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            if (table.PlayComplete)
            {
                return Json(new { Status = "PlayComplete" }, JsonRequestBehavior.AllowGet);
            }
            else if (table.LastPlay.PlayCounter > playCounter)
            {
                return Json(table.LastPlay, JsonRequestBehavior.AllowGet);
            }
            else if (table.ClaimExpose)
            {
                return Json(new { Status = "ClaimExpose", table.ClaimDirection }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "None" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

