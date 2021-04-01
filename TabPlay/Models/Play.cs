using System;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Play
    {
        public string Status { get; set; } = "Play";
        public string PlayDirection { get; set; }
        public int CardNumber { get; set; }
        public string CardString { get; set; }
        public int PlayCounter { get; set; }

        public Play(string playDirection, int cardNumber, string cardString, int playCounter)
        {
            PlayDirection = playDirection;
            CardNumber = cardNumber;
            CardString = cardString;
            PlayCounter = playCounter;
        }

        public void UpdateDB(int deviceNumber, int boardNumber)
        {
            Device device = AppData.DeviceList[deviceNumber];
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                // Delete any spurious previously made plays
                string SQLString = $"DELETE FROM PlayData WHERE Section={device.SectionID} AND [Table]={device.TableNumber} AND Round={device.RoundNumber} AND Board={boardNumber} AND Counter={PlayCounter}";
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

                SQLString = $"INSERT INTO PlayData (Section, [Table], Round, Board, Counter, Direction, Card, DateLog, TimeLog) VALUES ({device.SectionID}, {device.TableNumber}, {device.RoundNumber}, {boardNumber}, {PlayCounter}, '{PlayDirection.Substring(0, 1)}', '{CardString}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#)";
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