// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlay.Models
{
    public class Section
    {
        public int SectionID { get; set; }
        public string SectionLetter { get; set; }
        public int NumTables { get; set; }
        public int MissingPair { get; set; }
    }
}