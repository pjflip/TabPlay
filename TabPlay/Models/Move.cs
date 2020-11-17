// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Move
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; private set; }
        public int RoundNumber { get; private set; }
        public string Direction { get; private set; }
        public int PairNumber { get; private set; }
        public int NewRoundNumber { get; private set; }
        public int NewTableNumber { get; private set; }
        public string NewDirection { get; private set; }
        public bool Stay { get; private set; }
        public int LowBoard { get; private set; } = 0;
        public int HighBoard { get; private set; }
        public int BoardsNewTableNumber { get; private set; }

        public Move(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            RoundNumber = roundNumber;
            Direction = direction;
            NewRoundNumber = roundNumber + 1;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT LowBoard, HighBoard FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            LowBoard = reader.GetInt32(0);
                            HighBoard = reader.GetInt32(1);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }

                if (pairNumber != 0)
                {
                    PairNumber = pairNumber;
                }
                else
                {
                    if (AppData.IsIndividual)
                    {
                        SQLString = $"SELECT NSPair, EWPair, South, West FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            ODBCRetryHelper.ODBCRetry(() =>
                            {
                                reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    if (direction == "North")
                                    {
                                        PairNumber = reader.GetInt32(0);
                                    }
                                    else if (direction == "East")
                                    {
                                        PairNumber = reader.GetInt32(1);
                                    }
                                    else if (direction == "South")
                                    {
                                        PairNumber = reader.GetInt32(2);
                                    }
                                    else if (direction == "West")
                                    {
                                        PairNumber = reader.GetInt32(3);
                                    }
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
                        SQLString = $"SELECT NSPair, EWPair FROM RoundData WHERE Section={sectionID} AND Table={tableNumber} AND Round={roundNumber}";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            ODBCRetryHelper.ODBCRetry(() =>
                            {
                                reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    if (direction == "North" || direction == "South")
                                    {
                                        PairNumber = reader.GetInt32(0);
                                    }
                                    else if (direction == "East" || direction == "West")
                                    {
                                        PairNumber = reader.GetInt32(1);
                                    }
                                }
                            });
                        }
                        finally
                        {
                            reader.Close();
                            cmd.Dispose();
                        }
                    }
                }

                MoveOptionsList moveOptionsList = new MoveOptionsList(connection, SectionID, NewRoundNumber);
                PlayerMove playerMove = moveOptionsList.GetMove(tableNumber, PairNumber, direction);
                NewTableNumber = playerMove.NewTableNumber;
                NewDirection = playerMove.NewDirection;
                Stay = playerMove.Stay;
                BoardsNewTableNumber = moveOptionsList.GetBoardsNewTableNumber(tableNumber, LowBoard);
            }
        }
    }
}