// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public enum ButtonOptions
    {
        None,
        OKEnabled,
        OKDisabled,
        Skip,
        HandsAndOK,
        Claim
    }

    public static class Utilities
    {
        public static readonly string[] Directions = { "North", "East", "South", "West" };

        public static int DirectionToNumber(string direction)
        {
            if (direction == "North") return 0;
            else if (direction == "East") return 1;
            else if (direction == "South") return 2;
            else return 3;
        }

        // Find out how many rounds there are in the event
        // Need to re-query database in case rounds are added/removed by scoring program
        public static int NumberOfRoundsInEvent(int sectionID)
        {
            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = SQLString = $"SELECT MAX(Round) FROM RoundData WHERE Section={sectionID}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return Convert.ToInt32(queryResult);
        }

        // Get the dealer based on board number for standard boards
        public static string GetDealerForBoard(int boardNumber)
        {
            switch ((boardNumber - 1) % 4)
            {
                case 0:
                    return "North";
                case 1:
                    return "East";
                case 2:
                    return "South";
                case 3:
                    return "West";
                default:
                    return "";
            }
        }

        // Used for setting vulnerability by board number
        public static readonly bool[] NSVulnerability = { false, true, false, true, true, false, true, false, false, true, false, true, true, false, true, false };
        public static readonly bool[] EWVulnerability = { false, false, true, true, false, true, true, false, true, true, false, false, true, false, false, true };

        // Apply html styles to colour the pair number based on vulnerability
        public static string ColourPairByVulnerability(string dir, int boardNo, string pair)
        {
            string PairString;
            if (dir == "NS")
            {
                if (NSVulnerability[(boardNo - 1) % 16])
                {
                    PairString = $"<a style=\"color:red\">{pair}</a>";
                }
                else
                {
                    PairString = $"<a style=\"color:green\">{pair}</a>";
                }
            }
            else
            {
                if (EWVulnerability[(boardNo - 1) % 16])
                {
                    PairString = $"<a style=\"color:red\">{pair}</a>";
                }
                else
                {
                    PairString = $"<a style=\"color:green\">{pair}</a>";
                }
            }
            return PairString;
        }

        public static void CheckTabPlayPairNos(OdbcConnection conn)
        {
            object queryResult = null;

            // Check to see if TabPlayPairNo exists (it may get overwritten if the scoring program recreates the PlayerNumbers table)
            string SQLString = $"SELECT 1 FROM PlayerNumbers WHERE TabPlayPairNo IS NULL";
            OdbcCommand cmd1 = new OdbcCommand(SQLString, conn);
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    queryResult = cmd1.ExecuteScalar();
                });
            }
            finally
            {
                cmd1.Dispose();
            }

            if (queryResult != null)
            {
                // TabPlayPairNo doesn't exist, so recreate it.  This duplicates the code in TabPlayStarter
                SQLString = "SELECT Section, [Table], Direction FROM PlayerNumbers";
                OdbcCommand cmd2 = new OdbcCommand(SQLString, conn);
                OdbcDataReader reader2 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader2 = cmd2.ExecuteReader();
                        while (reader2.Read())
                        {
                            int tempSectionID = reader2.GetInt32(0);
                            int tempTable = reader2.GetInt32(1);
                            string tempDirection = reader2.GetString(2);
                            if (AppData.IsIndividual)
                            {
                                switch (tempDirection)
                                {
                                    case "N":
                                        SQLString = $"SELECT NSPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                    case "S":
                                        SQLString = $"SELECT South FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                    case "E":
                                        SQLString = $"SELECT EWPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                    case "W":
                                        SQLString = $"SELECT West FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                }
                            }
                            else
                            {
                                switch (tempDirection)
                                {
                                    case "N":
                                    case "S":
                                        SQLString = $"SELECT NSPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                    case "E":
                                    case "W":
                                        SQLString = $"SELECT EWPair FROM RoundData WHERE Section={tempSectionID} AND [Table]={tempTable} AND ROUND=1";
                                        break;
                                }
                            }
                            OdbcCommand cmd3 = new OdbcCommand(SQLString, conn);
                            try
                            {
                                ODBCRetryHelper.ODBCRetry(() =>
                                {
                                    queryResult = cmd3.ExecuteScalar();
                                });
                            }
                            finally
                            {
                                cmd3.Dispose();
                            }
                            string TSpairNo = queryResult.ToString();
                            SQLString = $"UPDATE PlayerNumbers SET TabPlayPairNo={TSpairNo} WHERE Section={tempSectionID} AND [Table]={tempTable} AND Direction='{tempDirection}'";
                            OdbcCommand cmd4 = new OdbcCommand(SQLString, conn);
                            try
                            {
                                ODBCRetryHelper.ODBCRetry(() =>
                                {
                                    cmd4.ExecuteNonQuery();
                                });
                            }
                            finally
                            {
                                cmd4.Dispose();
                            }
                        }
                    });
                }
                finally
                {
                    reader2.Close();
                    cmd2.Dispose();
                }
            }
        }

        public static string GetNameFromPlayerNumbersTable(OdbcConnection conn, int sectionID, int round, int pairNo, string direction)
        {
            string number = "###";
            string name = "";
            string dir = direction.Substring(0, 1);

            // First look for entries in the same direction
            // If the player has changed (eg in teams), there will be more than one PlayerNumbers record for this pair number and direction
            // We need the most recently added name applicable to this round
            string SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={sectionID} AND TabPlayPairNo={pairNo} AND Direction='{dir}'";
            OdbcCommand cmd1 = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader1 = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader1 = cmd1.ExecuteReader();
                    DateTime latestTimeLog = new DateTime(2010, 1, 1);
                    while (reader1.Read())
                    {
                        try
                        {
                            int readerRound = reader1.GetInt32(2);
                            DateTime timeLog;
                            if (reader1.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else
                            {
                                timeLog = reader1.GetDateTime(3);
                            }
                            if (readerRound <= round && timeLog >= latestTimeLog)
                            {
                                number = reader1.GetString(0);
                                name = reader1.GetString(1);
                                latestTimeLog = timeLog;
                            }
                        }
                        catch   // Record found, but format cannot be parsed
                        {
                            if (number == "###") number = "";
                        }
                    }
                });
            }
            finally
            {
                reader1.Close();
                cmd1.Dispose();
            }

            if (number == "###")  // Nothing found so try Round 0 entries in the other direction (for Howell type pairs movement)
            {
                string otherDir;
                switch (dir)
                {
                    case "N":
                        otherDir = "E";
                        break;
                    case "S":
                        otherDir = "W";
                        break;
                    case "E":
                        otherDir = "N";
                        break;
                    case "W":
                        otherDir = "S";
                        break;
                    default:
                        otherDir = "";
                        break;
                }
                SQLString = $"SELECT Number, Name, TimeLog FROM PlayerNumbers WHERE Section={sectionID} AND TabPlayPairNo={pairNo} AND Direction='{otherDir}' AND Round=0";
                OdbcCommand cmd2 = new OdbcCommand(SQLString, conn);
                OdbcDataReader reader2 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader2 = cmd2.ExecuteReader();
                        DateTime latestTimeLog = new DateTime(2010, 1, 1);
                        while (reader2.Read())
                        {
                            try
                            {
                                DateTime timeLog;
                                if (reader2.IsDBNull(2))
                                {
                                    timeLog = new DateTime(2010, 1, 1);
                                }
                                else
                                {
                                    timeLog = reader2.GetDateTime(2);
                                }
                                if (timeLog >= latestTimeLog)
                                {
                                    number = reader2.GetString(0);
                                    name = reader2.GetString(1);
                                    latestTimeLog = timeLog;
                                }
                            }
                            catch   // Record found, but format cannot be parsed
                            {
                                if (number == "###") number = "";
                            }
                        }
                    });
                }
                finally
                {
                    reader2.Close();
                    cmd2.Dispose();
                }
            }

            if (number == "###")  // Nothing found in either direction!!
            {
                number = "";
            }
            return FormatName(name, number);
        }

        public static string GetNameFromPlayerNumbersTableIndividual(OdbcConnection conn, int sectionID, int round, int playerNo)
        {
            string number = "###";
            string name = "";

            string SQLString = $"SELECT Number, Name, Round, TimeLog FROM PlayerNumbers WHERE Section={sectionID} AND TabPlayPairNo={playerNo}";
            OdbcCommand cmd = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader = cmd.ExecuteReader();
                    DateTime latestTimeLog = new DateTime(2010, 1, 1);
                    while (reader.Read())
                    {
                        try
                        {
                            int readerRound = reader.GetInt32(2);
                            DateTime timeLog;
                            if (reader.IsDBNull(3))
                            {
                                timeLog = new DateTime(2010, 1, 1);
                            }
                            else
                            {
                                timeLog = reader.GetDateTime(3);
                            }
                            if (readerRound <= round && timeLog >= latestTimeLog)
                            {
                                number = reader.GetString(0);
                                name = reader.GetString(1);
                                latestTimeLog = timeLog;
                            }
                        }
                        catch  // Record found, but format cannot be parsed
                        {
                            if (number == "###") number = "";
                        }
                    }
                });
            }
            finally
            {
                cmd.Dispose();
                reader.Close();
            }

            if (number == "###")  // Nothing found
            {
                number = "";
            }

            return FormatName(name, number);
        }

        // Function to deal with different display format options for blank and unknown names
        private static string FormatName(string name, string number)
        {
            if (name == "" || name == "Unknown")
            {
                if (number == "")
                {
                    return "";
                }
                else if (number == "0")
                {
                    return "Unknown";
                }
                else
                {
                    return "Unknown #" + number;
                }
            }
            else
            {
                return name;
            }
        }

        public static string GetNameFromExternalDatabase(int playerNumber)
        {
            string name = "";
            OdbcConnectionStringBuilder externalDB = new OdbcConnectionStringBuilder { Driver = "Microsoft Access Driver (*.mdb)" };
            externalDB.Add("Dbq", @"C:\Bridgemate\BMPlayerDB.mdb");
            externalDB.Add("Uid", "Admin");
            using (OdbcConnection connection = new OdbcConnection(externalDB.ToString()))
            {
                object queryResult = null;
                string SQLString = $"SELECT Name FROM PlayerNameDatabase WHERE ID={playerNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                        if (queryResult == null)
                        {
                            name = "Unknown #" + playerNumber;
                        }
                        else
                        {
                            name = queryResult.ToString();
                        }
                    });
                }
                catch (OdbcException)  // If we can't read the external database for whatever reason...
                {
                    name = "#" + playerNumber;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return name;
        }

        public static string Suit(string cardString)
        {
            if (cardString == "") return "";
            return cardString.Substring(1, 1);
        }

        public static int Rank(string cardString)
        {
            if (cardString == "") return 0;
            string rankString = cardString.Substring(0, 1);
            if (rankString == "A")
            {
                return 14;
            }
            else if (rankString == "K")
            {
                return 13;
            }
            else if (rankString == "Q")
            {
                return 12;
            }
            else if (rankString == "J")
            {
                return 11;
            }
            else if (rankString == "T")
            {
                return 10;
            }
            else
            {
                return Convert.ToInt32(rankString);
            }
        }

        public static string DisplaySuit(string cardString)
        {
            switch (cardString.Substring(1, 1))
            {
                case "S":
                    return "<span style=\"color:black\">&spades;</span>";
                case "H":
                    return "<span style=\"color:red\">&hearts;</span>";
                case "D":
                    return "<span style=\"color:orangered\">&diams;</span>";
                case "C":
                    return "<span style=\"color:darkblue\">&clubs;</span>";
                default:
                    return "";
            }
        }

        public static string DisplayRank(string cardString)
        {
            switch (cardString.Substring(1, 1))
            {
                case "S":
                    return $"<span style=\"color:black\">{cardString.Substring(0, 1)}</span>";
                case "H":
                    return $"<span style=\"color:red\">{cardString.Substring(0, 1)}</span>";
                case "D":
                    return $"<span style=\"color:orangered\">{cardString.Substring(0, 1)}</span>";
                case "C":
                    return $"<span style=\"color:darkblue\">{cardString.Substring(0, 1)}</span>";
                default:
                    return "";
            }
        }

        public static string DisplayContract(int contractLevel, string contractSuit, string contractX)
        {
            if (contractLevel <= 0) return "<span style=\"color:darkgreen\">PASS</span>";
            string s = contractLevel.ToString();
            switch (contractSuit)
            {
                case "NT":
                    s += "NT";
                    break;
                case "S":
                    s += "<span style=\"color:black\">&spades;</span>";
                    break;
                case "H":
                    s += "<span style=\"color:red\">&hearts;</span>";
                    break;
                case "D":
                    s += "<span style=\"color:orangered\">&diams;</span>";
                    break;
                case "C":
                    s += "<span style=\"color:darkblue\">&clubs;</span>";
                    break;
            }
            s += contractX.ToUpper();
            return s;
        }

        public static string DisplayBid(int passCount, string bidX, string bidSuit, int bidLevel)
        {
            if (passCount > 0)
            {
                return "<span style='color:darkgreen'>Pass</span>";
            }
            else if (bidX == "x")
            {
                return "<span style='color:darkred'>X</span>";
            }
            else if (bidX == "xx")
            {
                return "<span style='color:darkblue'>XX</span>";
            }
            else if (bidSuit == "S")
            {
                return bidLevel.ToString() + "<span style='color:black'>&spades;</span>";
            }
            else if (bidSuit == "H")
            {
                return bidLevel.ToString() + "<span style='color:red'>&hearts;</span>";
            }
            else if (bidSuit == "D")
            {
                return bidLevel.ToString() + "<span style='color:orangered'>&diams;</span>";
            }
            else if (bidSuit == "C")
            {
                return bidLevel.ToString() + "<span style='color:darkblue'>&clubs;</span>";
            }
            else
            {
                return bidLevel.ToString() + "NT";
            }
        }
    }
}
