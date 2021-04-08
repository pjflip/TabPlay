// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Device
    {
        public int SectionID { get; set; }
        public int TableNumber { get; set; }
        public string Direction { get; set; }
        public int PairNumber { get; set; }
        public string PlayerName { get; set; }
        public int RoundNumber { get; set; }
        public string SectionTableString { get; set; }

        public void UpdatePlayerName(int playerNumber)
        {
            PlayerName = "";

            // First get the name from the name source
            string dir = Direction.Substring(0, 1);    // Need just N, S, E or W

            if (playerNumber == 0)
            {
                PlayerName = "Unknown";
            }
            else
            {
                switch (Settings.NameSource)
                {
                    case 0:
                        PlayerName = AppData.GetNameFromPlayerNamesTable(playerNumber);
                        break;
                    case 1:
                        PlayerName = Utilities.GetNameFromExternalDatabase(playerNumber);
                        break;
                    case 2:
                        PlayerName = "";
                        break;
                    case 3:
                        PlayerName = AppData.GetNameFromPlayerNamesTable(playerNumber);
                        if (PlayerName == "" || PlayerName.Substring(0, 1) == "#" || (PlayerName.Length >= 7 && PlayerName.Substring(0, 7) == "Unknown"))
                        {
                            PlayerName = Utilities.GetNameFromExternalDatabase(playerNumber);
                        }
                        break;
                }
            }

            // Now update the PlayerNumbers table in the database
            // Numbers entered at the start (when round = 1) need to be set as round 0 in the database
            int roundNumber = RoundNumber;
            if (roundNumber == 1)
            {
                roundNumber = 0;
            }
            PlayerName = PlayerName.Replace("'", "''");    // Deal with apostrophes in names, eg O'Connor

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                object queryResult = null;

                // Check if PlayerNumbers entry exists already; if it does update it, if not create it
                string SQLString = $"SELECT [Number] FROM PlayerNumbers WHERE Section={SectionID} AND [Table]={TableNumber} AND ROUND={roundNumber} AND Direction='{dir}'";
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
                if (queryResult == DBNull.Value || queryResult == null)
                {
                    SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Name, Round, Processed, TimeLog, TabPlayPairNo) VALUES ({SectionID}, {TableNumber}, '{dir}', '{playerNumber}', '{PlayerName}', {roundNumber}, False, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, {PairNumber})";
                }
                else
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', [Name]='{PlayerName}', Processed=False, TimeLog=#{DateTime.Now:yyyy-MM-dd hh:mm:ss}# WHERE Section={SectionID} AND [Table]={TableNumber} AND Round={roundNumber} AND Direction='{dir}'";
                }
                OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd2.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd2.Dispose();
                }
            }
            return;
        }

        public void UpdateMove(int newRoundNumber, int newTableNumber, string newDirection)
        {
            RoundNumber = newRoundNumber;
            TableNumber = newTableNumber;
            Direction = newDirection;
            if (newTableNumber == 0)
            {
                SectionTableString = AppData.SectionList.Find(x => x.SectionID == SectionID).SectionLetter + ":Sitout";
            }
            else
            {
                SectionTableString = AppData.SectionList.Find(x => x.SectionID == SectionID).SectionLetter + newTableNumber.ToString();
            }
        }
    }
}
