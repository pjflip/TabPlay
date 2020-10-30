// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class HandsDisplayController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction, int boardNumber, int pairNumber)
        {
            HandsDisplay handsDisplay = new HandsDisplay (sectionID, tableNumber, roundNumber, direction, boardNumber, pairNumber);
            ViewData["Buttons"] = ButtonOptions.OKEnabled;
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {section.SectionLetter + tableNumber.ToString()} - Round {roundNumber} - Board {boardNumber}";
            }
            else
            {
                ViewData["Header"] = $"Table {section.SectionLetter + tableNumber.ToString()} - Round {roundNumber} - Board {boardNumber}";
            }
            ViewData["Title"] = $"Hands - {AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter + tableNumber.ToString()} {direction}";
            return View(handsDisplay);
        }
    }
}
