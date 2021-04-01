// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;

namespace TabPlay.Models
    {
    // AppData is a global class that applies accross all sessions
    // It includes a list of sections and a copy of the PlayerNames table (if it exists) 
    // and the status for each table
    public static class AppData
    {
        public static string DBConnectionString { get; private set; }
        public static bool IsIndividual { get; private set; }
        public static List<Section> SectionList = new List<Section>();
        public static List<Table> TableList = new List<Table>();
        public static List<Device> DeviceList = new List<Device>();

        private class PlayerRecord
        {
            public string Name;
            public int Number;
        }
        private static readonly List<PlayerRecord> PlayerNamesTable = new List<PlayerRecord>();
        private static readonly string PathToTabPlayDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabPlay\TabPlayDB.txt");
        private static DateTime TabPlayDBTime;

        public static void Refresh()
        {
            if (File.Exists(PathToTabPlayDB))
            {
                // Only do an update if TabPlayStarter has updated TabPlayDB.txt
                DateTime lastWriteTime = File.GetLastWriteTime(PathToTabPlayDB);
                if (lastWriteTime > TabPlayDBTime)
                {
                    DBConnectionString = File.ReadAllText(PathToTabPlayDB);
                    TabPlayDBTime = lastWriteTime;
                    if (DBConnectionString != "")
                    {
                        using (OdbcConnection connection = new OdbcConnection(DBConnectionString))
                        {
                            connection.Open();

                            // Check if new event is an individual (in which case there will be a field 'South' in the RoundData table)
                            string SQLString = $"SELECT TOP 1 South FROM RoundData";
                            OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                            try
                            {
                                ODBCRetryHelper.ODBCRetry(() =>
                                {
                                    cmd.ExecuteScalar();
                                    IsIndividual = true;
                                });
                            }
                            catch (OdbcException e)
                            {
                                if (e.Errors.Count > 1 || e.Errors[0].SQLState != "07002")   // Error other than field 'South' doesn't exist
                                {
                                    throw (e);
                                }
                                else
                                {
                                    IsIndividual = false;
                                }
                            }
                            finally
                            {
                                cmd.Dispose();
                            }

                            // Create list of sections
                            SQLString = "SELECT ID, Letter, Tables, MissingPair FROM Section";
                            SectionList.Clear();
                            cmd = new OdbcCommand(SQLString, connection);
                            OdbcDataReader reader = null;
                            try
                            {
                                ODBCRetryHelper.ODBCRetry(() =>
                                {
                                    reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Section s = new Section
                                        {
                                            SectionID = reader.GetInt32(0),
                                            SectionLetter = reader.GetString(1),
                                            NumTables = reader.GetInt32(2),
                                            MissingPair = reader.GetInt32(3)
                                        };
                                        SectionList.Add(s);
                                    }
                                });
                            }
                            finally
                            {
                                reader.Close();
                                cmd.Dispose();
                            }

                            // Retrieve global PlayerNames table
                            SQLString = $"SELECT Name, ID FROM PlayerNames";
                            PlayerNamesTable.Clear();
                            cmd = new OdbcCommand(SQLString, connection);
                            try
                            {
                                ODBCRetryHelper.ODBCRetry(() =>
                                {
                                    reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        PlayerRecord playerRecord = new PlayerRecord
                                        {
                                            Name = reader.GetString(0),
                                            Number = reader.GetInt32(1)
                                        };
                                        PlayerNamesTable.Add(playerRecord);
                                    };
                                });
                            }
                            catch (OdbcException e)
                            {
                                if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Error other than PlayerNames table does not exist
                                {
                                    throw (e);
                                }
                            }
                            finally
                            {
                                reader.Close();
                                cmd.Dispose();
                            }
                        }
                    }
                }
            }
        }

        public static string GetNameFromPlayerNamesTable(int playerNumber)
        {
            if (PlayerNamesTable.Count == 0)
            {
                return "#" + playerNumber;
            }
            PlayerRecord player = PlayerNamesTable.Find(x => (x.Number == playerNumber));
            if (player == null)
            {
                return "Unknown #" + playerNumber;
            }
            else
            {
                return player.Name;
            }
        }
    }
}
