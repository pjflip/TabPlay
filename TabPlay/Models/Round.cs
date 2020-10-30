// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Round
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; set; }
        public int RoundNumber { get; private set; }
        public int BoardNumber { get; private set; }
        public int PairNS { get; set; }   // Doubles as North player number for individuals
        public int PairEW { get; set; }   // Doubles as East player number for individuals
        public int South { get; set; }
        public int West { get; set; }
        public int LowBoard { get; set; }
        public int HighBoard { get; set; }
        public string[] Direction { get; private set; }
        public int[] PairNumber { get; set; }
        public string[] PlayerName { get; private set; }
        public bool[] Registered { get; set; }
        public bool PlayerNumberEntry { get; private set; }
        public string PairOrPlayer { get; private set; }
        public int PollInterval { get; private set; }

        public Round(int sectionID, int tableNumber, int roundNumber, string direction, int boardNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            RoundNumber = roundNumber;
            BoardNumber = boardNumber;
            Direction = new string[4];
            PairNumber = new int[4];
            PlayerName = new string[4];
            Registered = new bool[4];
            PollInterval = Settings.PollInterval;

            // All directionNumbers are relative to the direction that is 0, so we need to know which directionNumber is North
            int northDirectionNumber = (4 - Utilities.DirectionToNumber(direction)) % 4;
            Direction[northDirectionNumber] = "North";
            Direction[(northDirectionNumber + 1) % 4] = "East";
            Direction[(northDirectionNumber + 2) % 4] = "South";
            Direction[(northDirectionNumber + 3) % 4] = "West";

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                Utilities.CheckTabPlayPairNos(connection);
                string SQLString;
                OdbcCommand cmd = null;
                OdbcDataReader reader = null;
                if (AppData.IsIndividual)
                {
                    PairOrPlayer = "Player";
                    SQLString = $"SELECT NSPair, EWPair, South, West, LowBoard, HighBoard FROM RoundData WHERE Section={SectionID} AND Table={TableNumber} AND Round={RoundNumber}";
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
                                LowBoard = reader.GetInt32(4);
                                HighBoard = reader.GetInt32(5);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                    PairNumber[northDirectionNumber] = PairNS;
                    PairNumber[(northDirectionNumber + 1) % 4] = PairEW;
                    PairNumber[(northDirectionNumber + 2) % 4] = South;
                    PairNumber[(northDirectionNumber + 3) % 4] = West;
                    for (int i = 0; i < 4; i++)
                    {
                        PlayerName[i] = Utilities.GetNameFromPlayerNumbersTableIndividual(connection, SectionID, RoundNumber, PairNumber[i]);
                    }

                }
                else  // Not individual
                {
                    PairOrPlayer = "Pair";
                    SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={SectionID} AND Table={TableNumber} AND Round={RoundNumber}";
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
                                LowBoard = reader.GetInt32(2);
                                HighBoard = reader.GetInt32(3);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                    PairNumber[northDirectionNumber] = PairNS;
                    PairNumber[(northDirectionNumber + 1) % 4] = PairEW;
                    PairNumber[(northDirectionNumber + 2) % 4] = PairNS;
                    PairNumber[(northDirectionNumber + 3) % 4] = PairEW;
                    for (int i = 0; i < 4; i++)
                    {
                        PlayerName[i] = Utilities.GetNameFromPlayerNumbersTable(connection, SectionID, RoundNumber, PairNumber[i], Direction[i]);
                    }
                }
                if (BoardNumber == 0) BoardNumber = LowBoard;

                // Update table status in the database (just once per table)
                if (direction == "North" || (PairNS == 0 && direction == "East"))
                {
                    SQLString = $"UPDATE Tables SET CurrentRound={RoundNumber}, CurrentBoard={BoardNumber}, BiddingStarted=False, BiddingComplete=False, PlayComplete=False WHERE Section={SectionID} AND [Table]={TableNumber}";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            cmd.ExecuteNonQuery();
                        });
                    }
                    finally
                    {
                        cmd.Dispose();
                    }
                }
            }
            PlayerNumberEntry = (RoundNumber == 1 || Settings.NumberEntryEachRound) && (BoardNumber == LowBoard);

            return;
        }
    }
}