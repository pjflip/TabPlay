// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlay.Models
{
    public class HandsDisplay
    {
        public int DeviceNumber { get; private set; }
        public string[] Direction { get; private set; }
        public string Dealer { get; private set; }
        public string[,] CardString { get; private set; }
        public string[,] DisplayRank { get; private set; }
        public string[,] DisplaySuit { get; private set; }
        public int[,] SuitLengths { get; private set; }
        public bool Vuln02 { get; private set; }
        public bool Vuln13 { get; private set; }
        public string[] HCP { get; private set; }

        public string EvalNorthNT { get; private set; }
        public string EvalNorthSpades { get; private set; }
        public string EvalNorthHearts { get; private set; }
        public string EvalNorthDiamonds { get; private set; }
        public string EvalNorthClubs { get; private set; }
        public string EvalEastNT { get; private set; }
        public string EvalEastSpades { get; private set; }
        public string EvalEastHearts { get; private set; }
        public string EvalEastDiamonds { get; private set; }
        public string EvalEastClubs { get; private set; }
        public string EvalSouthNT { get; private set; }
        public string EvalSouthSpades { get; private set; }
        public string EvalSouthHearts { get; private set; }
        public string EvalSouthDiamonds { get; private set; }
        public string EvalSouthClubs { get; private set; }
        public string EvalWestNT { get; private set; }
        public string EvalWestSpades { get; private set; }
        public string EvalWestHearts { get; private set; }
        public string EvalWestDiamonds { get; private set; }
        public string EvalWestClubs { get; private set; }

        public HandsDisplay(int deviceNumber, Table table)
        {
            DeviceNumber = deviceNumber;
            Device device = AppData.DeviceList[deviceNumber];
            int northDirectionNumber = (4 - Utilities.DirectionToNumber(device.Direction)) % 4;

            HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == device.SectionID && x.BoardNumber == table.BoardNumber);
            if (handRecord == null)     // Can't find matching hand record, so use default SectionID = 1
            {
                handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == table.BoardNumber);
            }
            Dealer = handRecord.Dealer;

            Direction = new string[4];
            Direction[northDirectionNumber] = "North";
            Direction[(northDirectionNumber + 1) % 4] = "East";
            Direction[(northDirectionNumber + 2) % 4] = "South";
            Direction[(northDirectionNumber + 3) % 4] = "West";
            HCP = new string[4];
            HCP[northDirectionNumber] = handRecord.HCPNorth;
            HCP[(northDirectionNumber + 1) % 4] = handRecord.HCPEast;
            HCP[(northDirectionNumber + 2) % 4] = handRecord.HCPSouth;
            HCP[(northDirectionNumber + 3) % 4] = handRecord.HCPWest;
            CardString = handRecord.HandTable(northDirectionNumber, "NT");
            DisplayRank = new string[4, 13];
            DisplaySuit = new string[4, 13];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    DisplayRank[i, j] = Utilities.DisplayRank(CardString[i, j]);
                    DisplaySuit[i, j] = Utilities.DisplaySuit(CardString[i, j]);
                }
            }
            SuitLengths = handRecord.SuitLengths(northDirectionNumber, "NT");
            if (northDirectionNumber % 2 == 0)
            {
                Vuln02 = Utilities.NSVulnerability[(table.BoardNumber - 1) % 16];
                Vuln13 = Utilities.EWVulnerability[(table.BoardNumber - 1) % 16];
            }
            else
            {
                Vuln02 = Utilities.EWVulnerability[(table.BoardNumber - 1) % 16];
                Vuln13 = Utilities.NSVulnerability[(table.BoardNumber - 1) % 16];
            }

            EvalNorthNT = handRecord.EvalNorthNT;
            EvalNorthSpades = handRecord.EvalNorthSpades;
            EvalNorthHearts = handRecord.EvalNorthHearts;
            EvalNorthDiamonds = handRecord.EvalNorthDiamonds;
            EvalNorthClubs = handRecord.EvalNorthClubs;
            EvalEastNT = handRecord.EvalEastNT;
            EvalEastSpades = handRecord.EvalEastSpades;
            EvalEastHearts = handRecord.EvalEastHearts;
            EvalEastDiamonds = handRecord.EvalEastDiamonds;
            EvalEastClubs = handRecord.EvalEastClubs;
            EvalSouthNT = handRecord.EvalSouthNT;
            EvalSouthSpades = handRecord.EvalSouthSpades;
            EvalSouthHearts = handRecord.EvalSouthHearts;
            EvalSouthDiamonds = handRecord.EvalSouthDiamonds;
            EvalSouthClubs = handRecord.EvalSouthClubs;
            EvalWestNT = handRecord.EvalWestNT;
            EvalWestSpades = handRecord.EvalWestSpades;
            EvalWestHearts = handRecord.EvalWestHearts;
            EvalWestDiamonds = handRecord.EvalWestDiamonds;
            EvalWestClubs = handRecord.EvalWestClubs;
        }
    }
}
