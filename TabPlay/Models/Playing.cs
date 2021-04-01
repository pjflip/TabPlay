// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Playing
    {
        public int DeviceNumber { get; private set; }
        public int BoardNumber { get; private set; }
        public int ContractLevel { get; private set; }
        public string ContractSuit { get; private set; }
        public string ContractX { get; private set; }
        public string DisplayContract { get; private set; }
        public string Declarer { get; private set; }
        public int DummyDirectionNumber { get; private set; }
        public string[] Direction { get; private set; }
        public int[] PairNumber { get; set; }
        public string[] PlayerName { get; private set; }
        public string[,] CardString { get; private set; }
        public string[,] DisplayRank { get; private set; }
        public string[,] DisplaySuit { get; private set; }
        public bool[,] CardPlayed { get; private set; }
        public int[,] SuitLengths { get; private set; }
        public bool Vuln02 { get; private set; }
        public bool Vuln13 { get; private set; }
        public int PlayCounter { get; private set; }
        public int PlayDirectionNumber { get; private set; }
        public int LastCardNumber { get; set; }
        public string LastCardString { get; set; }
        public int TrickNumber { get; private set; }
        public int TricksNS { get; private set; }
        public int TricksEW { get; private set; }
        public string[] TrickCardString { get; private set; }
        public int[] TrickRank { get; private set; }
        public string[] TrickSuit { get; private set; }
        public string[] TrickDisplayRank { get; private set; }
        public string[] TrickDisplaySuit { get; private set; }
        public string TrickLeadSuit { get; private set; }
        public string PairOrPlayer { get; private set; }
        public int PollInterval { get; private set; }

        public Playing(int deviceNumber, Table table)
        {
            DeviceNumber = deviceNumber;
            Device device = AppData.DeviceList[deviceNumber];
            BoardNumber = table.BoardNumber;

            // All directionNumbers are relative to the direction that is 0
            int directionNumber = Utilities.DirectionToNumber(device.Direction);
            int northDirectionNumber = (4 - directionNumber) % 4;
            Direction = new string[4];
            Direction[northDirectionNumber] = "North";
            Direction[(northDirectionNumber + 1) % 4] = "East";
            Direction[(northDirectionNumber + 2) % 4] = "South";
            Direction[(northDirectionNumber + 3) % 4] = "West";
            PairNumber = new int[4];
            PlayerName = new string[4];
            for (int i = 0; i < 4; i++)
            {
                PairNumber[i] = table.PairNumber[(directionNumber + i) % 4];
                PlayerName[i] = table.PlayerName[(directionNumber + i) % 4];
            }

            if (northDirectionNumber % 2 == 0)
            {
                Vuln02 = Utilities.NSVulnerability[(BoardNumber - 1) % 16];
                Vuln13 = Utilities.EWVulnerability[(BoardNumber - 1) % 16];
            }
            else
            {
                Vuln02 = Utilities.EWVulnerability[(BoardNumber - 1) % 16];
                Vuln13 = Utilities.NSVulnerability[(BoardNumber - 1) % 16];
            }

            // Set default values for no plays
            PlayCounter = -999;
            LastCardNumber = -1;
            LastCardString = "";
            TrickNumber = 1;
            TricksNS = 0;
            TricksEW = 0;
            TrickLeadSuit = "";
            TrickCardString = new string[4];
            TrickRank = new int[4];
            TrickSuit = new string[4];
            TrickDisplayRank = new string[4];
            TrickDisplaySuit = new string[4];
            for (int i = 0; i < 4; i++)
            {
                TrickCardString[i] = "";
                TrickRank[i] = 0;
                TrickSuit[i] = "";
                TrickDisplayRank[i] = "";
                TrickDisplaySuit[i] = "";
            }
            PollInterval = Settings.PollInterval;
            if (AppData.IsIndividual) PairOrPlayer = "Player"; else PairOrPlayer = "Pair";
            List<Play> playList = new List<Play>();

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();

                // Get contract details
                string SQLString = $"SELECT [NS/EW], Contract FROM IntermediateData WHERE Section={device.SectionID} AND [Table]={device.TableNumber} AND Round={device.RoundNumber} AND Board={BoardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                string declarerNSEW = "";
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            declarerNSEW = reader.GetString(0);
                            string contractString = reader.GetString(1);
                            string[] temp = contractString.Split(' ');
                            ContractLevel = Convert.ToInt32(temp[0]);
                            ContractSuit = temp[1];
                            if (temp.Length > 2) ContractX = temp[2];
                            else ContractX = "";
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
                DisplayContract = Utilities.DisplayContract(ContractLevel, ContractSuit, ContractX);
                if (declarerNSEW == "N") Declarer = "North";
                else if (declarerNSEW == "E") Declarer = "East";
                else if (declarerNSEW == "S") Declarer = "South";
                else if (declarerNSEW == "W") Declarer = "West";
                PlayDirectionNumber = (northDirectionNumber + Utilities.DirectionToNumber(Declarer) + 1) % 4;
                DummyDirectionNumber = (northDirectionNumber + Utilities.DirectionToNumber(Declarer) + 2) % 4;

                // Get hand records and set cards
                HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == device.SectionID && x.BoardNumber == BoardNumber);
                if (handRecord == null)     // Can't find matching hand record, so use default SectionID = 1
                {
                    handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == BoardNumber);
                }
                CardString = handRecord.HandTable(northDirectionNumber, ContractSuit);
                DisplayRank = new string[4, 13];
                DisplaySuit = new string[4, 13];
                CardPlayed = new bool[4, 13];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        DisplayRank[i, j] = Utilities.DisplayRank(CardString[i, j]);
                        DisplaySuit[i, j] = Utilities.DisplaySuit(CardString[i, j]);
                        CardPlayed[i, j] = false;
                    }
                }
                SuitLengths = handRecord.SuitLengths(northDirectionNumber, ContractSuit);

                // Check PlayData table for any previous plays
                SQLString = $"SELECT Counter, Direction, Card FROM PlayData WHERE Section={device.SectionID} AND Table={device.TableNumber} AND Round={device.RoundNumber} AND Board={BoardNumber}";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string tempPlayDirection = reader.GetString(1);
                            string tempCardString = reader.GetString(2);
                            int tempPlayCounter = Convert.ToInt32(reader.GetValue(0));
                            if (tempPlayDirection == "N") tempPlayDirection = "North";
                            else if (tempPlayDirection == "E") tempPlayDirection = "East";
                            else if (tempPlayDirection == "S") tempPlayDirection = "South";
                            else if (tempPlayDirection == "W") tempPlayDirection = "West";
                            Play play = new Play(tempPlayDirection, 0, tempCardString, tempPlayCounter);
                            playList.Add(play);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }
            playList.Sort((x, y) => x.PlayCounter.CompareTo(y.PlayCounter));

            // Run through each played card (if any) in turn and work out the implications 
            foreach (Play play in playList)
            {
                PlayDirectionNumber = (northDirectionNumber + Utilities.DirectionToNumber(play.PlayDirection)) % 4;
                PlayCounter = play.PlayCounter;
                int cardNumber;
                for (cardNumber = 0; cardNumber < 13; cardNumber++)
                {
                    if (play.CardString == CardString[PlayDirectionNumber, cardNumber]) break;
                }
                CardPlayed[PlayDirectionNumber, cardNumber] = true;
                LastCardNumber = cardNumber;
                LastCardString = CardString[PlayDirectionNumber, cardNumber];
                TrickCardString[PlayDirectionNumber] = LastCardString;
                TrickRank[PlayDirectionNumber] = Utilities.Rank(LastCardString);
                TrickSuit[PlayDirectionNumber] = Utilities.Suit(LastCardString);
                TrickDisplayRank[PlayDirectionNumber] = Utilities.DisplayRank(LastCardString);
                TrickDisplaySuit[PlayDirectionNumber] = Utilities.DisplaySuit(LastCardString);
                if (PlayCounter % 4 == 0)  // First card in trick
                {
                    TrickLeadSuit = TrickSuit[PlayDirectionNumber];
                }
                if (PlayCounter % 4 == 3)  // Last card in trick, so find out which card won the trick...
                {
                    int winningDirectionNumber = -1;
                    string winningSuit = TrickLeadSuit;
                    int winningRank = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        string suit = Utilities.Suit(TrickCardString[i]);
                        int rank = Utilities.Rank(TrickCardString[i]);
                        if ((winningSuit == TrickLeadSuit && suit == TrickLeadSuit && rank > winningRank) || (winningSuit == ContractSuit && suit == ContractSuit && rank > winningRank))
                        {
                            winningDirectionNumber = i;
                            winningRank = rank;
                        }
                        else if (TrickLeadSuit != ContractSuit && winningSuit == TrickLeadSuit && suit == ContractSuit)
                        {
                            winningDirectionNumber = i;
                            winningRank = rank;
                            winningSuit = ContractSuit;
                        }
                    }

                    // ... and update trick counts
                    if (Direction[winningDirectionNumber] == "North" || Direction[winningDirectionNumber] == "South")
                    {
                        TricksNS++;
                    }
                    else
                    {
                        TricksEW++;
                    }
                    TrickNumber++;
                    PlayDirectionNumber = winningDirectionNumber;

                    // Reset current trick info
                    TrickLeadSuit = "";
                    for (int i = 0; i < 4; i++)
                    {
                        TrickCardString[i] = "";
                        TrickRank[i] = 0;
                        TrickSuit[i] = "";
                        TrickDisplayRank[i] = "";
                        TrickDisplaySuit[i] = "";
                    }
                }
                else
                {
                    // Not last card in trick, so move play on to next hand.  Only used if this is the last played card; otherwise overwritten
                    PlayDirectionNumber = (PlayDirectionNumber + 1) % 4;
                }
            }

            // Update table info
            table.LastPlay = new Play(Utilities.Directions[PlayDirectionNumber], LastCardNumber, LastCardString, PlayCounter);
        }
    }
}
