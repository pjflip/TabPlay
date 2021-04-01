// TabPlay - TabPlay, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;
using System.Data.Odbc;

namespace TabPlayStarter
{
    class HandEvaluationsList : List<HandEvaluation>
    {
        private readonly string dbConnectionString;

        public HandEvaluationsList(OdbcConnectionStringBuilder connectionString)
        {
            dbConnectionString = connectionString.ToString();
            using (OdbcConnection connection = new OdbcConnection(dbConnectionString))
            {
                connection.Open();
                string SQLString = "CREATE TABLE HandEvaluation (Section SHORT, Board SHORT, NorthSpades SHORT, NorthHearts SHORT, NorthDiamonds SHORT, NorthClubs SHORT, NorthNoTrump SHORT, EastSpades SHORT, EastHearts SHORT, EastDiamonds SHORT, EastClubs SHORT, EastNoTrump SHORT, SouthSpades SHORT, SouthHearts SHORT, SouthDiamonds SHORT, SouthClubs SHORT, SouthNotrump SHORT, WestSpades SHORT, WestHearts SHORT, WestDiamonds SHORT, WestClubs SHORT, WestNoTrump SHORT, NorthHcp SHORT, EastHcp SHORT, SouthHcp SHORT, WestHcp SHORT)";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S01")  // Error other than HandEvaluation table already exists
                    {
                        throw e;
                    }
                }
                cmd.Dispose();
            }
        }

        public void WriteToDB()
        {
            using (OdbcConnection connection = new OdbcConnection(dbConnectionString))
            {
                connection.Open();
                string SQLString = "DELETE FROM HandEvaluation";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                foreach (HandEvaluation hev in this)
                {
                    if (hev.NorthSpades != -1)
                    {
                        SQLString = $"INSERT INTO HandEvaluation (Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, NorthNotrump, EastSpades, EastHearts, EastDiamonds, EastClubs, EastNotrump, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, SouthNotrump, WestSpades, WestHearts, WestDiamonds, WestClubs, WestNotrump, NorthHcp, EastHcp, SouthHcp, WestHcp) VALUES ({hev.SectionID}, {hev.BoardNumber}, '{hev.NorthSpades}', '{hev.NorthHearts}', '{hev.NorthDiamonds}', '{hev.NorthClubs}', '{hev.NorthNotrump}', '{hev.EastSpades}', '{hev.EastHearts}', '{hev.EastDiamonds}', '{hev.EastClubs}', '{hev.EastNotrump}', '{hev.SouthSpades}', '{hev.SouthHearts}', '{hev.SouthDiamonds}', '{hev.SouthClubs}', '{hev.SouthNotrump}', '{hev.WestSpades}', '{hev.WestHearts}', '{hev.WestDiamonds}', '{hev.WestClubs}', '{hev.WestNotrump}', '{hev.NorthHcp}', '{hev.EastHcp}', '{hev.SouthHcp}', '{hev.WestHcp}')";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                }
                cmd.Dispose();
            }
        }
    }
}
