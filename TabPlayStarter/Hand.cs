// TabPlay - TabPlay, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Text;

namespace TabPlayStarter
{
    public class Hand
    {
        public int SectionID { get; set; }
        public int BoardNumber { get; set;}

        public string NorthSpades { get; set;}
        public string NorthHearts { get; set;}
        public string NorthDiamonds { get; set;}
        public string NorthClubs { get; set;}
        public string EastSpades { get; set;}
        public string EastHearts { get; set;}
        public string EastDiamonds { get; set;}
        public string EastClubs { get; set;}
        public string SouthSpades { get; set;}
        public string SouthHearts { get; set;}
        public string SouthDiamonds { get; set;}
        public string SouthClubs { get; set;}
        public string WestSpades { get; set;}
        public string WestHearts { get; set;}
        public string WestDiamonds { get; set;}
        public string WestClubs { get; set;}

        public string PBN
        {
            get
            {
                StringBuilder PBNString = new StringBuilder();
                switch ((BoardNumber - 1) % 4)
                {
                    case 0:
                        PBNString.Append("N:");
                        PBNString.Append(NorthSpades);
                        PBNString.Append(".");
                        PBNString.Append(NorthHearts);
                        PBNString.Append(".");
                        PBNString.Append(NorthDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(NorthClubs);
                        PBNString.Append(" ");
                        PBNString.Append(EastSpades);
                        PBNString.Append(".");
                        PBNString.Append(EastHearts);
                        PBNString.Append(".");
                        PBNString.Append(EastDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(EastClubs);
                        PBNString.Append(" ");
                        PBNString.Append(SouthSpades);
                        PBNString.Append(".");
                        PBNString.Append(SouthHearts);
                        PBNString.Append(".");
                        PBNString.Append(SouthDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(SouthClubs);
                        PBNString.Append(" ");
                        PBNString.Append(WestSpades);
                        PBNString.Append(".");
                        PBNString.Append(WestHearts);
                        PBNString.Append(".");
                        PBNString.Append(WestDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(WestClubs);
                        break;
                    case 1:
                        PBNString.Append("E:");
                        PBNString.Append(EastSpades);
                        PBNString.Append(".");
                        PBNString.Append(EastHearts);
                        PBNString.Append(".");
                        PBNString.Append(EastDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(EastClubs);
                        PBNString.Append(" ");
                        PBNString.Append(SouthSpades);
                        PBNString.Append(".");
                        PBNString.Append(SouthHearts);
                        PBNString.Append(".");
                        PBNString.Append(SouthDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(SouthClubs);
                        PBNString.Append(" ");
                        PBNString.Append(WestSpades);
                        PBNString.Append(".");
                        PBNString.Append(WestHearts);
                        PBNString.Append(".");
                        PBNString.Append(WestDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(WestClubs);
                        PBNString.Append(" ");
                        PBNString.Append(NorthSpades);
                        PBNString.Append(".");
                        PBNString.Append(NorthHearts);
                        PBNString.Append(".");
                        PBNString.Append(NorthDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(NorthClubs);
                        break;
                    case 2:
                        PBNString.Append("S:");
                        PBNString.Append(SouthSpades);
                        PBNString.Append(".");
                        PBNString.Append(SouthHearts);
                        PBNString.Append(".");
                        PBNString.Append(SouthDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(SouthClubs);
                        PBNString.Append(" ");
                        PBNString.Append(WestSpades);
                        PBNString.Append(".");
                        PBNString.Append(WestHearts);
                        PBNString.Append(".");
                        PBNString.Append(WestDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(WestClubs);
                        PBNString.Append(" ");
                        PBNString.Append(NorthSpades);
                        PBNString.Append(".");
                        PBNString.Append(NorthHearts);
                        PBNString.Append(".");
                        PBNString.Append(NorthDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(NorthClubs);
                        PBNString.Append(" ");
                        PBNString.Append(EastSpades);
                        PBNString.Append(".");
                        PBNString.Append(EastHearts);
                        PBNString.Append(".");
                        PBNString.Append(EastDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(EastClubs);
                        break;
                    case 3:
                        PBNString.Append("W:");
                        PBNString.Append(WestSpades);
                        PBNString.Append(".");
                        PBNString.Append(WestHearts);
                        PBNString.Append(".");
                        PBNString.Append(WestDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(WestClubs);
                        PBNString.Append(" ");
                        PBNString.Append(NorthSpades);
                        PBNString.Append(".");
                        PBNString.Append(NorthHearts);
                        PBNString.Append(".");
                        PBNString.Append(NorthDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(NorthClubs);
                        PBNString.Append(" ");
                        PBNString.Append(EastSpades);
                        PBNString.Append(".");
                        PBNString.Append(EastHearts);
                        PBNString.Append(".");
                        PBNString.Append(EastDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(EastClubs);
                        PBNString.Append(" ");
                        PBNString.Append(SouthSpades);
                        PBNString.Append(".");
                        PBNString.Append(SouthHearts);
                        PBNString.Append(".");
                        PBNString.Append(SouthDiamonds);
                        PBNString.Append(".");
                        PBNString.Append(SouthClubs);
                        break;
                }
                return PBNString.ToString();
            }

            set
            {
                char[] PBNDelimiter = { ':', '.', ' ' };
                string[] PBNArray = value.Split(PBNDelimiter);
                switch (PBNArray[0])
                {
                    case "N":
                        NorthSpades = PBNArray[1];
                        NorthHearts = PBNArray[2];
                        NorthDiamonds = PBNArray[3];
                        NorthClubs = PBNArray[4];
                        EastSpades = PBNArray[5];
                        EastHearts = PBNArray[6];
                        EastDiamonds = PBNArray[7];
                        EastClubs = PBNArray[8];
                        SouthSpades = PBNArray[9];
                        SouthHearts = PBNArray[10];
                        SouthDiamonds = PBNArray[11];
                        SouthClubs = PBNArray[12];
                        WestSpades = PBNArray[13];
                        WestHearts = PBNArray[14];
                        WestDiamonds = PBNArray[15];
                        WestClubs = PBNArray[16];
                        break;
                    case "E":
                        EastSpades = PBNArray[1];
                        EastHearts = PBNArray[2];
                        EastDiamonds = PBNArray[3];
                        EastClubs = PBNArray[4];
                        SouthSpades = PBNArray[5];
                        SouthHearts = PBNArray[6];
                        SouthDiamonds = PBNArray[7];
                        SouthClubs = PBNArray[8];
                        WestSpades = PBNArray[9];
                        WestHearts = PBNArray[10];
                        WestDiamonds = PBNArray[11];
                        WestClubs = PBNArray[12];
                        NorthSpades = PBNArray[13];
                        NorthHearts = PBNArray[14];
                        NorthDiamonds = PBNArray[15];
                        NorthClubs = PBNArray[16];
                        break;
                    case "S":
                        SouthSpades = PBNArray[1];
                        SouthHearts = PBNArray[2];
                        SouthDiamonds = PBNArray[3];
                        SouthClubs = PBNArray[4];
                        WestSpades = PBNArray[5];
                        WestHearts = PBNArray[6];
                        WestDiamonds = PBNArray[7];
                        WestClubs = PBNArray[8];
                        NorthSpades = PBNArray[9];
                        NorthHearts = PBNArray[10];
                        NorthDiamonds = PBNArray[11];
                        NorthClubs = PBNArray[12];
                        EastSpades = PBNArray[13];
                        EastHearts = PBNArray[14];
                        EastDiamonds = PBNArray[15];
                        EastClubs = PBNArray[16];
                        break;
                    case "W":
                        WestSpades = PBNArray[1];
                        WestHearts = PBNArray[2];
                        WestDiamonds = PBNArray[3];
                        WestClubs = PBNArray[4];
                        NorthSpades = PBNArray[5];
                        NorthHearts = PBNArray[6];
                        NorthDiamonds = PBNArray[7];
                        NorthClubs = PBNArray[8];
                        EastSpades = PBNArray[9];
                        EastHearts = PBNArray[10];
                        EastDiamonds = PBNArray[11];
                        EastClubs = PBNArray[12];
                        SouthSpades = PBNArray[13];
                        SouthHearts = PBNArray[14];
                        SouthDiamonds = PBNArray[15];
                        SouthClubs = PBNArray[16];
                        break;
                }
            }
        }
    }
}
