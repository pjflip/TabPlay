// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabPlay.Models
{
    // Settings is a global class that applies accross all sessions
    public static class Settings
    {
        public static bool ShowResults { get; private set; }
        public static bool ShowPercentage { get; private set; }
        public static int ShowRanking { get; private set; }
        public static bool ShowHandRecord { get; private set; }
        public static bool NumberEntryEachRound { get; private set; }
        public static int NameSource { get; private set; }
        public static int PollInterval { get; private set; }

        private static DateTime UpdateTime;

        public static void Refresh()
        {
            if (DateTime.Now.Subtract(UpdateTime).TotalMinutes < 1.0) return;  // Settings updated recently, so don't bother
            UpdateTime = DateTime.Now;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = "SELECT ShowResults, ShowPercentage, BM2Ranking, BM2ViewHandRecord, BM2NumberEntryEachRound, BM2NameSource, PollInterval FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ShowResults = reader.GetBoolean(0);
                            ShowPercentage = reader.GetBoolean(1);
                            ShowRanking = reader.GetInt32(2);
                            ShowHandRecord = reader.GetBoolean(3);
                            NumberEntryEachRound = reader.GetBoolean(4);
                            NameSource = reader.GetInt32(5);
                            PollInterval = reader.GetInt32(6);
                        }
                    });
                }
                catch
                {
                    ShowResults = true;
                    ShowPercentage = true;
                    ShowRanking = 1;
                    ShowHandRecord = true;
                    NumberEntryEachRound = true;
                    NameSource = 0;
                    PollInterval = 1000;
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