// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class HandsDisplayController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, string direction)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            HandsDisplay handsDisplay = new HandsDisplay (tableStatus, direction);
            ViewData["Buttons"] = ButtonOptions.OKEnabled;
            string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {tableStatus.RoundNumber} - Board {tableStatus.BoardNumber}";
            }
            else
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {tableStatus.RoundNumber} - Board {tableStatus.BoardNumber}";
            }
            ViewData["Title"] = $"Hands - {sectionLetter + tableNumber.ToString()} {direction}";
            return View(handsDisplay);
        }
    }
}
