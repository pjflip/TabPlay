// TabPlay - TabPlay, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;
using System.Windows.Forms;

namespace TabPlayStarter
{
    class Database
    {
        public string ConnectionString { get; private set; }

        public Database (string pathToDB)
        {
            OdbcConnectionStringBuilder cs = new OdbcConnectionStringBuilder();
            cs.Driver = "Microsoft Access Driver (*.mdb)";
            cs.Add("Dbq", pathToDB);
            cs.Add("Uid", "Admin");
            cs.Add("Pwd", "");
            ConnectionString = cs.ToString();
        }

        public bool Initialize()
        {
            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Check sections
                    int sectionID = 0;
                    string SQLString = "SELECT ID, Letter, [Tables], Winners FROM Section";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        sectionID = reader.GetInt32(0);
                        string sectionLetter = reader.GetString(1);
                        int numTables = reader.GetInt32(2);
                        if (sectionID < 1 || sectionID > 4 || (sectionLetter != "A" && sectionLetter != "B" && sectionLetter != "C" && sectionLetter != "D"))
                        {
                            reader.Close();
                            MessageBox.Show("Database countains incorrect Sections.  Maximum 4 Sections labelled A, B, C, D", "TabPlayStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        if (numTables > 30)
                        {
                            reader.Close();
                            MessageBox.Show("Database countains > 30 Tables in a Section", "TabPlayStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    reader.Close();
                    if (sectionID == 0)
                    {
                        MessageBox.Show("Database contains no Sections", "TabPlayStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Add column 'BiddingStarted' to table 'Tables' if it doesn't already exist
                    SQLString = "ALTER TABLE [Tables] ADD BiddingStarted YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }

                    // Add column 'BiddingComplete' to table 'Tables' if it doesn't already exist
                    SQLString = "ALTER TABLE [Tables] ADD BiddingComplete YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }

                    // Add column 'BiddingStarted' to table 'Tables' if it doesn't already exist
                    SQLString = "ALTER TABLE [Tables] ADD PlayComplete YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }

                    // Add column 'Name' to table 'PlayerNumbers' if it doesn't already exist
                    SQLString = "ALTER TABLE PlayerNumbers ADD [Name] VARCHAR(30)";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }

                    // Add column 'Round' to table 'PlayerNumbers' if it doesn't already exist
                    SQLString = "ALTER TABLE PlayerNumbers ADD [Round] SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }

                    // Ensure that all Round values are set to 0 to start with
                    SQLString = "UPDATE PlayerNumbers SET [Round]=0";
                    cmd = new OdbcCommand(SQLString, connection);
                    cmd.ExecuteNonQuery();

                    // Check if this is an individual event, when RoundData will have a 'South' column
                    bool individualEvent = true;
                    SQLString = $"SELECT TOP 1 South FROM RoundData";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteScalar();
                    }
                    catch (OdbcException)
                    {
                        individualEvent = false;
                    }

                    // If this is an individual event, add extra columns South and West to ReceivedData and IntermediateData if they don't exist
                    if (individualEvent)
                    {
                        SQLString = "ALTER TABLE ReceivedData ADD South SHORT";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (OdbcException e)
                        {
                            if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                            {
                                throw e;
                            }
                        }
                        SQLString = "ALTER TABLE ReceivedData ADD West SHORT";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (OdbcException e)
                        {
                            if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                            {
                                throw e;
                            }
                        }
                        SQLString = "ALTER TABLE IntermediateData ADD South SHORT";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (OdbcException e)
                        {
                            if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                            {
                                throw e;
                            }
                        }
                        SQLString = "ALTER TABLE IntermediateData ADD West SHORT";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (OdbcException e)
                        {
                            if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                            {
                                throw e;
                            }
                        }
                    }

                    // Add a new column 'TabPlayPairNo' to table 'PlayerNumbers' if it doesn't exist and populate it if possible
                    SQLString = "ALTER TABLE PlayerNumbers ADD TabPlayPairNo SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "SELECT Section, [Table], Direction FROM PlayerNumbers";
                    cmd = new OdbcCommand(SQLString, connection);
                    reader = cmd.ExecuteReader();
                    OdbcCommand cmd2 = new OdbcCommand();
                    while (reader.Read())
                    {
                        int section = reader.GetInt32(0);
                        int table = reader.GetInt32(1);
                        string direction = reader.GetString(2);
                        if (individualEvent)
                        {
                            switch (direction)
                            {
                                case "N":
                                    SQLString = $"SELECT NSPair FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                                case "S":
                                    SQLString = $"SELECT South FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                                case "E":
                                    SQLString = $"SELECT EWPair FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                                case "W":
                                    SQLString = $"SELECT West FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                            }
                        }
                        else
                        {
                            switch (direction)
                            {
                                case "N":
                                case "S":
                                    SQLString = $"SELECT NSPair FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                                case "E":
                                case "W":
                                    SQLString = $"SELECT EWPair FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                            }
                        }
                        cmd2 = new OdbcCommand(SQLString, connection);
                        object queryResult = cmd2.ExecuteScalar();
                        string pairNo = queryResult.ToString();
                        SQLString = $"UPDATE PlayerNumbers SET TabPlayPairNo={pairNo} WHERE Section={section} AND [Table]={table} AND Direction='{direction}'";
                        cmd2 = new OdbcCommand(SQLString, connection);
                        cmd2.ExecuteNonQuery();
                    }
                    cmd2.Dispose();

                    // Add various columns to table 'Settings' if they don't already exist and set defaults
                    SQLString = "ALTER TABLE Settings ADD ShowResults YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET ShowResults=YES";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD ShowPercentage YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET ShowPercentage=YES";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2NumberEntryEachRound YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET BM2NumberEntryEachRound=NO";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2ViewHandRecord YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET BM2ViewHandRecord=YES";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2Ranking SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET BM2Ranking=1";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2NameSource SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET ShowResults=0";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD PollInterval SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    int pollInterval = Properties.Settings.Default.PollInterval;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = $"UPDATE Settings SET PollInterval={pollInterval}";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }


                    // Check if any previous results in database
                    object Result;
                    SQLString = "SELECT * FROM ReceivedData";
                    cmd = new OdbcCommand(SQLString, connection);
                    Result = cmd.ExecuteScalar();
                    if (Result != null)
                    {
                        MessageBox.Show("Database contains previous results", "TabPlayStarter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "TabPlayStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
    }
}
