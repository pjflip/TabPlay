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
                    return RedirectToAction("Index", "FinalRankingList", new { sectionID, tableNumber, roundNumber, direction, pairNumber });
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen", new { sectionID, tableNumber, roundNumber, direction });
                }
            }

            Move move = new Move(sectionID, tableNumber, roundNumber, direction);

            string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
            ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()}";
            ViewData["Buttons"] = ButtonOptions.OKEnabled;
            ViewData["Title"] = $"Move - {sectionLetter + tableNumber.ToString()}";
            return View(move);
        }

        public ActionResult OKButtonClick(int sectionID, int newTableNumber, int newRoundNumber, string newDirection)
        {
            // Refresh settings for the start of the round
            Settings.Refresh();
            return RedirectToAction("Index", "RegisterPlayers", new { sectionID, tableNumber = newTableNumber, roundNumber = newRoundNumber, direction = newDirection, boardNumber = 0 });
        }
    }
}