// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Bidding
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; private set; }
        public int RoundNumber { get; private set; }
        public string Direction { get; private set; }
        public int BoardNumber { get; private set; }
        public int PairNS { get; set; }   // Doubles as North player number for individuals
        public int PairEW { get; set; }   // Doubles as East player number for individuals
        public int HighBoard { get; set; }
        public int South { get; set; }
        public int West { get; set; }
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
        public int PollInterval { get; private set; }

        public Bidding(int sectionID, int tableNumber, int roundNumber, string direction, int boardNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            RoundNumber = roundNumber;
            BoardNumber = boardNumber;
            Direction = direction;
            NSVulnerable = Utilities.NSVulnerability[(boardNumber - 1) % 16];
            EWVulnerable = Utilities.EWVulnerability[(boardNumber - 1) % 16];
            BidTable = new string[7, 4];
            for (int i = 0; i < 7; i++)
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
            List<DatabaseBid> databaseBidList = new List<DatabaseBid>();

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                Utilities.CheckTabPlayPairNos(connection);
                string SQLString;
                OdbcCommand cmd = null;
                OdbcDataReader reader = null;
                if (AppData.IsIndividual)
                {
                    SQLString = $"SELECT NSPair, EWPair, South, West, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                PairNS = reader.GetInt32(0);
                                PairEW = reader.GetInt32(1);
                                South = reader.GetInt32(2);
                                West = reader.GetInt32(3);
                                HighBoard = reader.GetInt32(5);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                    if (direction == "North")
                    {
                        PairNumber = PairNS;
                    }
                    else if (direction == "East")
                    {
                        PairNumber = PairEW;
                    }
                    else if (direction == "South")
                    {
                        PairNumber = South;
                    }
                    else if (direction == "West")
                    {
                        PairNumber = West;
                    }
                    PlayerName = Utilities.GetNameFromPlayerNumbersTableIndividual(connection, sectionID, roundNumber, PairNumber);
                }
                else  // Not individual
                {
                    SQLString = $"SELECT NSPair, EWPair, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                PairNS = reader.GetInt32(0);
                                PairEW = reader.GetInt32(1);
                                HighBoard = reader.GetInt32(2);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                    if (direction == "North" || direction == "South")
                    {
                        PairNumber = PairNS;
                    }
                    else if (direction == "East" || direction == "West")
                    {
                        PairNumber = PairEW;
                    }
                    PlayerName = Utilities.GetNameFromPlayerNumbersTable(connection, sectionID, roundNumber, PairNumber, direction);
                }

                SQLString = $"SELECT Counter, Bid, Direction FROM BiddingData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber} AND Board={boardNumber}";
                cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader2 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader2 = cmd.ExecuteReader();
                        while (reader2.Read())
                        {
                            int tempCounter = reader2.GetInt32(0);
                            string tempBid = reader2.GetString(1);
                            string tempDirection = reader2.GetString(2);
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
                    reader2.Close();
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

            HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == SectionID && x.BoardNumber == boardNumber);
            if (handRecord == null)     // Can't find matching hand record, so use default SectionID = 1
            {
                handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == boardNumber);
            }
            CardString = handRecord.HandRow(direction);
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

            // Set TableStatus
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == SectionID && x.TableNumber == TableNumber);
            tableStatus.LastBid = new Bid(LastCallDirection, LastBidLevel, LastBidSuit, LastBidX, false, LastBidDirection, PassCount, BidCounter);
        }
    }
}
