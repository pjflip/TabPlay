// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class RankingListController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber)
        {
            // Show ranking list only from round 2 onwards, if applicable
            if (roundNumber > 1 && Settings.ShowRanking == 1)
            {
                RankingList rankingList = new RankingList(sectionID, tableNumber, roundNumber, direction, pairNumber);

                // Only show the ranking list if it contains something meaningful
                if (rankingList != null && rankingList.Count != 0 && rankingList[0].ScoreDecimal != 0 && rankingList[0].ScoreDecimal != 50)
                {
                    ViewData["Buttons"] = ButtonOptions.OKEnabled;
                    string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
                    ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} {direction} - Round {roundNumber}";
                    ViewData["Title"] = $"Ranking List - {sectionLetter + tableNumber.ToString()} {direction}";
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
            return RedirectToAction("Index", "Move", new { sectionID, tableNumber, roundNumber, direction, pairNumber});
        }

        public ActionResult Final(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber)
        {
            RankingList rankingList = new RankingList(sectionID, tableNumber, roundNumber, direction, pairNumber);

            // Don't show the ranking list if it doesn't contain anything useful
            if (rankingList == null || rankingList.Count == 0 || rankingList[0].ScoreDecimal == 0 || rankingList[0].ScoreDecimal == 50)
            {
                return RedirectToAction("Index", "EndScreen", new { sectionID, tableNumber, roundNumber, direction });
            }
            else
            {
                rankingList.FinalRankingList = true;
                ViewData["Buttons"] = ButtonOptions.OKEnabled;
                string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
                ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} {direction} - Round {roundNumber}";
                ViewData["Title"] = $"Ranking List - {sectionLetter + tableNumber.ToString()} {direction}";
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

        public JsonResult PollRanking(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            return Json(new RankingList(sectionID, tableNumber, roundNumber, direction, pairNumber), JsonRequestBehavior.AllowGet);
        }
    }
}