// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlay.Models
{
    public class PlayerMove
    {
        public int NewTableNumber { get; set; }
        public string NewDirection { get; set; }
        public bool Stay { get; set; }
    }
}
