using System;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Bid
    {
        public string Status = "Bid";
        public string LastCallDirection {get; set;}
        public int LastBidLevel {get; set;}
        public string LastBidX { get; set; }
        public string LastBidSuit { get; set; }
        public bool Alert { get; set; }
        public string LastBidDirection { get; set; }
        public int PassCount { get; set; }
        public int BidCounter { get; set; }
        public string DisplayBid { get; set; }
        public string ToBidDirection { get; set; }

        public Bid(string lastCallDirection, int lastBidLevel, string lastBidSuit, string lastBidX, bool alert, string lastBidDirection, int passCount, int bidCounter)
        {
            LastCallDirection = lastCallDirection;
            LastBidLevel = lastBidLevel;
            LastBidX = lastBidX;
            LastBidSuit = lastBidSuit;
            Alert = alert;
            LastBidDirection = lastBidDirection;
            PassCount = passCount;
            BidCounter = bidCounter;
            DisplayBid = Utilities.DisplayBid(PassCount, LastBidX, LastBidSuit, LastBidLevel);
            if (LastCallDirection == "West")
            {
                ToBidDirection = "North";
            }
            else if (LastCallDirection == "North")
            {
                ToBidDirection = "East";
            }
            else if (LastCallDirection == "East")
            {
                ToBidDirection = "South";
            }
            else if (LastCallDirection == "South")
            {
                ToBidDirection = "West";
            }
        }

        public void UpdateDB(int sectionID, int tableNumber, int roundNumber, int boardNumber)
        {
            string dbBid;
            if (PassCount > 0)
            {
                dbBid = "PASS";
            }
            else if (LastBidX != "")
            {
                dbBid = LastBidX;
            }
            else
            {
                dbBid = LastBidLevel.ToString() + LastBidSuit;
            }

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                // Delete any spurious previously made bids
                string SQLString = $"DELETE FROM BiddingData WHERE Section={sectionID} AND [Table]={tableNumber} AND Round={roundNumber} AND Board={boardNumber} AND Counter={BidCounter}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
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

                SQLString = $"INSERT INTO BiddingData (Section, [Table], Round, Board, Counter, Direction, Bid, DateLog, TimeLog) VALUES ({sectionID}, {tableNumber}, {roundNumber}, {boardNumber}, {BidCounter}, '{LastCallDirection.Substring(0, 1)}', '{dbBid}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#)";
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

                if (BidCounter == 0)
                {
                    // Update table status in database
                    SQLString = $"UPDATE Tables SET BiddingStarted=True WHERE Section={sectionID} AND [Table]={tableNumber}";
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
}
