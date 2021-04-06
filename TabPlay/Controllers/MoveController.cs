// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class MoveController : Controller
    {
        public ActionResult Index(int deviceNumber, bool tableNotReady = false)
        {
            Device device = AppData.DeviceList[deviceNumber];
            if (device.RoundNumber >= Utilities.NumberOfRoundsInEvent(device.SectionID))  // Session complete
            {
                if (Settings.ShowRanking == 2)
                {
                    return RedirectToAction("Final", "RankingList", new { deviceNumber });
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen", new { deviceNumber });
                }
            }

            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            if (table != null && table.RoundNumber == device.RoundNumber)
            {
                // Nobody has yet advanced this table to the next round, so show that this player is ready to do so
                table.ReadyForNextRound[Utilities.DirectionToNumber(device.Direction)] = true;
            }
            Move move = new Move(deviceNumber, tableNotReady);

            ViewData["Header"] = $"Table {device.SectionTableString} - {device.Direction}";
            ViewData["Buttons"] = ButtonOptions.OKVisible;
            ViewData["Title"] = $"Move - {device.SectionTableString}:{device.Direction}";
            return View(move);
        }

        public ActionResult OKButtonClick(int deviceNumber, int newTableNumber, int newRoundNumber, string newDirection)
        {
            Device device = AppData.DeviceList[deviceNumber];

            // Moving to sit out (ie not a proper) table.  So go straight to ranking list
            if (newTableNumber == 0)
            {
                device.UpdateMove(newRoundNumber, 0, newDirection);
                return RedirectToAction("Index", "RankingList", new { deviceNumber });
            }

            // Check if new table is ready to play.  Test for null in case new table not yet started (unlikely in reality)
            Table newTable = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == newTableNumber);
            if (newTable != null) {
                // A player at the new table is ready if he's reached the Move screen or he's a missing pair
                bool allPlayersReady = (newTable.PairNumber[0] == 0 || newTable.ReadyForNextRound[0]) && (newTable.PairNumber[1] == 0 || newTable.ReadyForNextRound[1]) && (newTable.PairNumber[2] == 0 || newTable.ReadyForNextRound[2]) && (newTable.PairNumber[3] == 0 || newTable.ReadyForNextRound[3]);

                // Can move if new table has already been advanced to next round by another player, or (if not yet on next round) everyone at the new table is ready to move
                if (newTable.RoundNumber == newRoundNumber || (newTable.RoundNumber == device.RoundNumber && allPlayersReady))
                {
                    // Refresh settings for the start of the round
                    Settings.Refresh();
                    device.UpdateMove(newRoundNumber, newTableNumber, newDirection);
                    return RedirectToAction("Index", "RegisterPlayers", new { deviceNumber, boardNumber = 0 });
                }
            } 
            
            // Otherwise go back and wait
            return RedirectToAction("Index", "Move", new { deviceNumber, tableNotReady = true });
        }
    }
}