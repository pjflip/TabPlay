// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class MoveController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber)
        {
            if (roundNumber >= Utilities.NumberOfRoundsInEvent(sectionID))  // Session complete
            {
                if (Settings.ShowRanking == 2)
                {
                    return RedirectToAction("Final", "RankingList", new { sectionID, tableNumber, roundNumber, direction, pairNumber });
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen", new { sectionID, tableNumber, roundNumber, direction });
                }
            }

            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.RoundNumber == roundNumber)
            {
                // Nobody has yet advanced this table to the next round, so show that this player is ready to do so
                tableStatus.ReadyForNextRound[Utilities.DirectionToNumber(direction)] = true;
            }
            Move move = new Move(sectionID, tableNumber, roundNumber, direction, pairNumber);

            string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
            ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()}";
            ViewData["Buttons"] = ButtonOptions.OKEnabled;
            ViewData["Title"] = $"Move - {sectionLetter + tableNumber.ToString()}";
            return View(move);
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber, int newTableNumber, int newRoundNumber, string newDirection)
        {
            // Moving to sit out (ie not a proper) table.  So go straight to ranking list
            if (newTableNumber == 0)
            {
                return RedirectToAction("Index", "RankingList", new { sectionID, tableNumber = 0, roundNumber = newRoundNumber, direction, pairNumber });
            }

            // Check if new table is ready to play.  Test for null in case new table not yet started (unlikely in reality)
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == newTableNumber);
            if (tableStatus != null) {
                // A player is ready if he's reached the Move screen or he's a missing pair
                bool allPlayersReady = (tableStatus.PairNumber[0] == 0 || tableStatus.ReadyForNextRound[0]) && (tableStatus.PairNumber[1] == 0 || tableStatus.ReadyForNextRound[1]) && (tableStatus.PairNumber[2] == 0 || tableStatus.ReadyForNextRound[2]) && (tableStatus.PairNumber[3] == 0 || tableStatus.ReadyForNextRound[3]);
                if (tableStatus.RoundNumber == newRoundNumber || allPlayersReady)
                {
                    // Refresh settings for the start of the round
                    Settings.Refresh();
                    return RedirectToAction("Index", "RegisterPlayers", new { sectionID, tableNumber = newTableNumber, roundNumber = newRoundNumber, direction = newDirection, boardNumber = 0 });
                }
            } 
            TempData["TableNotReady"] = "TRUE"; 
            return RedirectToAction("Index", "Move", new { sectionID, tableNumber, roundNumber, direction, pairNumber });
        }
    }
}