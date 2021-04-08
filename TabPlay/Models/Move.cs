// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;
using System.Data.Odbc;

namespace TabPlay.Models
{
    public class Move
    {
        public int DeviceNumber { get; private set; }
        public int NewRoundNumber { get; private set; }
        public int NewTableNumber { get; private set; }
        public string NewDirection { get; private set; }
        public bool Stay { get; private set; }
        public int TableNotReadyNumber { get; private set; }

        public Move(int deviceNumber, int tableNotReadyNumber)
        {
            DeviceNumber = deviceNumber;
            TableNotReadyNumber = tableNotReadyNumber;
            Device device = AppData.DeviceList[deviceNumber];
            NewRoundNumber = device.RoundNumber + 1;
            List<MoveOption> moveOptionsList = new List<MoveOption>();
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                if (AppData.IsIndividual)
                {
                    string SQLString = $"SELECT Table, NSPair, EWPair, South, West FROM RoundData WHERE Section={device.SectionID} AND Round={NewRoundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                MoveOption tempMoveOption = new MoveOption()
                                {
                                    TableNumber = reader.GetInt32(0),
                                    North = reader.GetInt32(1),
                                    East = reader.GetInt32(2),
                                    South = reader.GetInt32(3),
                                    West = reader.GetInt32(4),
                                };
                                moveOptionsList.Add(tempMoveOption);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }

                    // Try Direction = North
                    MoveOption moveOption = moveOptionsList.Find(x => x.North == device.PairNumber);
                    if (moveOption != null)
                    {
                        NewTableNumber = moveOption.TableNumber;
                        NewDirection = "North";
                        Stay = (NewTableNumber == device.TableNumber && NewDirection == device.Direction);
                    }
                    else
                    {
                        // Try Direction = South
                        moveOption = moveOptionsList.Find(x => x.South == device.PairNumber);
                        if (moveOption != null)
                        {
                            NewTableNumber = moveOption.TableNumber;
                            NewDirection = "South";
                            Stay = (NewTableNumber == device.TableNumber && NewDirection == device.Direction);
                        }
                        else
                        {
                            // Try Direction = East
                            moveOption = moveOptionsList.Find(x => x.East == device.PairNumber);
                            if (moveOption != null)
                            {
                                NewTableNumber = moveOption.TableNumber;
                                NewDirection = "East";
                                Stay = (NewTableNumber == device.TableNumber && NewDirection == device.Direction);
                            }
                            else
                            {
                                // Try Direction = West
                                moveOption = moveOptionsList.Find(x => x.West == device.PairNumber);
                                if (moveOption != null)
                                {
                                    NewTableNumber = moveOption.TableNumber;
                                    NewDirection = "West";
                                    Stay = (NewTableNumber == device.TableNumber && NewDirection == device.Direction);
                                }
                                else   // No move info found - move to sit out
                                {
                                    NewTableNumber = 0;
                                    NewDirection = "";
                                    Stay = false;
                                }
                            }
                        }
                    }
                }
                else  // Not individual, so find pair
                {
                    string SQLString = $"SELECT Table, NSPair, EWPair FROM RoundData WHERE Section={device.SectionID} AND Round={NewRoundNumber}";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = null;
                    try
                    {
                        ODBCRetryHelper.ODBCRetry(() =>
                        {
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                MoveOption tempMoveOption = new MoveOption()
                                {
                                    TableNumber = reader.GetInt32(0),
                                    North = reader.GetInt32(1),
                                    East = reader.GetInt32(2),
                                };
                                moveOptionsList.Add(tempMoveOption);
                            }
                        });
                    }
                    finally
                    {
                        reader.Close();
                        cmd.Dispose();
                    }

                    MoveOption moveOption = null;
                    if (device.Direction == "North" || device.Direction == "South")
                    {
                        moveOption = moveOptionsList.Find(x => x.North == device.PairNumber);
                    }
                    else
                    {
                        moveOption = moveOptionsList.Find(x => x.East == device.PairNumber);
                    }
                    if (moveOption != null)
                    {
                        NewTableNumber = moveOption.TableNumber;
                        NewDirection = device.Direction;
                    }
                    else
                    {
                        // Pair changes Direction
                        if (device.Direction == "North" || device.Direction == "South")
                        {
                            moveOption = moveOptionsList.Find(x => x.East == device.PairNumber);
                        }
                        else
                        {
                            moveOption = moveOptionsList.Find(x => x.North == device.PairNumber);
                        }

                        if (moveOption != null)
                        {
                            NewTableNumber = moveOption.TableNumber;
                            if (device.Direction == "North")
                            {
                                NewDirection = "East";
                            }
                            else if (device.Direction == "East")
                            {
                                NewDirection = "North";
                            }
                            else if (device.Direction == "South")
                            {
                                NewDirection = "West";
                            }
                            else if (device.Direction == "West")
                            {
                                NewDirection = "South";
                            }
                        }
                        else   // No move info found - move to sit out
                        {
                            NewTableNumber = 0;
                            NewDirection = device.Direction;
                        }
                    }
                    Stay = (NewTableNumber == device.TableNumber && NewDirection == device.Direction);
                }
            }
        }
    }
}