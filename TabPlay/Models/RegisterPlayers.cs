// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Data.Odbc;

namespace TabPlay.Models
{
    public class RegisterPlayers
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; set; }
        public int RoundNumber { get; private set; }
        public int BoardNumber { get; private set; }
        public string[] Direction { get; private set; }
        public int[] PairNumber { get; set; }
        public string[] PlayerName { get; private set; }
        public bool[] Registered { get; set; }
        public bool PlayerNumberEntry { get; private set; }
        public string PairOrPlayer { get; private set; }
        public int PollInterval { get; private set; }

        public RegisterPlayers(TableStatus tableStatus, string direction)
        {
            SectionID = tableStatus.SectionID;
            TableNumber = tableStatus.TableNumber;
            RoundNumber = tableStatus.RoundNumber;
            BoardNumber = tableStatus.BoardNumber;
            Direction = new string[4];
            PairNumber = new int[4];
            PlayerName = new string[4];
            Registered = new bool[4];
            PollInterval = Settings.PollInterval;
            PlayerNumberEntry = (RoundNumber == 1 || Settings.NumberEntryEachRound) && (BoardNumber == tableStatus.LowBoard);
            if (AppData.IsIndividual) PairOrPlayer = "Player"; else PairOrPlayer = "Pair";

            int directionNumber = Utilities.DirectionToNumber(direction);

            // All directionNumbers are relative to the direction that is 0, so we need to know which directionNumber is North
            int northDirectionNumber = (4 - directionNumber) % 4;
            Direction[northDirectionNumber] = "North";
            Direction[(northDirectionNumber + 1) % 4] = "East";
            Direction[(northDirectionNumber + 2) % 4] = "South";
            Direction[(northDirectionNumber + 3) % 4] = "West";

            for (int i = 0; i < 4; i++)
            {
                // Convert from absolute (North=0) to relative (my direction=0) direction numbers
                Registered[i] = tableStatus.Registered[(directionNumber + i) % 4];
                PlayerName[i] = tableStatus.PlayerName[(directionNumber + i) % 4];
                PairNumber[i] = tableStatus.PairNumber[(directionNumber + i) % 4];
            }
            return;
        }
    }
}