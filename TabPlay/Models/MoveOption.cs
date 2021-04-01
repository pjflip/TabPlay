// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlay.Models
{
    public class MoveOption
    {
        public int TableNumber { get; set; }
        public int North { get; set; } 
        public int East { get; set; }
        public int South { get; set; }
        public int West { get; set; }
    }
}