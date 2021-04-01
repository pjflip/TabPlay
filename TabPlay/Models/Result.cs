// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;

namespace TabPlay.Models
{
    public class Result
    {
        public int DeviceNumber { get; set; }
        public int BoardNumber { get; set; }
        public int PairNS { get; set; }
        public int South { get; set; }
        public int PairEW { get; set; }
        public int West { get; set; }
        public string DeclarerNSEW { get; set; } = "";
        public int ContractLevel { get; set; } = -999;
        public string ContractSuit { get; set; } = "";
        public string ContractX { get; set; } = "";
        public string LeadCard { get; set; } = "";
        public int Score { get; private set; }
        public int MatchpointsNS { get; set; }
        public int MatchpointsEW { get; set; }
        public int MatchpointsMax { get; set; }
        public bool CurrentResult { get; set; } = false;

        // Basic constructor for ranking and result lists
        public Result() { }

        // Constructor to set just contract level for Pass Out and Not Played
        public Result(int deviceNumber, int boardNumber, int contractLevel)
        {
            DeviceNumber = deviceNumber;
            BoardNumber = boardNumber;
            ContractLevel = contractLevel;
        }

        // Constructor to set declarer for intermediate result
        public Result(int deviceNumber, int boardNumber, int contractLevel, string contractSuit, string contractX)
        {
            DeviceNumber = deviceNumber;
            Device device = AppData.DeviceList[deviceNumber];
            BoardNumber = boardNumber;
            ContractLevel = contractLevel;
            ContractSuit = contractSuit;
            ContractX = contractX;
            
            List<DatabaseBid> databaseBidList = new List<DatabaseBid>();
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT Counter, Bid, Direction FROM BiddingData WHERE Section={device.SectionID} AND Table={device.TableNumber} AND Round={device.RoundNumber} AND Board={boardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int tempCounter = reader.GetInt32(0);
                            string tempBid = reader.GetString(1);
                            string tempDirection = reader.GetString(2);
                            DatabaseBid databaseBid = new DatabaseBid
                            {
                                Counter = tempCounter,
                                Bid = tempBid,
                                Direction = tempDirection
                            };
                            databaseBidList.Add(databaseBid);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }

            databaseBidList.Sort((x, y) => x.Counter.CompareTo(y.Counter));
            DatabaseBid contractBid = databaseBidList.FindLast(x => x.Bid != "PASS" && x.Bid != "x" && x.Bid != "xx");
            if (contractBid.Direction == "N" || contractBid.Direction == "S")
            {
                DeclarerNSEW = databaseBidList.Find(x => (x.Direction == "N" || x.Direction == "S") && x.Bid.Length > 1 && x.Bid.Substring(1, 1) == contractBid.Bid.Substring(1, 1)).Direction;
            }
            else
            {
                DeclarerNSEW = databaseBidList.Find(x => (x.Direction == "E" || x.Direction == "W") && x.Bid.Length > 1 && x.Bid.Substring(1, 1) == contractBid.Bid.Substring(1, 1)).Direction;
            }
        }

        // Database read constructor
        public Result(int deviceNumber, int boardNumber, string dataTable)
        {
            DeviceNumber = deviceNumber;
            Device device = AppData.DeviceList[deviceNumber];
            BoardNumber = boardNumber;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = "";

                if (AppData.IsIndividual)
                {
                    SQLString = $"SELECT [NS/EW], Contract, Result, LeadCard, Remarks, PairNS, PairEW, South, West FROM {dataTable} WHERE Section={device.SectionID} AND [Table]={device.TableNumber} AND Round={device.RoundNumber} AND Board={BoardNumber}";
                }
                else
                {
                    SQLString = $"SELECT [NS/EW], Contract, Result, LeadCard, Remarks, PairNS, PairEW FROM {dataTable} WHERE Section={device.SectionID} AND [Table]={device.TableNumber} AND Round={device.RoundNumber} AND Board={BoardNumber}";
                }
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader.GetString(4) == "Not played")
                            {
                                ContractLevel = -1;
                                ContractSuit = "";
                                ContractX = "";
                                DeclarerNSEW = "";
                                TricksTakenNumber = -1;
                                LeadCard = "";
                            }
                            else
                            {
                                string temp = reader.GetString(1);
                                Contract = temp;   // Sets ContractLevel, etc
                                if (ContractLevel == 0)  // Passed out
                                {
                                    DeclarerNSEW = "";
                                    TricksTakenNumber = -1;
                                    LeadCard = "";
                                }
                                else
                                {
                                    DeclarerNSEW = reader.GetString(0);
                                    TricksTakenSymbol = reader.GetString(2);
                                    LeadCard = reader.GetString(3);
                                }
                            }
                            PairNS = reader.GetInt32(5);
                            PairEW = reader.GetInt32(6);
                            if (AppData.IsIndividual)
                            {
                                South = reader.GetInt32(7);
                                West = reader.GetInt32(8);
                            }
                        }
                        else  // No result in database
                        {
                            ContractLevel = -999;
                            ContractSuit = "";
                            ContractX = "";
                            DeclarerNSEW = "";
                            TricksTakenNumber = -1;
                            LeadCard = "";
                            PairNS = 0;
                            PairEW = 0;
                            if (AppData.IsIndividual)
                            {
                                South = 0;
                                West = 0;
                            }
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }
        }

        public string Declarer
        {
            get
            {
                if (DeclarerNSEW == "N") return "North";
                else if (DeclarerNSEW == "E") return "East";
                else if (DeclarerNSEW == "S") return "South";
                else if (DeclarerNSEW == "W") return "West";
                else return "";
            }
        }

        public string Contract
        {
            get
            {
                if (ContractLevel < 0)  // No result or board not played
                {
                    return "";
                }
                else if (ContractLevel == 0)
                {
                    return "PASS";
                }
                else
                {
                    string contract = $"{ContractLevel} {ContractSuit}";
                    if (ContractX != "")
                    {
                        contract += " " + ContractX;
                    }
                    return contract;
                }
            }
            set
            {
                if (value.Length <= 2)  // Board not played or corrupt data
                {
                    ContractLevel = -999;
                }
                else if (value == "PASS")
                {
                    ContractLevel = 0;
                    ContractSuit = "";
                    ContractX = "";
                }
                else  // Contract (hopefully) contains a valid contract
                {
                    string[] temp = value.Split(' ');
                    ContractLevel = Convert.ToInt32(temp[0]);
                    ContractSuit = temp[1];
                    if (temp.Length > 2) ContractX = temp[2];
                    else ContractX = "";
                }
            }
        }

        private int tricksTakenNumber;
        public int TricksTakenNumber
        {
            get
            {
                return tricksTakenNumber;
            }
            set
            {
                tricksTakenNumber = value;
                if (tricksTakenNumber == -1)
                {
                    tricksTakenSymbol = "";
                }
                else
                {
                    int tricksTakenLevel = tricksTakenNumber - ContractLevel - 6;
                    if (tricksTakenLevel == 0)
                    {
                        tricksTakenSymbol = "=";
                    }
                    else
                    {
                        tricksTakenSymbol = tricksTakenLevel.ToString("+#;-#;0");
                    }
                }
            }
        }

        private string tricksTakenSymbol;
        public string TricksTakenSymbol
        {
            get
            {
                return tricksTakenSymbol;
            }
            set
            {
                tricksTakenSymbol = value;
                if (tricksTakenSymbol == "")
                {
                    tricksTakenNumber = -1;
                }
                else if (tricksTakenSymbol == "=")
                {
                    tricksTakenNumber = ContractLevel + 6;
                }
                else
                {
                    tricksTakenNumber = ContractLevel + Convert.ToInt32(tricksTakenSymbol) + 6;
                }
            }
        }

        public string DisplayLeadCard()
        {
            string s = LeadCard;
            if (s == null || s == "" || s == "SKIP") return "";
            s = s.Replace("S", "<a style=\"color:black\">&spades;</a>");
            s = s.Replace("H", "<a style=\"color:red\">&hearts;</a>");
            s = s.Replace("D", "<a style=\"color:lightsalmon\">&diams;</a>");
            s = s.Replace("C", "<a style=\"color:darkblue\">&clubs;</a>");
            s = s.Replace("10", "T");
            return s;
        }

        public string DisplayTravellerContract()
        {
            if (ContractLevel == 0) return "<a style=\"color:darkgreen\">PASS</a>";
            StringBuilder s = new StringBuilder(ContractLevel.ToString());
            switch (ContractSuit) {
                case "S":
                    s.Append("<a style=\"color:black\">&spades;</a>");
                    break;
                case "H":
                    s.Append("<a style=\"color:red\">&hearts;</a>");
                    break;
                case "D":
                    s.Append("<a style=\"color:lightsalmon\">&diams;</a>");
                    break;
                case "C":
                    s.Append("<a style=\"color:darkblue\">&clubs;</a>");
                    break;
                case "NT":
                    s.Append("NT");
                    break;
            }
            if (ContractX != "")
            {
                s.Append(ContractX.ToUpper());
            }
            return s.ToString();
        }

        public void CalculateScore()
        {
            Score = 0;
            if (ContractLevel <= 0) return;

            bool vul;
            if (DeclarerNSEW == "N" || DeclarerNSEW == "S")
            {
                vul = Utilities.NSVulnerability[(Convert.ToInt32(BoardNumber) - 1) % 16];
            }
            else
            {
                vul = Utilities.EWVulnerability[(Convert.ToInt32(BoardNumber) - 1) % 16];
            }
            int diff = TricksTakenNumber - ContractLevel - 6;
            if (diff < 0)      // Contract not made
            {
                if (ContractX == "")
                {
                    if (vul)
                    {
                        Score = 100 * diff;
                    }
                    else
                    {
                        Score = 50 * diff;
                    }
                }
                else if (ContractX == "x")
                {
                    if (vul)
                    {
                        Score = 300 * diff + 100;
                    }
                    else
                    {
                        Score = 300 * diff + 400;
                        if (diff == -1) Score -= 200;
                        if (diff == -2) Score -= 100;
                    }
                }
                else  // x = "xx"
                {
                    if (vul)
                    {
                        Score = 600 * diff + 200;
                    }
                    else
                    {
                        Score = 600 * diff + 800;
                        if (diff == -1) Score -= 400;
                        if (diff == -2) Score -= 200;
                    }
                }
            }
            else      // Contract made
            {
                // Basic score, game/part-score bonuses and making x/xx contract bonuses
                if (ContractSuit == "C" || ContractSuit == "D")
                {
                    if (ContractX == "")
                    {
                        Score = 20 * (TricksTakenNumber - 6);
                        if (ContractLevel <= 4)
                        {
                            Score += 50;
                        }
                        else
                        {
                            if (vul) Score += 500;
                            else Score += 300;
                        }
                    }
                    else if (ContractX == "x")
                    {
                        Score = 40 * ContractLevel + 50;
                        if (vul) Score += 200 * diff;
                        else Score += 100 * diff;
                        if (ContractLevel <= 2)
                        {
                            Score += 50;
                        }
                        else
                        {
                            if (vul) Score += 500;
                            else Score += 300;
                        }
                    }
                    else    // x = "xx"
                    {
                        Score = 80 * ContractLevel + 100;
                        if (vul) Score += 400 * diff;
                        else Score += 200 * diff;
                        if (ContractLevel == 1)
                        {
                            Score += 50;
                        }
                        else
                        {
                            if (vul) Score += 500;
                            else Score += 300;
                        }
                    }
                }
                else   // Major suits and NT
                {
                    if (ContractX == "")
                    {
                        Score = 30 * (TricksTakenNumber - 6);
                        if (ContractSuit == "NT")
                        {
                            Score += 10;
                            if (ContractLevel <= 2)
                            {
                                Score += 50;
                            }
                            else
                            {
                                if (vul) Score += 500;
                                else Score += 300;
                            }
                        }
                        else      // Major suit
                        {
                            if (ContractLevel <= 3)
                            {
                                Score += 50;
                            }
                            else
                            {
                                if (vul) Score += 500;
                                else Score += 300;
                            }
                        }
                    }
                    else if (ContractX == "x")
                    {
                        Score = 60 * ContractLevel + 50;
                        if (ContractSuit == "NT") Score += 20;
                        if (vul) Score += 200 * diff;
                        else Score += 100 * diff;
                        if (ContractLevel <= 1)
                        {
                            Score += 50;
                        }
                        else
                        {
                            if (vul) Score += 500;
                            else Score += 300;
                        }
                    }
                    else    // x = "xx"
                    {
                        Score = 120 * ContractLevel + 100;
                        if (ContractSuit == "NT") Score += 40;
                        if (vul) Score += 400 * diff + 500;
                        else Score += 200 * diff + 300;
                    }
                }
                // Slam bonuses
                if (ContractLevel == 6)
                {
                    if (vul) Score += 750;
                    else Score += 500;
                }
                else if (ContractLevel == 7)
                {
                    if (vul) Score += 1500;
                    else Score += 1000;
                }
            }
            if (DeclarerNSEW == "E" || DeclarerNSEW == "W") Score = -Score;
        }

        public void UpdateDB(string dataTable)
        {
            int declarerNumber = 0;
            string remarks = "";
            Device device = AppData.DeviceList[DeviceNumber];

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = "";
                OdbcCommand cmd = null;

                // Get pair numbers
                if (AppData.IsIndividual)
                {
                    SQLString = $"SELECT NSPair, EWPair, South, West FROM RoundData WHERE Section={device.SectionID} AND Table={device.TableNumber} AND Round={device.RoundNumber}";
                    cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                PairNS = reader.GetInt32(0);
                                PairEW = reader.GetInt32(1);
                                South = reader.GetInt32(2);
                                West = reader.GetInt32(3);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
                else  // Not individual
                {
                    SQLString = $"SELECT NSPair, EWPair FROM RoundData WHERE Section={device.SectionID} AND Table={device.TableNumber} AND Round={device.RoundNumber}";
                    cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                PairNS = reader.GetInt32(0);
                                PairEW = reader.GetInt32(1);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }

                // Set remarks and declarer
                if (ContractLevel == -1)
                {
                    remarks = "Not played";
                    tricksTakenSymbol = "";
                }
                if (ContractLevel > 0)
                {
                    if (DeclarerNSEW == "N" || DeclarerNSEW == "S")   // Only use N or E player numbers for both pairs and individuals
                    {
                        declarerNumber = PairNS;
                    }
                    else
                    {
                        declarerNumber = PairEW;
                    }
                }

                // Delete any previous result
                SQLString = $"DELETE FROM {dataTable} WHERE Section={device.SectionID} AND [Table]={device.TableNumber} AND Round={device.RoundNumber} AND Board={BoardNumber}";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery(); 
                    });
                }
                finally
                {
                    cmd.Dispose();
                }

                // Add new result
                if (AppData.IsIndividual)
                {
                    SQLString = $"INSERT INTO {dataTable} (Section, [Table], Round, Board, PairNS, PairEW, South, West, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({device.SectionID}, {device.TableNumber}, {device.RoundNumber}, {BoardNumber}, {PairNS}, {PairEW}, {South}, {West}, {declarerNumber}, '{DeclarerNSEW}', '{Contract}', '{TricksTakenSymbol}', '{LeadCard}', '{remarks}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, False, False, False, False, False, False)";
                }
                else
                {
                    SQLString = $"INSERT INTO {dataTable} (Section, [Table], Round, Board, PairNS, PairEW, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({device.SectionID}, {device.TableNumber}, {device.RoundNumber}, {BoardNumber}, {PairNS}, {PairEW}, {declarerNumber}, '{DeclarerNSEW}', '{Contract}', '{TricksTakenSymbol}', '{LeadCard}', '{remarks}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, False, False, False, False, False, False)";
                }
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }

                // Update table status
                if (dataTable == "IntermediateData")
                {
                    SQLString = $"UPDATE Tables SET BiddingComplete=True WHERE Section={device.SectionID} AND [Table]={device.TableNumber}";
                }
                else
                {
                    SQLString = $"UPDATE Tables SET PlayComplete=True WHERE Section={device.SectionID} AND [Table]={device.TableNumber}";
                }
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }
    }
}
