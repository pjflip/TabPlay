﻿// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class MoveOptionsList : List<MoveOption>
    {
        public MoveOptionsList(OdbcConnection conn, int sectionID, int roundNumber)
        {
            using (conn)
            {
                if (AppData.IsIndividual)
                {
                    string SQLString = $"SELECT Table, NSPair, EWPair, South, West FROM RoundData WHERE Section={sectionID} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, conn);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                MoveOption moveOption = new MoveOption()
                                {
                                    TableNumber = reader.GetInt32(0),
                                    PairNS = reader.GetInt32(1),
                                    PairEW = reader.GetInt32(2),
                                    South = reader.GetInt32(3),
                                    West = reader.GetInt32(4),
                                };
                                Add(moveOption);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
                else  // Not individual
                {
                    string SQLString = $"SELECT Table, NSPair, EWPair FROM RoundData WHERE Section={sectionID} AND Round={roundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, conn);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                MoveOption moveOption = new MoveOption()
                                {
                                    TableNumber = reader.GetInt32(0),
                                    PairNS = reader.GetInt32(1),
                                    PairEW = reader.GetInt32(2),
                                };
                                Add(moveOption);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }
                }
            }
        }

        public PlayerMove GetMove(int tableNumber, int pairNumber, string direction)
        {
            PlayerMove playerMove = new PlayerMove();
            MoveOption moveOption = new MoveOption();
            if (!AppData.IsIndividual)  // Pairs
            {
                if (direction == "North" || direction == "South")
                {
                    moveOption = Find(x => x.PairNS == pairNumber);
                }
                else
                {
                    moveOption = Find(x => x.PairEW == pairNumber);
                }
                if (moveOption != null)
                {
                    playerMove.NewTableNumber = moveOption.TableNumber;
                    playerMove.NewDirection = direction;
                }
                else
                {
                    // Pair changes Direction
                    if (direction == "North" || direction == "South")
                    {
                        moveOption = Find(x => x.PairEW == pairNumber);
                    }
                    else
                    {
                        moveOption = Find(x => x.PairNS == pairNumber);
                    }

                    if (moveOption != null)
                    {
                        playerMove.NewTableNumber = moveOption.TableNumber;
                        if (direction == "North")
                        {
                            playerMove.NewDirection = "East";
                        }
                        else if (direction == "East")
                        {
                            playerMove.NewDirection = "North";
                        }
                        else if (direction == "South")
                        {
                            playerMove.NewDirection = "West";
                        }
                        else if (direction == "West")
                        {
                            playerMove.NewDirection = "South";
                        }
                    }
                    else   // No move info found - move to sit out
                    {
                        playerMove.NewTableNumber = 0;
                        playerMove.NewDirection = "";
                    }
                }
                playerMove.Stay = (playerMove.NewTableNumber == tableNumber && playerMove.NewDirection == direction);
                return playerMove;
            }
            else   // Individual
            {
                // Try Direction = North
                moveOption = Find(x => x.PairNS == pairNumber);
                if (moveOption != null)
                {
                    playerMove.NewTableNumber = moveOption.TableNumber;
                    playerMove.NewDirection = "North";
                    playerMove.Stay = (playerMove.NewTableNumber == tableNumber && playerMove.NewDirection == direction);
                    return playerMove;
                }

                // Try Direction = South
                moveOption = Find(x => x.South == pairNumber);
                if (moveOption != null)
                {
                    playerMove.NewTableNumber = moveOption.TableNumber;
                    playerMove.NewDirection = "South";
                    playerMove.Stay = (playerMove.NewTableNumber == tableNumber && playerMove.NewDirection == direction);
                    return playerMove;
                }

                // Try Direction = East
                moveOption = Find(x => x.PairEW == pairNumber);
                if (moveOption != null)
                {
                    playerMove.NewTableNumber = moveOption.TableNumber;
                    playerMove.NewDirection = "East";
                    playerMove.Stay = (playerMove.NewTableNumber == tableNumber && playerMove.NewDirection == direction);
                    return playerMove;
                }

                // Try Direction = West
                moveOption = Find(x => x.West == pairNumber);
                if (moveOption != null)
                {
                    playerMove.NewTableNumber = moveOption.TableNumber;
                    playerMove.NewDirection = "West";
                    playerMove.Stay = (playerMove.NewTableNumber == tableNumber && playerMove.NewDirection == direction);
                    return playerMove;
                }

                else   // No move info found - move to sit out
                {
                    playerMove.NewTableNumber = 0;
                    playerMove.NewDirection = "";
                    playerMove.Stay = false;
                    return playerMove;
                }
            }
        }
    }
}