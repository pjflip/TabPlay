// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class EndScreenController : Controller
    {
        public ActionResult Index(int deviceNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            ViewData["Header"] = "";
            ViewData["Buttons"] = ButtonOptions.OKVisible;
            ViewData["Header"] = $"Table {AppData.SectionList.Find(x => x.SectionID == device.SectionID).SectionLetter + device.TableNumber.ToString()} {device.Direction}";
            ViewData["Title"] = $"End Screen - {AppData.SectionList.Find(x => x.SectionID == device.SectionID).SectionLetter + device.TableNumber.ToString()} {device.Direction}";
            ViewData["DeviceNumber"] = deviceNumber;
            return View();
        }

        public ActionResult OKButtonClick(int deviceNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            // Check if new round has been added; can't apply to individuals
            if (device.RoundNumber >= Utilities.NumberOfRoundsInEvent(device.SectionID))  
            {
                // Final round, so no new rounds added
                return RedirectToAction("Index", "EndScreen", new { deviceNumber });
            }
            else
            {
                return RedirectToAction("Index", "Move", new { deviceNumber });
            }
        }
    }
}