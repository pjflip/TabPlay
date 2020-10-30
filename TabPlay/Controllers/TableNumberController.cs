// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class TableNumberController : Controller
    {
        public ActionResult Index(int sectionID) 
        {
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            ViewData["Header"] = $"Section {section.SectionLetter}";
            ViewData["Buttons"] = ButtonOptions.OKDisabled;
            ViewData["Title"] = $"Table Number";
            return View(section);  
        }
    }
}