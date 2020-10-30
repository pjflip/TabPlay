// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class FinalRankingListController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber)
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
                Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
                ViewData["Header"] = $"Table {section.SectionLetter + tableNumber.ToString()} {direction} - Round {roundNumber}";
                ViewData["Title"] = $"Ranking List - {section.SectionLetter + tableNumber.ToString()} {direction}";
                if (rankingList.TwoWinners)
                {
                    return View("TwoWinnersRankingList", rankingList);
                }
                else
                {
                    return View("OneWinnerRankingList", rankingList);
                }
            }
        }
    }
}