// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class EndScreenController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction)
        {
            Location location = new Location
            {
                SectionID = sectionID,
                TableNumber = tableNumber,
                Direction = direction,
                RoundNumber = roundNumber
            };
            ViewData["Header"] = "";
            ViewData["Buttons"] = ButtonOptions.OKEnabled;
            ViewData["Header"] = $"Table {AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter + tableNumber.ToString()} {direction}";
            ViewData["Title"] = $"End Screen - {AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter + tableNumber.ToString()} {direction}";
            return View(location);
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, int roundNumber, string direction)
        {
            // Check if new round has been added; can't apply to individuals
            if (roundNumber >= Utilities.NumberOfRoundsInEvent(sectionID))  
            {
                // Final round, so no new rounds added
                return RedirectToAction("Index", "EndScreen", new { sectionID, tableNumber, roundNumber, direction });
            }
            else
            {
                return RedirectToAction("Index", "Move", new { sectionID, tableNumber, roundNumber, direction, pairNumber = 0 });
            }
        }
    }
}