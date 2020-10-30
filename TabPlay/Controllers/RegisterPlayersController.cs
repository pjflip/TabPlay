// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class RegisterPlayersController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction, int boardNumber)
        {
            Round round = new Round(sectionID, tableNumber, roundNumber, direction, boardNumber);
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (boardNumber > round.HighBoard)
            {
                tableStatus.RoundComplete = true;
                return RedirectToAction("Index", "RankingList", new { sectionID, tableNumber, roundNumber, direction, pairNumber = round.PairNumber[0] });
            }
            tableStatus.Reset(round, direction);

            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            ViewData["Header"] = $"Table {section.SectionLetter + tableNumber.ToString()} - {direction}";
            ViewData["Title"] = $"Register Players - {section.SectionLetter + tableNumber.ToString()} {direction}";

            if (direction == "North")
            {
                ViewData["Buttons"] = ButtonOptions.OKDisabled;
            }
            else
            {
                ViewData["Buttons"] = ButtonOptions.None;
            }
            if (round.PairNS == section.MissingPair) round.PairNS = 0;
            if (round.PairEW == section.MissingPair) round.PairEW = 0;
            if (round.PairNS == 0 || round.PairEW == 0)  // Sit out
            {
                return View("SitOut", round);
            }
            else
            {
                return View("FullTable", round);
            }
        }

        public JsonResult EnterPlayerNumber(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber, int playerNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            int directionNumber = Utilities.DirectionToNumber(direction);
            PlayerName playerName = new PlayerName(sectionID, tableNumber, roundNumber, direction, pairNumber, playerNumber);
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            tableStatus.PlayerName[directionNumber] = playerName.Name;
            tableStatus.UpdateTime[directionNumber] = DateTime.Now;
            return Json(new { playerName.Name }, JsonRequestBehavior.AllowGet);
        }

        public EmptyResult OKButtonClick(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            tableStatus.BiddingStarted = true;
            tableStatus.Registered = new bool[4]{ false, false, false, false};
            return new EmptyResult();
        }

        public JsonResult PollRegister(int sectionID, int tableNumber, string direction)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.BiddingStarted == true)
            {
                return Json(new { Status = "BiddingStarted" }, JsonRequestBehavior.AllowGet);
            }
            int directionNumber = Utilities.DirectionToNumber(direction);
            bool anyUpdates = false;
            DateTime myUpdateTime = tableStatus.UpdateTime[directionNumber];
            for (int i = 0; i < 4; i++)
            {
                if (tableStatus.UpdateTime[i] > myUpdateTime)
                {
                    anyUpdates = true;
                    tableStatus.UpdateTime[directionNumber] = tableStatus.UpdateTime[i];
                }
            }
            if (anyUpdates)
            {
                PlayerStatus playerStatusUpdate = new PlayerStatus(directionNumber, tableStatus);
                return Json(playerStatusUpdate, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "None" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}