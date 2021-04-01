// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Traveller : List<Result>
    {
        public int DeviceNumber { get; private set; }
        public int BoardNumber { get; private set; }
        public int ContractLevel { get; private set; }
        public string DisplayContract { get; private set; }
        public string Declarer { get; private set; }
        public int Score { get; private set; }
        public int PercentageNS { get; private set; }

        public Traveller(int deviceNumber, Table table)
        {
            DeviceNumber = deviceNumber;
            Device device = AppData.DeviceList[deviceNumber];
            BoardNumber = table.BoardNumber;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString;
                OdbcCommand cmd = null;
                OdbcDataReader reader = null;
                try
                {
                    if (AppData.IsIndividual)
                    {
                        SQLString = $"SELECT PairNS, PairEW, South, West, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={device.SectionID} AND Board={BoardNumber}";
                        cmd = new OdbcCommand(SQLString, connection);
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Result result = new Result()
                                {
                                    BoardNumber = BoardNumber,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    South = reader.GetInt32(2),
                                    West = reader.GetInt32(3),
                                    DeclarerNSEW = reader.GetString(4),
                                    Contract = reader.GetString(5),
                                    LeadCard = reader.GetString(6),
                                    TricksTakenSymbol = reader.GetString(7)
                                };
                                if (result.Contract.Length > 2)  // Testing for unplayed boards and corrupt ReceivedData table
                                {
                                    result.CalculateScore();
                                    if (table.PairNumber[0] == result.PairNS)
                                    {
                                        ContractLevel = result.ContractLevel;
                                        DisplayContract = Utilities.DisplayContract(ContractLevel, result.ContractSuit, result.ContractX) + " " + result.TricksTakenSymbol;
                                        Declarer = result.Declarer;
                                        Score = result.Score;
                                        result.CurrentResult = true;
                                    }
                                    Add(result);
                                }
                            }
                        });
                    }
                    else
                    {
                        SQLString = $"SELECT PairNS, PairEW, [NS/EW], Contract, LeadCard, Result FROM ReceivedData WHERE Section={device.SectionID} AND Board={BoardNumber}";
                        cmd = new OdbcCommand(SQLString, connection);
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Result result = new Result()
                                {
                                    BoardNumber = BoardNumber,
                                    PairNS = reader.GetInt32(0),
                                    PairEW = reader.GetInt32(1),
                                    DeclarerNSEW = reader.GetString(2),
                                    Contract = reader.GetString(3),
                                    LeadCard = reader.GetString(4),
                                    TricksTakenSymbol = reader.GetString(5)
                                };
                                if (result.Contract.Length > 2)  // Testing for unplayed boards and corrupt ReceivedData table
                                {
                                    result.CalculateScore();
                                    if (table.PairNumber[0] == result.PairNS)
                                    {
                                        ContractLevel = result.ContractLevel;
                                        DisplayContract = Utilities.DisplayContract(ContractLevel, result.ContractSuit, result.ContractX) + " " + result.TricksTakenSymbol;
                                        Declarer = result.Declarer;
                                        Score = result.Score;
                                        result.CurrentResult = true;
                                    }
                                    Add(result);
                                }
                            }
                        });
                    }
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            };

            // Sort traveller and calculate percentage for current result
            Sort((x, y) => y.Score.CompareTo(x.Score));
            if (Settings.ShowPercentage)
            {
                if (Count == 1)
                {
                    PercentageNS = 50;
                }
                else
                {
                    int currentMP = 2 * FindAll((x) => x.Score < Score).Count + FindAll((x) => x.Score == Score).Count - 1;
                    PercentageNS = Convert.ToInt32(50.0 * currentMP / (Count - 1));
                }
            }
            else
            {
                PercentageNS = -1;   // Don't show percentage
            }
        }
    }
}