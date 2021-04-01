// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class ErrorScreenController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Header"] = "";
            ViewData["Buttons"] = ButtonOptions.OKEnabled;
            ViewData["Title"] = "Error Screen";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            AppData.Refresh();
            if (AppData.DBConnectionString == "")
            {
                TempData["WarningMessage"] = "Scoring database not yet started";
                return RedirectToAction("Index", "StartScreen");
            }
            else
            {
                Settings.Refresh();
                return RedirectToAction("Index", "Section");
            }
        }
    }
}