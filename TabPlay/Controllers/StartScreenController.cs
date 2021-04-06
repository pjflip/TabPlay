// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Reflection;
using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class StartScreenController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Header"] = "";
            ViewData["Buttons"] = ButtonOptions.OKVisible;
            ViewData["Version"] = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ViewData["Title"] = $"Start Screen";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            AppData.Refresh();
            if (AppData.DBConnectionString == "")
            {
                TempData["WarningMessage"] = "Scoring database not yet selected";
                return RedirectToAction("Index", "StartScreen");
            }
            Settings.Refresh();
            HandRecords.Set();

            return RedirectToAction("Index", "Section");
        }
    }
}