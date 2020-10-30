using System;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class TableStatus
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; private set; }
        public string Direction { get; set; }
        public int RoundNumber { get; set; }
        public int BoardNumber { get; set; }
        public bool NSExists { get; private set; }
        public bool EWExists { get; private set; }
        public bool BiddingStarted { get; set; }
        public bool BiddingComplete { get; set; }
        public bool PlayComplete { get; set; }
        public bool RoundComplete { get; set; }
        public bool[] Registered { get; set; }
        public string[] PlayerName { get; set; }
        public DateTime[] UpdateTime { get; set; }
        public Bid LastBid { get; set; }
        public Play LastPlay { get; set; }
        public bool ClaimExpose { get; set; }
        public string ClaimDirection { get; set; }

        public TableStatus(int sectionID, int tableNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            RoundNumber = 1;
            BiddingComplete = false;
            BiddingStarted = false;
            PlayComplete = false;
            RoundComplete = false;
            LastBid = new Bid("", 0, "", "", false, "", 0, -1);
            LastPlay = new Play("", 0, "", -999);
            ClaimExpose = false;

            // Direction numbers in TableStatus are absolute (North=0)
            PlayerName = new string[4] { "", "", "", "" };
            Registered = new bool[4] { false, false, false, false };
            UpdateTime = new DateTime[4] { DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now };
            int pairNS = 0;
            int pairEW = 0;

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                object queryResult = null;
                connection.Open();

                // Get the status of this table
                string SQLString = $"SELECT CurrentRound, CurrentBoard, BiddingStarted, BiddingComplete, PlayComplete FROM Tables WHERE Section={sectionID} AND [Table]={tableNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            queryResult = reader.GetValue(0);
                            if (queryResult != DBNull.Value && queryResult != null)
                            {
                                RoundNumber = Convert.ToInt32(queryResult);
                            }
                            queryResult = reader.GetValue(1);
                            if (queryResult != DBNull.Value && queryResult != null)
                            {
                                BoardNumber = Convert.ToInt32(queryResult);
                            }
                            BiddingStarted = reader.GetBoolean(2);
                            BiddingComplete = reader.GetBoolean(3);
                            PlayComplete = reader.GetBoolean(4);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }

                // Ensure this table is recorded as logged on
                SQLString = $"UPDATE Tables SET LogOnOff=1 WHERE Section={sectionID} AND [Table]={tableNumber}";
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

                // Get the pair numbers for this round
                SQLString = $"SELECT NSPair, EWPair FROM RoundData WHERE Section={sectionID} AND [Table]={tableNumber} AND Round={RoundNumber}";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            pairNS = reader.GetInt32(0);
                            pairEW = reader.GetInt32(1);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
                NSExists = (pairNS != 0);
                EWExists = (pairEW != 0);
            }
        }

        public void Reset(Round round, string direction)
        {
            int directionNumber = Utilities.DirectionToNumber(direction);
            Registered[directionNumber] = true;
            UpdateTime[directionNumber] = DateTime.Now;
            for (int i = 0; i < 4; i++)
            {
                // Convert from absolute (North=0) to relative (my direction=0) direction numbers
                round.Registered[i] = Registered[(directionNumber + i) % 4];
                PlayerName[(directionNumber + i) % 4] = round.PlayerName[i];
            }

            // Reset TableStatus for next board, if not done yet
            if (BiddingStarted == true)
            {
                BiddingStarted = false;
                BiddingComplete = false;
                PlayComplete = false;
                RoundComplete = false;
                LastBid = new Bid("", 0, "", "", false, "", 0, -1);
                LastPlay = new Play("", 0, "", -999);
                ClaimExpose = false;
                ClaimDirection = "";
                NSExists = (round.PairNS != 0);
                EWExists = (round.PairEW != 0);
            }
        }
    }
}