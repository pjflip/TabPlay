// TabPlay - TabPlay, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Data.Odbc;
using System.Text;

namespace TabPlayStarter
{
    class Options
    {
        public bool ShowTraveller { get; set; }
        public bool ShowPercentage { get; set; }
        public int ShowRanking { get; set; }
        public bool ShowHandRecord { get; set; }
        public bool NumberEntryEachRound { get; set; }
        public int NameSource { get; set; }
        public int PollInterval { get; set; }

        private readonly string dbConnectionString;

        public Options(OdbcConnectionStringBuilder connectionString)
        {
            dbConnectionString = connectionString.ToString();
            using (OdbcConnection connection = new OdbcConnection(dbConnectionString))
            {
                string SQLString = $"SELECT ShowResults, ShowPercentage, BM2Ranking, BM2ViewHandRecord, BM2NumberEntryEachRound, BM2NameSource, PollInterval FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                reader.Read();
                ShowTraveller = reader.GetBoolean(0);
                ShowPercentage = reader.GetBoolean(1);
                ShowRanking = reader.GetInt32(2);
                ShowHandRecord = reader.GetBoolean(3);
                NumberEntryEachRound = reader.GetBoolean(4);
                NameSource = reader.GetInt32(5);
                PollInterval = reader.GetInt32(6);
                reader.Close();
                cmd.Dispose();
            }
        }

        public void UpdateDB()
        {
            using (OdbcConnection connection = new OdbcConnection(dbConnectionString))
            {
                StringBuilder SQLString = new StringBuilder();
                SQLString.Append($"UPDATE Settings SET");
                if (ShowTraveller)
                {
                    SQLString.Append(" ShowResults=YES,");
                }
                else
                {
                    SQLString.Append(" ShowResults=NO,");
                }
                if (ShowPercentage)
                {
                    SQLString.Append(" ShowPercentage=YES,");
                }
                else
                {
                    SQLString.Append(" ShowPercentage=NO,");
                }
                SQLString.Append($" BM2Ranking={ShowRanking},");
                if (ShowHandRecord)
                {
                    SQLString.Append(" BM2ViewHandRecord=YES,");
                }
                else
                {
                    SQLString.Append(" BM2ViewHandRecord=NO,");
                }
                if (NumberEntryEachRound)
                {
                    SQLString.Append(" BM2NumberEntryEachRound=YES,");
                }
                else
                {
                    SQLString.Append(" BM2NumberEntryEachRound=NO,");
                }
                SQLString.Append($" BM2NameSource={NameSource},");
                SQLString.Append($" PollInterval={PollInterval}");
                OdbcCommand cmd = new OdbcCommand(SQLString.ToString(), connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}