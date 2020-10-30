// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class PlayingController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction, int boardNumber)
        {
            Playing playing = new Playing(sectionID, tableNumber, roundNumber, direction, boardNumber);

            ViewData["Buttons"] = ButtonOptions.Claim;
            string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {roundNumber} - Board {boardNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{playing.PairNS}+{playing.South}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{playing.PairEW}+{playing.West}")}";
            }
            else
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {roundNumber} - Board {boardNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {playing.PairNS}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {playing.PairEW}")}";
            }
            ViewData["Title"] = $"Playing - {sectionLetter + tableNumber.ToString()} {direction}";
            return View(playing);
        }

        public EmptyResult SendPlay(int sectionID, int tableNumber, int roundNumber, int boardNumber, string direction, int cardNumber, string cardString, int playCounter)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Play play = new Play(direction, cardNumber, cardString, playCounter);
            play.UpdateDB(sectionID, tableNumber, roundNumber, boardNumber);
            AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).LastPlay = play;
            if (playCounter == 0)  // First play, so update opening lead in results table
            {
                Result result = new Result(sectionID, tableNumber, roundNumber, boardNumber, "IntermediateData");
                result.LeadCard = cardString;
                result.UpdateDB("IntermediateData");
            }
            return new EmptyResult();
        }

        public EmptyResult SendOpeningLead(int sectionID, int tableNumber, string direction, int cardNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            // PlayCounter = -1 indicates opening lead tabled but not played
            AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).LastPlay = new Play(direction, cardNumber, "", -1);
            return new EmptyResult();
        }

        public EmptyResult SendResult(int sectionID, int tableNumber, int roundNumber, int boardNumber, int tricks)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Result result = new Result(sectionID, tableNumber, roundNumber, boardNumber, "IntermediateData");
            result.TricksTakenNumber = tricks;
            result.UpdateDB("ReceivedData");
            AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).PlayComplete = true;
            return new EmptyResult();
        }

        public EmptyResult SendClaimExpose(int sectionID, int tableNumber, string claimDirection)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            tableStatus.ClaimExpose = true;
            tableStatus.ClaimDirection = claimDirection;
            return new EmptyResult();
        }

        public JsonResult PollPlay(int sectionID, int tableNumber, int playCounter)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.PlayComplete)
            {
                return Json(new { Status = "PlayComplete" }, JsonRequestBehavior.AllowGet);
            }
            else if (tableStatus.LastPlay.PlayCounter > playCounter)
            {
                return Json(tableStatus.LastPlay, JsonRequestBehavior.AllowGet);
            }
            else if (tableStatus.ClaimExpose)
            {
                return Json(new { Status = "ClaimExpose", tableStatus.ClaimDirection }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "None" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

