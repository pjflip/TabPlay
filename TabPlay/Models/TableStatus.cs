using System;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class TableStatus
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; private set; }
        public int RoundNumber { get; set; } = 1;
        public int BoardNumber { get; set; } = 0;
        public int LowBoard { get; set; }
        public int HighBoard { get; set; }
        public bool BiddingStarted { get; set; }
        public bool BiddingComplete { get; set; }
        public bool PlayComplete { get; set; }
        public bool[] ReadyForNextRound { get; set; } = { false, false, false, false };
        public bool[] Registered { get; set; } = { false, false, false, false };
        public string[] PlayerName { get; set; } = { "", "", "", "" };
        public int[] PairNumber { get; set; } = { 0, 0, 0, 0 };
        public DateTime[] UpdateTime { get; set; } = { DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now };
        public Bid LastBid { get; set; }
        public Play LastPlay { get; set; }
        public bool ClaimExpose { get; set; } = false;
        public string ClaimDirection { get; set; }

        public TableStatus(int sectionID, int tableNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            LastBid = new Bid("", 0, "", "", false, "", 0, -1);
            LastPlay = new Play("", 0, "", -999);

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
                        if (reader.Read())
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

                // Get the pair numbers and boards for this round
                GetRoundData(connection);
            }
            // Check for invalid board number and set accordingly
            if (BoardNumber < LowBoard || BoardNumber > HighBoard) BoardNumber = LowBoard;
        }

        public void NewBoard(int boardNumber)
        {
            BoardNumber = boardNumber;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                ResetStatus(connection);
            }
        }

        public void NewRound(int roundNumber)
        {
            RoundNumber = roundNumber;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                GetRoundData(connection);
                BoardNumber = LowBoard;
                ResetStatus(connection);
                ReadyForNextRound = new bool[] { false, false, false, false };
            }
        }

        private void GetRoundData(OdbcConnection connection)
        {
            Utilities.CheckTabPlayPairNos(connection);
            if (AppData.IsIndividual)
            {
                string SQLString = $"SELECT NSPair, EWPair, South, West, LowBoard, HighBoard FROM RoundData WHERE Section={SectionID} AND Table={TableNumber} AND Round={RoundNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            PairNumber[0] = reader.GetInt32(0);
                            PairNumber[1] = reader.GetInt32(1);
                            PairNumber[2] = reader.GetInt32(2);
                            PairNumber[3] = reader.GetInt32(3);
                            LowBoard = reader.GetInt32(4);
                            HighBoard = reader.GetInt32(5);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
                for (int i = 0; i < 4; i++)
                {
                    PlayerName[i] = Utilities.GetNameFromPlayerNumbersTableIndividual(connection, SectionID, RoundNumber, PairNumber[i]);
                }
            }
            else  // Not individual
            {
                string SQLString = $"SELECT NSPair, EWPair, LowBoard, HighBoard FROM RoundData WHERE Section={SectionID} AND Table={TableNumber} AND Round={RoundNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            PairNumber[0] = PairNumber[2] = reader.GetInt32(0);
                            PairNumber[1] = PairNumber[3] = reader.GetInt32(1);
                            LowBoard = reader.GetInt32(2);
                            HighBoard = reader.GetInt32(3);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
                PlayerName[0] = Utilities.GetNameFromPlayerNumbersTable(connection, SectionID, RoundNumber, PairNumber[0], "North");
                PlayerName[1] = Utilities.GetNameFromPlayerNumbersTable(connection, SectionID, RoundNumber, PairNumber[1], "East");
                PlayerName[2] = Utilities.GetNameFromPlayerNumbersTable(connection, SectionID, RoundNumber, PairNumber[2], "South");
                PlayerName[3] = Utilities.GetNameFromPlayerNumbersTable(connection, SectionID, RoundNumber, PairNumber[3], "West");
            }
        }

        private void ResetStatus(OdbcConnection connection)
        {
            BiddingStarted = false;
            BiddingComplete = false;
            PlayComplete = false;
            LastBid = new Bid("", 0, "", "", false, "", 0, -1);
            LastPlay = new Play("", 0, "", -999);
            ClaimExpose = false;
            ClaimDirection = "";

            string SQLString = $"UPDATE Tables SET CurrentRound={RoundNumber}, CurrentBoard={BoardNumber}, BiddingStarted=False, BiddingComplete=False, PlayComplete=False WHERE Section={SectionID} AND [Table]={TableNumber}";
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
        }
    }
}