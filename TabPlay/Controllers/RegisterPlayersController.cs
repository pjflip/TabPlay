// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class RegisterPlayersController : Controller
    {
        public ActionResult Index(int deviceNumber, int boardNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            if (boardNumber > table.HighBoard)
            {
                return RedirectToAction("Index", "RankingList", new { deviceNumber });
            }
            
            // Only the first player to reach this screen will do the table refresh, as this will update the table's round number to the new round
            if (device.RoundNumber > table.RoundNumber)
            {
                table.NewRound(device.RoundNumber);
            }
            else if (boardNumber > table.BoardNumber)
            {
                table.NewBoard(boardNumber);
            }

            int directionNumber = Utilities.DirectionToNumber(device.Direction);
            table.Registered[directionNumber] = true;
            table.UpdateTime[directionNumber] = DateTime.Now;

            Section section = AppData.SectionList.Find(x => x.SectionID == device.SectionID);
            if (table.PairNumber[0] == section.MissingPair) table.PairNumber[0] = table.PairNumber[2] = 0;
            if (table.PairNumber[1] == section.MissingPair) table.PairNumber[1] = table.PairNumber[3] = 0;
            RegisterPlayers registerPlayers = new RegisterPlayers(deviceNumber, table);

            ViewData["Header"] = $"Table {device.SectionTableString} - {device.Direction}";
            ViewData["Title"] = $"Register - {device.SectionTableString}:{device.Direction}";
            ViewData["Buttons"] = ButtonOptions.OKInvisible;

            if (table.PairNumber[0] == 0 || table.PairNumber[1] == 0)  // Sit out
            {
                return View("SitOut", registerPlayers);
            }
            else
            {
                return View("FullTable", registerPlayers);
            }
        }

        public JsonResult EnterPlayerNumber(int deviceNumber, int playerNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            device.UpdatePlayerName(playerNumber);
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            int directionNumber = Utilities.DirectionToNumber(device.Direction);
            table.PlayerName[directionNumber] = device.PlayerName;
            table.UpdateTime[directionNumber] = DateTime.Now;
            return Json(device.PlayerName, JsonRequestBehavior.AllowGet);
        }

        public EmptyResult OKButtonClick(int deviceNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            table.BiddingStarted = true;
            
            // Refresh registrations now to avoid any race condition when moving to new table
            table.Registered = new bool[4] { false, false, false, false };
            return new EmptyResult();
        }

        public JsonResult PollRegister(int deviceNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            if (table.BiddingStarted == true)
            {
                return Json(new { Status = "BiddingStarted" }, JsonRequestBehavior.AllowGet);
            }
            int directionNumber = Utilities.DirectionToNumber(device.Direction);
            bool anyUpdates = false;
            DateTime myUpdateTime = table.UpdateTime[directionNumber];
            for (int i = 0; i < 4; i++)
            {
                if (table.UpdateTime[i] > myUpdateTime)
                {
                    anyUpdates = true;
                    table.UpdateTime[directionNumber] = table.UpdateTime[i];
                }
            }
            if (anyUpdates)
            {
                PlayerStatus playerStatus = new PlayerStatus(directionNumber, table);
                return Json(playerStatus, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "None" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}