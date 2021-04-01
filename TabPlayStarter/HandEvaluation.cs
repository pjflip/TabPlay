// TabPlay - TabPlay, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlayStarter
{
    public class HandEvaluation
    {
        public int SectionID{ get; private set;}
        public int BoardNumber{ get; private set;}

        public int NorthSpades{ get; private set;}
        public int NorthHearts{ get; private set;}
        public int NorthDiamonds{ get; private set;}
        public int NorthClubs{ get; private set;}
        public int NorthNotrump{ get; private set;}
        public int EastSpades{ get; private set;}
        public int EastHearts{ get; private set;}
        public int EastDiamonds{ get; private set;}
        public int EastClubs{ get; private set;}
        public int EastNotrump{ get; private set;}
        public int SouthSpades{ get; private set;}
        public int SouthHearts{ get; private set;}
        public int SouthDiamonds{ get; private set;}
        public int SouthClubs{ get; private set;}
        public int SouthNotrump{ get; private set;}
        public int WestSpades{ get; private set;}
        public int WestHearts{ get; private set;}
        public int WestDiamonds{ get; private set;}
        public int WestClubs{ get; private set;}
        public int WestNotrump{ get; private set;}

        public int NorthHcp{ get; private set;}
        public int SouthHcp{ get; private set;}
        public int EastHcp{ get; private set;}
        public int WestHcp{ get; private set;}

        public HandEvaluation(Hand hand)
        {
            SectionID = hand.SectionID;
            BoardNumber = hand.BoardNumber;

            int[] ddTable = new int[20];
            ddTable = DoubleDummySolver.CalcTable(hand.PBN);
            NorthSpades = ddTable[0];
            EastSpades = ddTable[1];
            SouthSpades = ddTable[2];
            WestSpades = ddTable[3];
            NorthHearts = ddTable[4];
            EastHearts = ddTable[5];
            SouthHearts = ddTable[6];
            WestHearts = ddTable[7];
            NorthDiamonds = ddTable[8];
            EastDiamonds = ddTable[9];
            SouthDiamonds = ddTable[10];
            WestDiamonds = ddTable[11];
            NorthClubs = ddTable[12];
            EastClubs = ddTable[13];
            SouthClubs = ddTable[14];
            WestClubs = ddTable[15];
            NorthNotrump = ddTable[16];
            EastNotrump = ddTable[17];
            SouthNotrump = ddTable[18];
            WestNotrump = ddTable[19];

            int northHcp = 0;
            int southHcp = 0;
            int eastHcp = 0;
            int westHcp = 0;
            if (hand.NorthSpades.Contains("A")) northHcp += 4;
            if (hand.NorthHearts.Contains("A")) northHcp += 4;
            if (hand.NorthDiamonds.Contains("A")) northHcp += 4;
            if (hand.NorthClubs.Contains("A")) northHcp += 4;
            if (hand.EastSpades.Contains("A")) eastHcp += 4;
            if (hand.EastHearts.Contains("A")) eastHcp += 4;
            if (hand.EastDiamonds.Contains("A")) eastHcp += 4;
            if (hand.EastClubs.Contains("A")) eastHcp += 4;
            if (hand.SouthSpades.Contains("A")) southHcp += 4;
            if (hand.SouthHearts.Contains("A")) southHcp += 4;
            if (hand.SouthDiamonds.Contains("A")) southHcp += 4;
            if (hand.SouthClubs.Contains("A")) southHcp += 4;
            if (hand.WestSpades.Contains("A")) westHcp += 4;
            if (hand.WestHearts.Contains("A")) westHcp += 4;
            if (hand.WestDiamonds.Contains("A")) westHcp += 4;
            if (hand.WestClubs.Contains("A")) westHcp += 4;
            if (hand.NorthSpades.Contains("K")) northHcp += 3;
            if (hand.NorthHearts.Contains("K")) northHcp += 3;
            if (hand.NorthDiamonds.Contains("K")) northHcp += 3;
            if (hand.NorthClubs.Contains("K")) northHcp += 3;
            if (hand.EastSpades.Contains("K")) eastHcp += 3;
            if (hand.EastHearts.Contains("K")) eastHcp += 3;
            if (hand.EastDiamonds.Contains("K")) eastHcp += 3;
            if (hand.EastClubs.Contains("K")) eastHcp += 3;
            if (hand.SouthSpades.Contains("K")) southHcp += 3;
            if (hand.SouthHearts.Contains("K")) southHcp += 3;
            if (hand.SouthDiamonds.Contains("K")) southHcp += 3;
            if (hand.SouthClubs.Contains("K")) southHcp += 3;
            if (hand.WestSpades.Contains("K")) westHcp += 3;
            if (hand.WestHearts.Contains("K")) westHcp += 3;
            if (hand.WestDiamonds.Contains("K")) westHcp += 3;
            if (hand.WestClubs.Contains("K")) westHcp += 3;
            if (hand.NorthSpades.Contains("Q")) northHcp += 2;
            if (hand.NorthHearts.Contains("Q")) northHcp += 2;
            if (hand.NorthDiamonds.Contains("Q")) northHcp += 2;
            if (hand.NorthClubs.Contains("Q")) northHcp += 2;
            if (hand.EastSpades.Contains("Q")) eastHcp += 2;
            if (hand.EastHearts.Contains("Q")) eastHcp += 2;
            if (hand.EastDiamonds.Contains("Q")) eastHcp += 2;
            if (hand.EastClubs.Contains("Q")) eastHcp += 2;
            if (hand.SouthSpades.Contains("Q")) southHcp += 2;
            if (hand.SouthHearts.Contains("Q")) southHcp += 2;
            if (hand.SouthDiamonds.Contains("Q")) southHcp += 2;
            if (hand.SouthClubs.Contains("Q")) southHcp += 2;
            if (hand.WestSpades.Contains("Q")) westHcp += 2;
            if (hand.WestHearts.Contains("Q")) westHcp += 2;
            if (hand.WestDiamonds.Contains("Q")) westHcp += 2;
            if (hand.WestClubs.Contains("Q")) westHcp += 2;
            if (hand.NorthSpades.Contains("J")) northHcp += 1;
            if (hand.NorthHearts.Contains("J")) northHcp += 1;
            if (hand.NorthDiamonds.Contains("J")) northHcp += 1;
            if (hand.NorthClubs.Contains("J")) northHcp += 1;
            if (hand.EastSpades.Contains("J")) eastHcp += 1;
            if (hand.EastHearts.Contains("J")) eastHcp += 1;
            if (hand.EastDiamonds.Contains("J")) eastHcp += 1;
            if (hand.EastClubs.Contains("J")) eastHcp += 1;
            if (hand.SouthSpades.Contains("J")) southHcp += 1;
            if (hand.SouthHearts.Contains("J")) southHcp += 1;
            if (hand.SouthDiamonds.Contains("J")) southHcp += 1;
            if (hand.SouthClubs.Contains("J")) southHcp += 1;
            if (hand.WestSpades.Contains("J")) westHcp += 1;
            if (hand.WestHearts.Contains("J")) westHcp += 1;
            if (hand.WestDiamonds.Contains("J")) westHcp += 1;
            if (hand.WestClubs.Contains("J")) westHcp += 1;
            NorthHcp = northHcp;
            EastHcp = eastHcp;
            SouthHcp = southHcp;
            WestHcp = westHcp;
        }
    }
}
