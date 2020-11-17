// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class DirectionController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber) 
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus == null)
            {
                tableStatus = new TableStatus(sectionID, tableNumber);
                AppData.TableStatusList.Add(tableStatus);
            }

            string sectionLetter = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter;
            ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {tableStatus.RoundNumber}";
            ViewData["Buttons"] = ButtonOptions.OKDisabled;
            ViewData["Title"] = $"Direction - {sectionLetter + tableNumber.ToString()}";
            return View(tableStatus);  
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, string direction)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.ReadyForNextRound[Utilities.DirectionToNumber(direction)])
            {
                return RedirectToAction("Index", "Move", new { sectionID, tableNumber, roundNumber = tableStatus.RoundNumber, direction, pairNumber = 0 });
            }
            else if (tableStatus.PlayComplete)
            {
                return RedirectToAction("Index", "RegisterPlayers", new { sectionID, tableNumber, roundNumber = tableStatus.RoundNumber, direction, boardNumber = tableStatus.BoardNumber + 1 });
            }
            else if (tableStatus.BiddingComplete)
            {
                return RedirectToAction("Index", "Playing", new { sectionID, tableNumber, direction });
            }
            else if (tableStatus.BiddingStarted)
            {
                return RedirectToAction("Index", "Bidding", new { sectionID, tableNumber, direction });
            }
            else
            {
                return RedirectToAction("Index", "RegisterPlayers", new { sectionID, tableNumber, roundNumber = tableStatus.RoundNumber, direction, boardNumber = tableStatus.BoardNumber });
            }
        }
    }
}