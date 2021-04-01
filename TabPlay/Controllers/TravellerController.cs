// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class TravellerController : Controller
    {
        public ActionResult Index(int deviceNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            if (!Settings.ShowResults)
            {
                return RedirectToAction("Index", "RegisterPlayers", new { deviceNumber, boardNumber = table.BoardNumber + 1 });
            }
            if (Settings.ShowHandRecord)
            {
                ViewData["Buttons"] = ButtonOptions.HandsAndOK;
            }
            else
            {
                ViewData["Buttons"] = ButtonOptions.OKEnabled;
            }
            Traveller traveller = new Traveller(deviceNumber, table);

            ViewData["Title"] = $"Traveller - {device.SectionTableString}:{device.Direction}";
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {device.SectionTableString} - Round {table.RoundNumber} - Board {table.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", table.BoardNumber, $"{table.PairNumber[0]}+{table.PairNumber[2]}")} v {Utilities.ColourPairByVulnerability("EW", table.BoardNumber, $"{table.PairNumber[1]}+{table.PairNumber[3]}")}";
                return View("Individual", traveller);
            }
            else 
            {
                ViewData["Header"] = $"Table {device.SectionTableString} - Round {table.RoundNumber} - Board {table.BoardNumber} - {Utilities.ColourPairByVulnerability("NS", table.BoardNumber, $"NS {table.PairNumber[0]}")} v {Utilities.ColourPairByVulnerability("EW", table.BoardNumber, $"EW {table.PairNumber[1]}")}";
                return View("Pairs", traveller);
            }
        }
    }
}
