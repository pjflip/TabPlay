// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class RankingListController : Controller
    {
        public ActionResult Index(int deviceNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            // Show ranking list only from round 2 onwards, if applicable
            if (device.RoundNumber > 1 && Settings.ShowRanking == 1)
            {
                RankingList rankingList = new RankingList(deviceNumber);

                // Only show the ranking list if it contains something meaningful
                if (rankingList != null && rankingList.Count != 0 && rankingList[0].ScoreDecimal != 0 && rankingList[0].ScoreDecimal != 50)
                {
                    ViewData["Buttons"] = ButtonOptions.OKEnabled;
                    ViewData["Header"] = $"Table {device.SectionTableString}:{device.Direction} - Round {device.RoundNumber}";
                    ViewData["Title"] = $"Ranking List - {device.SectionTableString}:{device.Direction}";
                    if (rankingList.TwoWinners)
                    {
                        return View("TwoWinners", rankingList);
                    }
                    else
                    {
                        return View("OneWinner", rankingList);
                    }
                }
            }
            return RedirectToAction("Index", "Move", new { deviceNumber });
        }

        public ActionResult Final(int deviceNumber)
        {
            RankingList rankingList = new RankingList(deviceNumber);

            // Don't show the ranking list if it doesn't contain anything useful
            if (rankingList == null || rankingList.Count == 0 || rankingList[0].ScoreDecimal == 0 || rankingList[0].ScoreDecimal == 50)
            {
                return RedirectToAction("Index", "EndScreen", new { deviceNumber });
            }
            else
            {
                Device device = AppData.DeviceList[deviceNumber];
                rankingList.FinalRankingList = true;
                ViewData["Buttons"] = ButtonOptions.OKEnabled;
                ViewData["Header"] = $"Table {device.SectionTableString}:{device.Direction} - Round {device.RoundNumber}";
                ViewData["Title"] = $"Ranking List - {device.SectionTableString}:{device.Direction}";
                if (rankingList.TwoWinners)
                {
                    return View("TwoWinners", rankingList);
                }
                else
                {
                    return View("OneWinner", rankingList);
                }
            }
        }

        public JsonResult PollRanking(int deviceNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            return Json(new RankingList(deviceNumber), JsonRequestBehavior.AllowGet);
        }
    }
}