// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class TravellerController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction, int boardNumber, int pairNumber)
        {
            if (!Settings.ShowResults)
            {
                return RedirectToAction("Index", "RegisterPlayers", new { sectionID, tableNumber, roundNumber, direction, boardNumber = boardNumber + 1 });
            }
            if (Settings.ShowHandRecord)
            {
                ViewData["Buttons"] = ButtonOptions.HandsAndOK;
            }
            else
            {
                ViewData["Buttons"] = ButtonOptions.OKEnabled;
            }
            Traveller traveller = new Traveller(sectionID, tableNumber, roundNumber, direction, boardNumber, pairNumber);

            string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
            ViewData["Title"] = $"Ranking List - {sectionLetter + tableNumber.ToString()} {direction}";
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {roundNumber} - Board {boardNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{traveller.PairNS}+{traveller.South}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{traveller.PairEW}+{traveller.West}")}";
                return View("Individual", traveller);
            }
            else 
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {roundNumber} - Board {boardNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {traveller.PairNS}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {traveller.PairEW}")}";
                return View("Pairs", traveller);
            }
        }
    }
}
