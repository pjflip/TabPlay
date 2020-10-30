// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class SectionController : Controller
    {
        public ActionResult Index()
        {
            // Check if only one section - if so use it
            if (AppData.SectionsList.Count == 1)
            {
                return RedirectToAction("Index", "TableNumber", new { sectionID = AppData.SectionsList[0].SectionID });
            }
            else
            // Get section
            {
                ViewData["Header"] = "";
                ViewData["Buttons"] = ButtonOptions.OKDisabled;
                ViewData["Title"] = $"Section";
                return View(AppData.SectionsList);
            }
        }
    }
}