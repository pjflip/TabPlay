// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabPlay.Models;

namespace TabPlay.Controllers
{
    public class DirectionController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, string direction = "", bool confirm = false) 
        {
            Table table = AppData.TableList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (table == null)
            {
                table = new Table(sectionID, tableNumber);
                AppData.TableList.Add(table);
            }

            EnterDirection enterDirection = new EnterDirection
            {
                SectionID = sectionID,
                TableNumber = tableNumber,
                Direction = direction,
                PairNumber = table.PairNumber,
                Confirm = confirm
            };
            string sectionLetter = AppData.SectionList.Find(x => x.SectionID == sectionID).SectionLetter;
            ViewData["Header"] = $"Table {sectionLetter + tableNumber.ToString()} - Round {table.RoundNumber}";
            ViewData["Buttons"] = ButtonOptions.OKInvisible;
            ViewData["Title"] = $"Direction - {sectionLetter + tableNumber.ToString()}";
            return View(enterDirection);  
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, string direction, bool confirm)
        {
            Table table = AppData.TableList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            Device device = AppData.DeviceList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber && x.Direction == direction);
            int directionNumber = Utilities.DirectionToNumber(direction);

            int deviceNumber = 0;
            if (device != null && !confirm)
            {
                return RedirectToAction("Index", "Direction", new { sectionID, tableNumber, direction, confirm = true });
            }
            else if (device == null)
            {
                // Not on list, so need to add it
                device = new Device
                {
                    SectionID = sectionID,
                    TableNumber = tableNumber,
                    Direction = direction,
                    PairNumber = table.PairNumber[directionNumber],
                    RoundNumber = table.RoundNumber,
                    SectionTableString = AppData.SectionList.Find(x => x.SectionID == sectionID).SectionLetter + tableNumber.ToString()
                };
                AppData.DeviceList.Add(device);
                deviceNumber = AppData.DeviceList.LastIndexOf(device);
            }

            if (table.ReadyForNextRound[directionNumber])
            {
                return RedirectToAction("Index", "Move", new { deviceNumber });
            }
            else if (table.PlayComplete)
            {
                return RedirectToAction("Index", "RegisterPlayers", new { deviceNumber, boardNumber = table.BoardNumber + 1 });
            }
            else if (table.BiddingComplete)
            {
                return RedirectToAction("Index", "Playing", new { deviceNumber });
            }
            else if (table.BiddingStarted)
            {
                return RedirectToAction("Index", "Bidding", new { deviceNumber });
            }
            else
            {
                return RedirectToAction("Index", "RegisterPlayers", new { deviceNumber, boardNumber = table.BoardNumber });
            }
        }
    }
}