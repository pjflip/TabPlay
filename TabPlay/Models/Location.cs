// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlay.Models
{
    public class Location
    {
        public int SectionID { get; set; }
        public int TableNumber { get; set; }
        public string Direction { get; set; }
        public int RoundNumber { get; set; }
    }
}
