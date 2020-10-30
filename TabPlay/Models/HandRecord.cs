// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlay.Models
{
    public class HandRecord
    {
        public int SectionID {get; set;}
        public int BoardNumber {get; set;}
        public string Dealer {get; set;}

        public string NorthSpades {get; set;}
        public string NorthHearts {get; set;}
        public string NorthDiamonds {get; set;}
        public string NorthClubs {get; set;}
        public string EastSpades {get; set;}
        public string EastHearts {get; set;}
        public string EastDiamonds {get; set;}
        public string EastClubs {get; set;}
        public string SouthSpades {get; set;}
        public string SouthHearts {get; set;}
        public string SouthDiamonds {get; set;}
        public string SouthClubs {get; set;}
        public string WestSpades {get; set;}
        public string WestHearts {get; set;}
        public string WestDiamonds {get; set;}
        public string WestClubs {get; set;}

        public string EvalNorthNT {get; set;}
        public string EvalNorthSpades {get; set;}
        public string EvalNorthHearts {get; set;}
        public string EvalNorthDiamonds {get; set;}
        public string EvalNorthClubs {get; set;}
        public string EvalEastNT {get; set;}
        public string EvalEastSpades {get; set;}
        public string EvalEastHearts {get; set;}
        public string EvalEastDiamonds {get; set;}
        public string EvalEastClubs {get; set;}
        public string EvalSouthNT {get; set;}
        public string EvalSouthSpades {get; set;}
        public string EvalSouthHearts {get; set;}
        public string EvalSouthDiamonds {get; set;}
        public string EvalSouthClubs {get; set;}
        public string EvalWestNT { get; set; }
        public string EvalWestSpades {get; set;}
        public string EvalWestHearts {get; set;}
        public string EvalWestDiamonds {get; set;}
        public string EvalWestClubs {get; set;}

        public string HCPNorth {get; set;}
        public string HCPSouth {get; set;}
        public string HCPEast {get; set;}
        public string HCPWest {get; set;}

        public string[,] HandTable(int northDirectionNumber, string trumps = "NT")
        {
            string[,] handTable = new string[4, 13];
            for (int iHand = 0; iHand < 4; iHand++)
            {
                int dir = northDirectionNumber;
                int iCard = 0;
                if (trumps == "NT" || trumps == "S")
                {
                    for (int i = 0; i < NorthSpades.Length; i++)
                    {
                        handTable[dir, iCard] = NorthSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < NorthHearts.Length; i++)
                    {
                        handTable[dir, iCard] = NorthHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < NorthClubs.Length; i++)
                    {
                        handTable[dir, iCard] = NorthClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                    for (int i = 0; i < NorthDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = NorthDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                }
                else if (trumps == "H")
                {
                    for (int i = 0; i < NorthHearts.Length; i++)
                    {
                        handTable[dir, iCard] = NorthHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < NorthSpades.Length; i++)
                    {
                        handTable[dir, iCard] = NorthSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < NorthDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = NorthDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                    for (int i = 0; i < NorthClubs.Length; i++)
                    {
                        handTable[dir, iCard] = NorthClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                }
                else if (trumps == "D")
                {
                    for (int i = 0; i < NorthDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = NorthDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                    for (int i = 0; i < NorthSpades.Length; i++)
                    {
                        handTable[dir, iCard] = NorthSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < NorthHearts.Length; i++)
                    {
                        handTable[dir, iCard] = NorthHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < NorthClubs.Length; i++)
                    {
                        handTable[dir, iCard] = NorthClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                }
                else if (trumps == "C")
                {
                    for (int i = 0; i < NorthClubs.Length; i++)
                    {
                        handTable[dir, iCard] = NorthClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                    for (int i = 0; i < NorthHearts.Length; i++)
                    {
                        handTable[dir, iCard] = NorthHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < NorthSpades.Length; i++)
                    {
                        handTable[dir, iCard] = NorthSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < NorthDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = NorthDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                }
                dir = (dir + 1) % 4;  // Change to East
                iCard = 0;
                if (trumps == "NT" || trumps == "S")
                {
                    for (int i = 0; i < EastSpades.Length; i++)
                    {
                        handTable[dir, iCard] = EastSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < EastHearts.Length; i++)
                    {
                        handTable[dir, iCard] = EastHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < EastClubs.Length; i++)
                    {
                        handTable[dir, iCard] = EastClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                    for (int i = 0; i < EastDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = EastDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                }
                else if (trumps == "H")
                {
                    for (int i = 0; i < EastHearts.Length; i++)
                    {
                        handTable[dir, iCard] = EastHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < EastSpades.Length; i++)
                    {
                        handTable[dir, iCard] = EastSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < EastDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = EastDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                    for (int i = 0; i < EastClubs.Length; i++)
                    {
                        handTable[dir, iCard] = EastClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                }
                else if (trumps == "D")
                {
                    for (int i = 0; i < EastDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = EastDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                    for (int i = 0; i < EastSpades.Length; i++)
                    {
                        handTable[dir, iCard] = EastSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < EastHearts.Length; i++)
                    {
                        handTable[dir, iCard] = EastHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < EastClubs.Length; i++)
                    {
                        handTable[dir, iCard] = EastClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                }
                else if (trumps == "C")
                {
                    for (int i = 0; i < EastClubs.Length; i++)
                    {
                        handTable[dir, iCard] = EastClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                    for (int i = 0; i < EastHearts.Length; i++)
                    {
                        handTable[dir, iCard] = EastHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < EastSpades.Length; i++)
                    {
                        handTable[dir, iCard] = EastSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < EastDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = EastDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                }
                dir = (dir + 1) % 4;  // Change to South
                iCard = 0;
                if (trumps == "NT" || trumps == "S")
                {
                    for (int i = 0; i < SouthSpades.Length; i++)
                    {
                        handTable[dir, iCard] = SouthSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < SouthHearts.Length; i++)
                    {
                        handTable[dir, iCard] = SouthHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < SouthClubs.Length; i++)
                    {
                        handTable[dir, iCard] = SouthClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                    for (int i = 0; i < SouthDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = SouthDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                }
                else if (trumps == "H")
                {
                    for (int i = 0; i < SouthHearts.Length; i++)
                    {
                        handTable[dir, iCard] = SouthHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < SouthSpades.Length; i++)
                    {
                        handTable[dir, iCard] = SouthSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < SouthDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = SouthDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                    for (int i = 0; i < SouthClubs.Length; i++)
                    {
                        handTable[dir, iCard] = SouthClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                }
                else if (trumps == "D")
                {
                    for (int i = 0; i < SouthDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = SouthDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                    for (int i = 0; i < SouthSpades.Length; i++)
                    {
                        handTable[dir, iCard] = SouthSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < SouthHearts.Length; i++)
                    {
                        handTable[dir, iCard] = SouthHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < SouthClubs.Length; i++)
                    {
                        handTable[dir, iCard] = SouthClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                }
                else if (trumps == "C")
                {
                    for (int i = 0; i < SouthClubs.Length; i++)
                    {
                        handTable[dir, iCard] = SouthClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                    for (int i = 0; i < SouthHearts.Length; i++)
                    {
                        handTable[dir, iCard] = SouthHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < SouthSpades.Length; i++)
                    {
                        handTable[dir, iCard] = SouthSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < SouthDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = SouthDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                }
                dir = (dir + 1) % 4;  // Change to West
                iCard = 0;
                if (trumps == "NT" || trumps == "S")
                {
                    for (int i = 0; i < WestSpades.Length; i++)
                    {
                        handTable[dir, iCard] = WestSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < WestHearts.Length; i++)
                    {
                        handTable[dir, iCard] = WestHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < WestClubs.Length; i++)
                    {
                        handTable[dir, iCard] = WestClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                    for (int i = 0; i < WestDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = WestDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                }
                else if (trumps == "H")
                {
                    for (int i = 0; i < WestHearts.Length; i++)
                    {
                        handTable[dir, iCard] = WestHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < WestSpades.Length; i++)
                    {
                        handTable[dir, iCard] = WestSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < WestDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = WestDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                    for (int i = 0; i < WestClubs.Length; i++)
                    {
                        handTable[dir, iCard] = WestClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                }
                else if (trumps == "D")
                {
                    for (int i = 0; i < WestDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = WestDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                    for (int i = 0; i < WestSpades.Length; i++)
                    {
                        handTable[dir, iCard] = WestSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < WestHearts.Length; i++)
                    {
                        handTable[dir, iCard] = WestHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < WestClubs.Length; i++)
                    {
                        handTable[dir, iCard] = WestClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                }
                else if (trumps == "C")
                {
                    for (int i = 0; i < WestClubs.Length; i++)
                    {
                        handTable[dir, iCard] = WestClubs.Substring(i, 1) + "C";
                        iCard++;
                    }
                    for (int i = 0; i < WestHearts.Length; i++)
                    {
                        handTable[dir, iCard] = WestHearts.Substring(i, 1) + "H";
                        iCard++;
                    }
                    for (int i = 0; i < WestSpades.Length; i++)
                    {
                        handTable[dir, iCard] = WestSpades.Substring(i, 1) + "S";
                        iCard++;
                    }
                    for (int i = 0; i < WestDiamonds.Length; i++)
                    {
                        handTable[dir, iCard] = WestDiamonds.Substring(i, 1) + "D";
                        iCard++;
                    }
                }
            }
            return handTable;
        }

        public string[] HandRow(string direction)
        {
            string[] handRow = new string[13];
            int iCard = 0;
            if (direction == "North")
            {
                for (int i = 0; i < NorthSpades.Length; i++)
                {
                    handRow[iCard] = NorthSpades.Substring(i, 1) + "S";
                    iCard++;
                }
                for (int i = 0; i < NorthHearts.Length; i++)
                {
                    handRow[iCard] = NorthHearts.Substring(i, 1) + "H";
                    iCard++;
                }
                for (int i = 0; i < NorthClubs.Length; i++)
                {
                    handRow[iCard] = NorthClubs.Substring(i, 1) + "C";
                    iCard++;
                }
                for (int i = 0; i < NorthDiamonds.Length; i++)
                {
                    handRow[iCard] = NorthDiamonds.Substring(i, 1) + "D";
                    iCard++;
                }
            }
            else if (direction == "East")
            {
                for (int i = 0; i < EastSpades.Length; i++)
                {
                    handRow[iCard] = EastSpades.Substring(i, 1) + "S";
                    iCard++;
                }
                for (int i = 0; i < EastHearts.Length; i++)
                {
                    handRow[iCard] = EastHearts.Substring(i, 1) + "H";
                    iCard++;
                }
                for (int i = 0; i < EastClubs.Length; i++)
                {
                    handRow[iCard] = EastClubs.Substring(i, 1) + "C";
                    iCard++;
                }
                for (int i = 0; i < EastDiamonds.Length; i++)
                {
                    handRow[iCard] = EastDiamonds.Substring(i, 1) + "D";
                    iCard++;
                }
            }
            else if (direction == "South")
            {
                for (int i = 0; i < SouthSpades.Length; i++)
                {
                    handRow[iCard] = SouthSpades.Substring(i, 1) + "S";
                    iCard++;
                }
                for (int i = 0; i < SouthHearts.Length; i++)
                {
                    handRow[iCard] = SouthHearts.Substring(i, 1) + "H";
                    iCard++;
                }
                for (int i = 0; i < SouthClubs.Length; i++)
                {
                    handRow[iCard] = SouthClubs.Substring(i, 1) + "C";
                    iCard++;
                }
                for (int i = 0; i < SouthDiamonds.Length; i++)
                {
                    handRow[iCard] = SouthDiamonds.Substring(i, 1) + "D";
                    iCard++;
                }
            }
            else if (direction == "West")
            {
                for (int i = 0; i < WestSpades.Length; i++)
                {
                    handRow[iCard] = WestSpades.Substring(i, 1) + "S";
                    iCard++;
                }
                for (int i = 0; i < WestHearts.Length; i++)
                {
                    handRow[iCard] = WestHearts.Substring(i, 1) + "H";
                    iCard++;
                }
                for (int i = 0; i < WestClubs.Length; i++)
                {
                    handRow[iCard] = WestClubs.Substring(i, 1) + "C";
                    iCard++;
                }
                for (int i = 0; i < WestDiamonds.Length; i++)
                {
                    handRow[iCard] = WestDiamonds.Substring(i, 1) + "D";
                    iCard++;
                }
            }
            return handRow;
        }

        public int[,] SuitLengths(int northDirectionNumber, string trumps = "NT")
        {
            int[,] lengths = new int[4, 4];
            int dir = northDirectionNumber;
            if (trumps == "NT" || trumps == "S")
            {
                lengths[dir, 0] = NorthSpades.Length;
                lengths[dir, 1] = NorthHearts.Length;
                lengths[dir, 2] = NorthClubs.Length;
                lengths[dir, 3] = NorthDiamonds.Length;
            }
            else if (trumps == "H")
            {
                lengths[dir, 0] = NorthHearts.Length;
                lengths[dir, 1] = NorthSpades.Length;
                lengths[dir, 2] = NorthDiamonds.Length;
                lengths[dir, 3] = NorthClubs.Length;
            }
            else if (trumps == "D")
            {
                lengths[dir, 0] = NorthDiamonds.Length;
                lengths[dir, 1] = NorthSpades.Length;
                lengths[dir, 2] = NorthHearts.Length;
                lengths[dir, 3] = NorthClubs.Length;
            }
            else if (trumps == "C")
            {
                lengths[dir, 0] = NorthClubs.Length;
                lengths[dir, 1] = NorthHearts.Length;
                lengths[dir, 2] = NorthSpades.Length;
                lengths[dir, 3] = NorthDiamonds.Length;
            }
            dir = (dir + 1) % 4;  // Set to East
            if (trumps == "NT" || trumps == "S")
            {
                lengths[dir, 0] = EastSpades.Length;
                lengths[dir, 1] = EastHearts.Length;
                lengths[dir, 2] = EastClubs.Length;
                lengths[dir, 3] = EastDiamonds.Length;
            }
            else if (trumps == "H")
            {
                lengths[dir, 0] = EastHearts.Length;
                lengths[dir, 1] = EastSpades.Length;
                lengths[dir, 2] = EastDiamonds.Length;
                lengths[dir, 3] = EastClubs.Length;
            }
            else if (trumps == "D")
            {
                lengths[dir, 0] = EastDiamonds.Length;
                lengths[dir, 1] = EastSpades.Length;
                lengths[dir, 2] = EastHearts.Length;
                lengths[dir, 3] = EastClubs.Length;
            }
            else if (trumps == "C")
            {
                lengths[dir, 0] = EastClubs.Length;
                lengths[dir, 1] = EastHearts.Length;
                lengths[dir, 2] = EastSpades.Length;
                lengths[dir, 3] = EastDiamonds.Length;
            }
            dir = (dir + 1) % 4;  // Set to South
            if (trumps == "NT" || trumps == "S")
            {
                lengths[dir, 0] = SouthSpades.Length;
                lengths[dir, 1] = SouthHearts.Length;
                lengths[dir, 2] = SouthClubs.Length;
                lengths[dir, 3] = SouthDiamonds.Length;
            }
            else if (trumps == "H")
            {
                lengths[dir, 0] = SouthHearts.Length;
                lengths[dir, 1] = SouthSpades.Length;
                lengths[dir, 2] = SouthDiamonds.Length;
                lengths[dir, 3] = SouthClubs.Length;
            }
            else if (trumps == "D")
            {
                lengths[dir, 0] = SouthDiamonds.Length;
                lengths[dir, 1] = SouthSpades.Length;
                lengths[dir, 2] = SouthHearts.Length;
                lengths[dir, 3] = SouthClubs.Length;
            }
            else if (trumps == "C")
            {
                lengths[dir, 0] = SouthClubs.Length;
                lengths[dir, 1] = SouthHearts.Length;
                lengths[dir, 2] = SouthSpades.Length;
                lengths[dir, 3] = SouthDiamonds.Length;
            }
            dir = (dir + 1) % 4;  // Set to West
            if (trumps == "NT" || trumps == "S")
            {
                lengths[dir, 0] = WestSpades.Length;
                lengths[dir, 1] = WestHearts.Length;
                lengths[dir, 2] = WestClubs.Length;
                lengths[dir, 3] = WestDiamonds.Length;
            }
            else if (trumps == "H")
            {
                lengths[dir, 0] = WestHearts.Length;
                lengths[dir, 1] = WestSpades.Length;
                lengths[dir, 2] = WestDiamonds.Length;
                lengths[dir, 3] = WestClubs.Length;
            }
            else if (trumps == "D")
            {
                lengths[dir, 0] = WestDiamonds.Length;
                lengths[dir, 1] = WestSpades.Length;
                lengths[dir, 2] = WestHearts.Length;
                lengths[dir, 3] = WestClubs.Length;
            }
            else if (trumps == "C")
            {
                lengths[dir, 0] = WestClubs.Length;
                lengths[dir, 1] = WestHearts.Length;
                lengths[dir, 2] = WestSpades.Length;
                lengths[dir, 3] = WestDiamonds.Length;
            }
            return lengths;
        }
    }
}