// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class HandsDisplayController : Controller
    {
        public ActionResult Index(int deviceNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            Table table = AppData.TableList.Find(x => x.SectionID == device.SectionID && x.TableNumber == device.TableNumber);
            HandsDisplay handsDisplay = new HandsDisplay (deviceNumber, table);
            ViewData["Buttons"] = ButtonOptions.OKVisible;
            string sectionLetter = AppData.SectionList.Find(x => x.SectionID == device.SectionID).SectionLetter;
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {sectionLetter + device.TableNumber.ToString()} - Round {table.RoundNumber} - Board {table.BoardNumber}";
            }
            else
            {
                ViewData["Header"] = $"Table {sectionLetter + device.TableNumber.ToString()} - Round {table.RoundNumber} - Board {table.BoardNumber}";
            }
            ViewData["Title"] = $"Hands - {sectionLetter + device.TableNumber.ToString()} {device.Direction}";
            return View(handsDisplay);
        }
    }
}
