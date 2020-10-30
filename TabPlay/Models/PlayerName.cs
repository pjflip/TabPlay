// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class PlayerName
    {
        public string Name { get; private set; }

        public PlayerName(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber, int playerNumber)
        {
            Name = "";

            // First get the name from the name source
            string dir = direction.Substring(0, 1);    // Need just N, S, E or W

            if (playerNumber == 0)
            {
                Name = "Unknown";
            }
            else
            {
                switch (Settings.NameSource)
                {
                    case 0:
                        Name = AppData.GetNameFromPlayerNamesTable(playerNumber);
                        break;
                    case 1:
                        Name = Utilities.GetNameFromExternalDatabase(playerNumber);
                        break;
                    case 2:
                        Name = "";
                        break;
                    case 3:
                        Name = AppData.GetNameFromPlayerNamesTable(playerNumber);
                        if (Name == "" || Name.Substring(0, 1) == "#" || (Name.Length >= 7 && Name.Substring(0, 7) == "Unknown"))
                        {
                            Name = Utilities.GetNameFromExternalDatabase(playerNumber);
                        }
                        break;
                }
            }

            // Now update the PlayerNumbers table in the database
            // Numbers entered at the start (when round = 1) need to be set as round 0 in the database
            if (roundNumber == 1)
            {
                roundNumber = 0;
            }
            Name = Name.Replace("'", "''");    // Deal with apostrophes in names, eg O'Connor

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                object queryResult = null;

                // Check if PlayerNumbers entry exists already; if it does update it, if not create it
                string SQLString = $"SELECT [Number] FROM PlayerNumbers WHERE Section={sectionID} AND [Table]={tableNumber} AND ROUND={roundNumber} AND Direction='{dir}'";
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
                    SQLString = $"INSERT INTO PlayerNumbers (Section, [Table], Direction, [Number], Name, Round, Processed, TimeLog, TabPlayPairNo) VALUES ({sectionID}, {tableNumber}, '{dir}', '{playerNumber}', '{Name}', {roundNumber}, False, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, {pairNumber})";
                }
                else
                {
                    SQLString = $"UPDATE PlayerNumbers SET [Number]='{playerNumber}', [Name]='{Name}', Processed=False, TimeLog=#{DateTime.Now:yyyy-MM-dd hh:mm:ss}# WHERE Section={sectionID} AND [Table]={tableNumber} AND Round={roundNumber} AND Direction='{dir}'";
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
    }
}
