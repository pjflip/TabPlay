// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Bidding
    {
        public int DeviceNumber { get; private set; }
        public string Direction { get; private set; }
        public int BoardNumber { get; private set; }
        public int PairNumber { get; private set; }
        public string PlayerName { get; private set; }
        public string[] CardString { get; private set; }
        public string[] DisplayRank { get; private set; }
        public string[] DisplaySuit { get; private set; }
        public string Dealer { get; private set; }
        public bool NSVulnerable { get; private set; }
        public bool EWVulnerable { get; private set; }
        public string[] BidDirections { get; private set; }
        public string[,] BidTable { get; private set; }
        public string LastCallDirection { get; private set; }
        public int LastBidLevel { get; private set; }
        public string LastBidSuit { get; private set; }
        public string LastBidX { get; private set; }
        public string LastBidDirection { get; private set; }
        public int PassCount { get; private set; }
        public int BidCounter { get; private set; }
        public string ToBidDirection { get; private set; }
        public string PairOrPlayer { get; private set; }
        public int PollInterval { get; private set; }

        public Bidding(int deviceNumber, Table table)
        {
            DeviceNumber = deviceNumber;
            Device device = AppData.DeviceList[deviceNumber];
            BoardNumber = table.BoardNumber;
            Direction = device.Direction;
            int directionNumber = Utilities.DirectionToNumber(Direction);
            PairNumber = table.PairNumber[directionNumber];
            PlayerName = table.PlayerName[directionNumber];
            NSVulnerable = Utilities.NSVulnerability[(BoardNumber - 1) % 16];
            EWVulnerable = Utilities.EWVulnerability[(BoardNumber - 1) % 16];
            BidTable = new string[10, 4];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    BidTable[i, j] = "";
                }
            }
            LastCallDirection = "";
            LastBidLevel = 0;
            LastBidSuit = "";
            LastBidX = "";
            LastBidDirection = "";
            PassCount = 0;
            BidCounter = -1;
            ToBidDirection = "";
            PollInterval = Settings.PollInterval;
            if (AppData.IsIndividual) PairOrPlayer = "Player"; else PairOrPlayer = "Pair";
            List<DatabaseBid> databaseBidList = new List<DatabaseBid>();

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                Utilities.CheckTabPlayPairNos(connection);
                string SQLString = $"SELECT Counter, Bid, Direction FROM BiddingData WHERE Section={device.SectionID} AND Table={device.TableNumber} AND Round={device.RoundNumber} AND Board={BoardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int tempCounter = reader.GetInt32(0);
                            string tempBid = reader.GetString(1);
                            string tempDirection = reader.GetString(2);
                            DatabaseBid databaseBid = new DatabaseBid
                            {
                                Counter = tempCounter,
                                Bid = tempBid,
                                Direction = tempDirection
                            };
                            databaseBidList.Add(databaseBid);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }

            databaseBidList.Sort((x, y) => x.Counter.CompareTo(y.Counter));
            foreach (DatabaseBid databaseBid in databaseBidList)
            {
                BidCounter = databaseBid.Counter;
                if (databaseBid.Direction == "N")
                {
                    LastCallDirection = "North";
                    ToBidDirection = "East";
                }
                else if (databaseBid.Direction == "E")
                {
                    LastCallDirection = "East";
                    ToBidDirection = "South";
                }
                else if (databaseBid.Direction == "S")
                {
                    LastCallDirection = "South";
                    ToBidDirection = "West";
                }
                else if (databaseBid.Direction == "W")
                {
                    LastCallDirection = "West";
                    ToBidDirection = "North";
                }
                if (databaseBid.Bid == "PASS")
                {
                    PassCount++;
                    BidTable[BidCounter / 4, BidCounter % 4] = "<span style='color:darkgreen'>Pass</span>";
                }
                else if (databaseBid.Bid == "x")
                {
                    PassCount = 0;
                    BidTable[BidCounter / 4, BidCounter % 4] = "<span style='color:darkred'>X</span>";
                    LastBidX = "x";
                    LastBidDirection = LastCallDirection;
                }
                else if (databaseBid.Bid == "xx")
                {
                    PassCount = 0;
                    BidTable[BidCounter / 4, BidCounter % 4] = "<span style='color:darkblue'>XX</span>";
                    LastBidX = "xx";
                    LastBidDirection = LastCallDirection;
                }
                else
                {
                    LastBidLevel = Convert.ToInt32(databaseBid.Bid.Substring(0, 1));
                    LastBidX = "";
                    LastBidDirection = LastCallDirection;
                    LastBidSuit = databaseBid.Bid.Substring(1);
                    if (LastBidSuit == "S")
                    {
                        BidTable[BidCounter / 4, BidCounter % 4] = LastBidLevel.ToString() + "<span style='color:black'>&spades;</span>";
                    }
                    else if (LastBidSuit == "H")
                    {
                        BidTable[BidCounter / 4, BidCounter % 4] = LastBidLevel.ToString() + "<span style='color:red'>&hearts;</span>";
                    }
                    else if (LastBidSuit == "D")
                    {
                        BidTable[BidCounter / 4, BidCounter % 4] = LastBidLevel.ToString() + "<span style='color:orangered'>&diams;</span>";
                    }
                    else if (LastBidSuit == "C")
                    {
                        BidTable[BidCounter / 4, BidCounter % 4] = LastBidLevel.ToString() + "<span style='color:darkblue'>&clubs;</span>";
                    }
                    else
                    {
                        BidTable[BidCounter / 4, BidCounter % 4] = LastBidLevel.ToString() + "NT";
                    }
                }
            }

            HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == device.SectionID && x.BoardNumber == BoardNumber);
            if (handRecord == null)     // Can't find matching hand record, so use default SectionID = 1
            {
                handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == BoardNumber);
            }
            CardString = handRecord.HandRow(device.Direction);
            DisplayRank = new string[13];
            DisplaySuit = new string[13];
            for (int i = 0; i < 13; i++)
            {
                DisplayRank[i] = Utilities.DisplayRank(CardString[i]);
                DisplaySuit[i] = Utilities.DisplaySuit(CardString[i]);
            }
            Dealer = handRecord.Dealer;

            BidDirections = new string[4];
            int northDirectionNumber = (4 - Utilities.DirectionToNumber(Dealer)) % 4;
            BidDirections[northDirectionNumber] = "North";
            BidDirections[(northDirectionNumber + 1) % 4] = "East";
            BidDirections[(northDirectionNumber + 2) % 4] = "South";
            BidDirections[(northDirectionNumber + 3) % 4] = "West";

            if (ToBidDirection == "") ToBidDirection = Dealer;

            // Set table info
            table.LastBid = new Bid(LastCallDirection, LastBidLevel, LastBidSuit, LastBidX, false, LastBidDirection, PassCount, BidCounter);
        }
    }
}
