// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Move
    {
        public int SectionID { get; private set; }
        public int NewRoundNumber { get; private set; }
        public int NewTableNumber { get; set; }
        public string NewDirection { get; set; }
        public bool Stay { get; set; }
        public int LowBoard { get; private set; }
        public int HighBoard { get; private set; }
        public int BoardsNewTableNumber { get; private set; }

        private int pairNumber;

        public Move(int sectionID, int tableNumber, int roundNumber, string direction)
        {
            SectionID = sectionID;
            NewRoundNumber = roundNumber + 1;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                Utilities.CheckTabPlayPairNos(connection);
                if (AppData.IsIndividual)
                {
                    string SQLString = $"SELECT NSPair, EWPair, South, West, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                if (direction == "North")
                                {
                                    pairNumber = reader.GetInt32(0);
                                }
                                else if (direction == "East")
                                {
                                    pairNumber = reader.GetInt32(1);
                                }
                                else if (direction == "South")
                                {
                                    pairNumber = reader.GetInt32(2);
                                }
                                else if (direction == "West")
                                {
                                    pairNumber = reader.GetInt32(3);
                                }
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
                }
                else  // Not individual
                {
                    string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                if (direction == "North" || direction == "South")
                                {
                                    pairNumber = reader.GetInt32(0);
                                }
                                else if (direction == "East" || direction == "West")
                                {
                                    pairNumber = reader.GetInt32(1);
                                }
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
                }

                MoveOptionsList roundsList = new MoveOptionsList(connection, SectionID, NewRoundNumber);
                PlayerMove playerMove = roundsList.GetMove(tableNumber, pairNumber, direction);
                NewTableNumber = playerMove.NewTableNumber;
                NewDirection = playerMove.NewDirection;
                Stay = playerMove.Stay;
                BoardsNewTableNumber = roundsList.GetBoardsNewTableNumber(tableNumber, LowBoard);
            }
        }
    }
}