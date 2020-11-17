// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class TravellerController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, string direction)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (!Settings.ShowResults)
            {
                return RedirectToAction("Index", "RegisterPlayers", new { sectionID, tableNumber, tableStatus.RoundNumber, direction, boardNumber = tableStatus.BoardNumber + 1 });
            }
            if (Settings.ShowHandRecord)
            {
                ViewData["Buttons"] = ButtonOptions.HandsAndOK;
            }
            else
            {
                ViewData["Buttons"] = ButtonOptions.OKEnabled;
            }
            Traveller traveller = new Traveller(tableStatus, direction);

            string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
            ViewData["Title"] = $"Traveller - {sectionLetter + tableNumber.ToString()} {direction}";
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {tableStatus.RoundNumber} - Board {tableStatus.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.BoardNumber, $"{tableStatus.PairNumber[0]}+{tableStatus.PairNumber[2]}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.BoardNumber, $"{tableStatus.PairNumber[1]}+{tableStatus.PairNumber[3]}")}";
                return View("Individual", traveller);
            }
            else 
            {
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {tableStatus.RoundNumber} - Board {tableStatus.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.BoardNumber, $"NS {tableStatus.PairNumber[0]}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.BoardNumber, $"EW {tableStatus.PairNumber[1]}")}";
                return View("Pairs", traveller);
            }
        }
    }
}
