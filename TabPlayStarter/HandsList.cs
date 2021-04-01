// TabPlay - TabPlay, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;

namespace TabPlayStarter
{
    class HandsList : List<Hand>
    {
        public HandsList(OdbcConnectionStringBuilder connectionString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString.ToString()))
            {
                string SQLString = $"SELECT Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs FROM HandRecord";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Hand hand = new Hand
                        {
                            SectionID = reader.GetInt32(0),
                            BoardNumber = reader.GetInt32(1),
                            NorthSpades = reader.GetValue(2).ToString(),
                            NorthHearts = reader.GetValue(3).ToString(),
                            NorthDiamonds = reader.GetValue(4).ToString(),
                            NorthClubs = reader.GetValue(5).ToString(),
                            EastSpades = reader.GetValue(6).ToString(),
                            EastHearts = reader.GetValue(7).ToString(),
                            EastDiamonds = reader.GetValue(8).ToString(),
                            EastClubs = reader.GetValue(9).ToString(),
                            SouthSpades = reader.GetValue(10).ToString(),
                            SouthHearts = reader.GetValue(11).ToString(),
                            SouthDiamonds = reader.GetValue(12).ToString(),
                            SouthClubs = reader.GetValue(13).ToString(),
                            WestSpades = reader.GetValue(14).ToString(),
                            WestHearts = reader.GetValue(15).ToString(),
                            WestDiamonds = reader.GetValue(16).ToString(),
                            WestClubs = reader.GetValue(17).ToString()
                        };
                        Add(hand);
                    }
                    reader.Close();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Error other than HandRecord table does not exist
                    {
                        throw e;
                    }
                }
                cmd.Dispose();
            }
        }

        public HandsList(string pathToPbnFile)
        {
            bool newBoard = false;
            string line = null;
            char[] quoteDelimiter = { '"' };

            Clear();

            StreamReader file = new StreamReader(pathToPbnFile);
            if (!file.EndOfStream)
            {
                line = file.ReadLine();
                newBoard = line.Length > 7 && line.Substring(0, 7) == "[Board ";
            }
            while (!file.EndOfStream)
            {
                if (newBoard)
                {
                    newBoard = false;
                    Hand hand = new Hand()
                    {
                        SectionID = 1,         // Default SectionID=1 if hands apply to more than one section
                        NorthSpades = "###"
                    };
                    hand.BoardNumber = Convert.ToInt32(line.Split(quoteDelimiter)[1]);
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Length > 6 && line.Substring(0, 6) == "[Deal ")
                        {
                            hand.PBN = line.Split(quoteDelimiter)[1];
                        }
                        else if (line.Length > 7 && line.Substring(0, 7) == "[Board ")
                        {
                            newBoard = true;
                            if (hand.NorthSpades != "###") Add(hand);
                            break;
                        }
                    }
                    if (file.EndOfStream)
                    {
                        if (hand.NorthSpades != "###") Add(hand);
                    }
                }
                else if (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    newBoard = line.Length > 7 && line.Substring(0, 7) == "[Board ";
                }
            }
            file.Close();
        }

        public void WriteToDB(OdbcConnectionStringBuilder connectionString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString.ToString()))
            {
                connection.Open();
                string SQLString = "DELETE FROM HandRecord";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                foreach (Hand hc in this)
                {
                    if (hc.NorthSpades != "###")
                    {
                        SQLString = $"INSERT INTO HandRecord (Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs) VALUES ({hc.SectionID}, {hc.BoardNumber}, '{hc.NorthSpades}', '{hc.NorthHearts}', '{hc.NorthDiamonds}', '{hc.NorthClubs}', '{hc.EastSpades}', '{hc.EastHearts}', '{hc.EastDiamonds}', '{hc.EastClubs}', '{hc.SouthSpades}', '{hc.SouthHearts}', '{hc.SouthDiamonds}', '{hc.SouthClubs}', '{hc.WestSpades}', '{hc.WestHearts}', '{hc.WestDiamonds}', '{hc.WestClubs}')";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                }
                cmd.Dispose();
            }
        }
    }
}
