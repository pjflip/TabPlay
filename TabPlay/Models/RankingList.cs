// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class RankingList : List<Ranking>
    {
        public int SectionID { get; private set; }
        public int RoundNumber { get; private set; }
        public int TableNumber { get; private set; }
        public string Direction { get; set; }
        public int PairNumber { get; set; }
        public string PairOrPlayer { get; private set; }
        public bool TwoWinners { get; private set; }
        public bool FinalRankingList { get; set; } = false;

        public RankingList(int sectionID, int tableNumber, int roundNumber, string direction, int pairNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            RoundNumber = roundNumber;
            Direction = direction;
            PairNumber = pairNumber;
            if (AppData.IsIndividual)
            {
                PairOrPlayer = "Player";
            }
            else
            {
                PairOrPlayer = "Pair";
            }

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT Orientation, Number, Score, Rank FROM Results WHERE Section={sectionID}";

                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader1 = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader1 = cmd.ExecuteReader();
                        while (reader1.Read())
                        {
                            Ranking ranking = new Ranking
                            {
                                Orientation = reader1.GetString(0),
                                PairNumber = reader1.GetInt32(1),
                                Score = reader1.GetString(2),
                                Rank = reader1.GetString(3)
                            };
                            ranking.ScoreDecimal = Convert.ToDouble(ranking.Score);
                            Add(ranking);
                        }
                    });
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Any error other than results table doesn't exist
                    {
                        throw (e);
                    }
                }
                finally
                {
                    reader1.Close();
                    cmd.Dispose();
                }

                if (Count == 0)  // Results table either doesn't exist or contains no entries, so try to calculate rankings
                {
                    if (AppData.IsIndividual)
                    {
                        InsertRange(0, CalculateIndividualRankingFromReceivedData(connection, sectionID));
                    }
                    else
                    {
                        InsertRange(0, CalculateRankingFromReceivedData(connection, sectionID));
                    }
                }
            }
            // Make sure that ranking list is sorted into presentation order
            Sort((x, y) =>
            {
                int sortValue = y.Orientation.CompareTo(x.Orientation);    // N's first then E's
                if (sortValue == 0) sortValue = y.ScoreDecimal.CompareTo(x.ScoreDecimal);
                if (sortValue == 0) sortValue = x.PairNumber.CompareTo(y.PairNumber);
                return sortValue;
            });

            // Work out if one- or two-winner movement
            TwoWinners = !AppData.IsIndividual && Exists(x => x.Orientation == "E");
        }

        private static List<Ranking> CalculateRankingFromReceivedData(OdbcConnection conn, int sectionID)
        {
            List<Ranking> rankingList = new List<Ranking>();
            List<Result> resList = new List<Result>();

            int Winners = 0;

            // Check Winners
            object queryResult = null;
            string SQLString = $"SELECT Winners FROM Section WHERE ID={sectionID}";
            OdbcCommand cmd1 = new OdbcCommand(SQLString, conn);
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    queryResult = cmd1.ExecuteScalar();
                });
                Winners = Convert.ToInt32(queryResult);
            }
            catch (OdbcException)
            {
                // If Winners column doesn't exist, or any other error, can't calculate ranking
                return null;
            }
            finally
            {
                cmd1.Dispose();
            }

            if (Winners == 0)
            {
                // Winners not set, so no chance of calculating ranking
                return null;
            }

            // No Results table and Winners = 1 or 2, so use ReceivedData to calculate ranking
            SQLString = $"SELECT Board, PairNS, PairEW, [NS/EW], Contract, Result FROM ReceivedData WHERE Section={sectionID}";
            OdbcCommand cmd2 = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader = cmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        Result result = new Result()
                        {
                            BoardNumber = reader.GetInt32(0),
                            PairNS = reader.GetInt32(1),
                            PairEW = reader.GetInt32(2),
                            DeclarerNSEW = reader.GetString(3),
                            Contract = reader.GetString(4),
                            TricksTakenSymbol = reader.GetString(5)
                        };
                        if (result.Contract.Length > 2)  // Testing for unplayed boards and corrupt ReceivedData table
                            {
                            result.CalculateScore();
                            resList.Add(result);
                        }
                    }
                });
            }
            catch (OdbcException)
            {
                return null;
            }
            finally
            {
                reader.Close();
                cmd2.Dispose();
            }

            // Calculate MPs 
            List<Result> currentBoardResultList = new List<Result>();
            int currentBoard;
            int currentScore;
            foreach (Result result in resList)
            {
                currentScore = result.Score;
                currentBoard = result.BoardNumber;
                currentBoardResultList = resList.FindAll(x => x.BoardNumber == currentBoard);
                result.MatchpointsNS = 2 * currentBoardResultList.FindAll(x => x.Score < currentScore).Count + currentBoardResultList.FindAll(x => x.Score == currentScore).Count - 1;
                result.MatchpointsMax = 2 * currentBoardResultList.Count - 2;
                result.MatchpointsEW = result.MatchpointsMax - result.MatchpointsNS;
            }

            if (Winners == 1)
            {
                // Add up MPs for each pair, creating Ranking List entries as we go
                foreach (Result result in resList)
                {
                    Ranking rankingListFind = rankingList.Find(x => x.PairNumber == result.PairNS);
                    if (rankingListFind == null)
                    {
                        Ranking ranking = new Ranking()
                        {
                            PairNumber = result.PairNS,
                            Orientation = "0",
                            MP = result.MatchpointsNS,
                            MPMax = result.MatchpointsMax
                        };
                        rankingList.Add(ranking);
                    }
                    else
                    {
                        rankingListFind.MP += result.MatchpointsNS;
                        rankingListFind.MPMax += result.MatchpointsMax;
                    }
                    rankingListFind = rankingList.Find(x => x.PairNumber == result.PairEW);
                    if (rankingListFind == null)
                    {
                        Ranking ranking = new Ranking()
                        {
                            PairNumber = result.PairEW,
                            Orientation = "0",
                            MP = result.MatchpointsEW,
                            MPMax = result.MatchpointsMax
                        };
                        rankingList.Add(ranking);
                    }
                    else
                    {
                        rankingListFind.MP += result.MatchpointsEW;
                        rankingListFind.MPMax += result.MatchpointsMax;
                    }
                }
                // Calculate percentages
                foreach (Ranking ranking in rankingList)
                {
                    if (ranking.MPMax == 0)
                    {
                        ranking.ScoreDecimal = 50.0;
                    }
                    else
                    {
                        ranking.ScoreDecimal = 100.0 * ranking.MP / ranking.MPMax;
                    }
                    ranking.Score = ranking.ScoreDecimal.ToString("0.##");
                }
                // Calculate ranking
                foreach (Ranking ranking in rankingList)
                {
                    double currentScoreDecimal = ranking.ScoreDecimal;
                    int rank = rankingList.FindAll(x => x.ScoreDecimal > currentScoreDecimal).Count + 1;
                    ranking.Rank = rank.ToString();
                    if (rankingList.FindAll(x => x.ScoreDecimal == currentScoreDecimal).Count > 1)
                    {
                        ranking.Rank += "=";
                    }
                }
            }
            else    // Winners = 2
            {
                // Add up MPs for each pair, creating Ranking List entries as we go
                foreach (Result result in resList)
                {
                    Ranking rankingListFind = rankingList.Find(x => x.PairNumber == result.PairNS && x.Orientation == "N");
                    if (rankingListFind == null)
                    {
                        Ranking ranking = new Ranking()
                        {
                            PairNumber = result.PairNS,
                            Orientation = "N",
                            MP = result.MatchpointsNS,
                            MPMax = result.MatchpointsMax
                        };
                        rankingList.Add(ranking);
                    }
                    else
                    {
                        rankingListFind.MP += result.MatchpointsNS;
                        rankingListFind.MPMax += result.MatchpointsMax;
                    }
                    rankingListFind = rankingList.Find(x => x.PairNumber == result.PairEW && x.Orientation == "E");
                    if (rankingListFind == null)
                    {
                        Ranking ranking = new Ranking()
                        {
                            PairNumber = result.PairEW,
                            Orientation = "E",
                            MP = result.MatchpointsEW,
                            MPMax = result.MatchpointsMax
                        };
                        rankingList.Add(ranking);
                    }
                    else
                    {
                        rankingListFind.MP += result.MatchpointsEW;
                        rankingListFind.MPMax += result.MatchpointsMax;
                    }
                }
                // Calculate percentages
                foreach (Ranking ranking in rankingList)
                {
                    if (ranking.MPMax == 0)
                    {
                        ranking.ScoreDecimal = 50.0;
                    }
                    else
                    {
                        ranking.ScoreDecimal = 100.0 * ranking.MP / ranking.MPMax;
                    }
                    ranking.Score = ranking.ScoreDecimal.ToString("0.##");
                }
                // Sort and calculate ranking within Orientation subsections
                foreach (Ranking ranking in rankingList)
                {
                    double currentScoreDecimal = ranking.ScoreDecimal;
                    string currentOrientation = ranking.Orientation;
                    int rank = rankingList.FindAll(x => x.Orientation == currentOrientation && x.ScoreDecimal > currentScoreDecimal).Count + 1;
                    ranking.Rank = rank.ToString();
                    if (rankingList.FindAll(x => x.Orientation == currentOrientation && x.ScoreDecimal == currentScoreDecimal).Count > 1)
                    {
                        ranking.Rank += "=";
                    }
                }
            }
            return rankingList;
        }

        private static List<Ranking> CalculateIndividualRankingFromReceivedData(OdbcConnection conn, int sectionID)
        {
            List<Ranking> rankingList = new List<Ranking>();
            List<Result> resList = new List<Result>();

            string SQLString = $"SELECT Board, PairNS, PairEW, South, West, [NS/EW], Contract, Result FROM ReceivedData WHERE Section={sectionID}";

            OdbcCommand cmd = new OdbcCommand(SQLString, conn);
            OdbcDataReader reader = null;
            try
            {
                ODBCRetryHelper.ODBCRetry(() =>
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Result result = new Result()
                        {
                            BoardNumber = reader.GetInt32(0),
                            PairNS = reader.GetInt32(1),
                            PairEW = reader.GetInt32(2),
                            South = reader.GetInt32(3),
                            West = reader.GetInt32(4),
                            DeclarerNSEW = reader.GetString(5),
                            Contract = reader.GetString(6),
                            TricksTakenSymbol = reader.GetString(7)
                        };
                        if (result.Contract.Length > 2)  // Testing for unplayed boards and corrupt ReceivedData table
                            {
                            result.CalculateScore();
                            resList.Add(result);
                        }
                    }
                });
            }
            catch (OdbcException)
            {
                return null;
            }
            finally
            {
                reader.Close();
                cmd.Dispose();
            }

            // Calculate MPs 
            List<Result> currentBoardResultList = new List<Result>();
            int currentBoard;
            int currentScore;
            foreach (Result result in resList)
            {
                currentScore = result.Score;
                currentBoard = result.BoardNumber;
                currentBoardResultList = resList.FindAll(x => x.BoardNumber == currentBoard);
                result.MatchpointsNS = 2 * currentBoardResultList.FindAll(x => x.Score < currentScore).Count + currentBoardResultList.FindAll(x => x.Score == currentScore).Count - 1;
                result.MatchpointsMax = 2 * currentBoardResultList.Count - 2;
                result.MatchpointsEW = result.MatchpointsMax - result.MatchpointsNS;
            }

            // Add up MPs for each pair, creating Ranking List entries as we go
            foreach (Result result in resList)
            {
                Ranking rankingListFind = rankingList.Find(x => x.PairNumber == result.PairNS);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNumber = result.PairNS,
                        Orientation = "0",
                        MP = result.MatchpointsNS,
                        MPMax = result.MatchpointsMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MatchpointsNS;
                    rankingListFind.MPMax += result.MatchpointsMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNumber == result.PairEW);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNumber = result.PairEW,
                        Orientation = "0",
                        MP = result.MatchpointsEW,
                        MPMax = result.MatchpointsMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MatchpointsEW;
                    rankingListFind.MPMax += result.MatchpointsMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNumber == result.South);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNumber = result.South,
                        Orientation = "0",
                        MP = result.MatchpointsNS,
                        MPMax = result.MatchpointsMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MatchpointsNS;
                    rankingListFind.MPMax += result.MatchpointsMax;
                }
                rankingListFind = rankingList.Find(x => x.PairNumber == result.West);
                if (rankingListFind == null)
                {
                    Ranking ranking = new Ranking()
                    {
                        PairNumber = result.West,
                        Orientation = "0",
                        MP = result.MatchpointsEW,
                        MPMax = result.MatchpointsMax
                    };
                    rankingList.Add(ranking);
                }
                else
                {
                    rankingListFind.MP += result.MatchpointsEW;
                    rankingListFind.MPMax += result.MatchpointsMax;
                }
            }
            // Calculate percentages
            foreach (Ranking ranking in rankingList)
            {
                if (ranking.MPMax == 0)
                {
                    ranking.ScoreDecimal = 50.0;
                }
                else
                {
                    ranking.ScoreDecimal = 100.0 * ranking.MP / ranking.MPMax;
                }
                ranking.Score = ranking.ScoreDecimal.ToString("0.##");
            }
            // Calculate ranking
            rankingList.Sort((x, y) => y.ScoreDecimal.CompareTo(x.ScoreDecimal));
            foreach (Ranking ranking in rankingList)
            {
                double currentScoreDecimal = ranking.ScoreDecimal;
                int rank = rankingList.FindAll(x => x.ScoreDecimal > currentScoreDecimal).Count + 1;
                ranking.Rank = rank.ToString();
                if (rankingList.FindAll(x => x.ScoreDecimal == currentScoreDecimal).Count > 1)
                {
                    ranking.Rank += "=";
                }
            }

            return rankingList;
        }
    }
}
