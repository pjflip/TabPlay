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
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (boardNumber > tableStatus.HighBoard)
            {
                return RedirectToAction("Index", "RankingList", new { sectionID, tableNumber, roundNumber, direction, pairNumber = tableStatus.PairNumber[Utilities.DirectionToNumber(direction)] });
            }
            
            // Only the first player to reach this screen will do the refresh
            if (roundNumber > tableStatus.RoundNumber)
            {
                tableStatus.NewRound(roundNumber);
            }
            else if (boardNumber > tableStatus.BoardNumber)
            {
                tableStatus.NewBoard(boardNumber);
            }

            int directionNumber = Utilities.DirectionToNumber(direction);
            tableStatus.Registered[directionNumber] = true;
            tableStatus.UpdateTime[directionNumber] = DateTime.Now;

            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            if (tableStatus.PairNumber[0] == section.MissingPair) tableStatus.PairNumber[0] = tableStatus.PairNumber[2] = 0;
            if (tableStatus.PairNumber[1] == section.MissingPair) tableStatus.PairNumber[1] = tableStatus.PairNumber[3] = 0;
            RegisterPlayers registerPlayers = new RegisterPlayers(tableStatus, direction);

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

            if (tableStatus.PairNumber[0] == 0 || tableStatus.PairNumber[1] == 0)  // Sit out
            {
                return View("SitOut", registerPlayers);
            }
            else
            {
                return View("FullTable", registerPlayers);
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
            
            // Refresh registrations now to avoid any race condition when moving to new table
            tableStatus.Registered = new bool[4] { false, false, false, false };
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