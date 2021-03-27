// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlay.Models
{
    public class MoveOption
    {
        public int TableNumber { get; set; }
        public int PairNS { get; set; } 
        public int PairEW { get; set; }
        public int South { get; set; }
        public int West { get; set; }
    }
}