// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Data.Odbc;
using System.Text;

namespace TabScoreStarter
{
    class Options
    {
        public bool ShowTraveller { get; set; }
        public bool ShowPercentage { get; set; }
        public bool EnterLeadCard { get; set; }
        public bool ValidateLeadCard { get; set; }
        public int ShowRanking { get; set; }
        public bool ShowHandRecord { get; set; }
        public bool NumberEntryEachRound { get; set; }
        public int NameSource { get; set; }
        public int EnterResultsMethod { get; set; }
        public bool TabletMoves { get; set; }

        private readonly string dbConnectionString;

        public Options(Database db)
        {
            dbConnectionString = db.ConnectionString;
            using (OdbcConnection connection = new OdbcConnection(dbConnectionString))
            {
                string SQLString = $"SELECT ShowResults, ShowPercentage, LeadCard, BM2ValidateLeadCard, BM2Ranking, BM2ViewHandRecord, BM2NumberEntryEachRound, BM2NameSource, EnterResultsMethod, TabletMoves FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                reader.Read();
                ShowTraveller = reader.GetBoolean(0);
                ShowPercentage = reader.GetBoolean(1);
                EnterLeadCard = reader.GetBoolean(2);
                ValidateLeadCard = reader.GetBoolean(3);
                ShowRanking = reader.GetInt32(4);
                ShowHandRecord = reader.GetBoolean(5);
                NumberEntryEachRound = reader.GetBoolean(6);
                NameSource = reader.GetInt32(7);
                EnterResultsMethod = reader.GetInt32(8);
                if (EnterResultsMethod != 1) EnterResultsMethod = 0;
                TabletMoves = reader.GetBoolean(9);
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
                if (EnterLeadCard)
                {
                    SQLString.Append(" LeadCard=YES,");
                }
                else
                {
                    SQLString.Append(" LeadCard=NO,");
                }
                if (ValidateLeadCard)
                {
                    SQLString.Append(" BM2ValidateLeadCard=YES,");
                }
                else
                {
                    SQLString.Append(" BM2ValidateLeadCard=NO,");
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
                SQLString.Append($" EnterResultsMethod={EnterResultsMethod},");
                if (TabletMoves)
                {
                    SQLString.Append(" TabletMoves=YES");
                }
                else
                {
                    SQLString.Append(" TabletMoves=NO");
                }
                OdbcCommand cmd = new OdbcCommand(SQLString.ToString(), connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}

