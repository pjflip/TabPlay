// TabPlay - a tablet-based system for playing bridge.   Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabPlay.Models
{
    public class Ranking
    {
        public string Orientation {get; set;}
        public int PairNumber {get; set;}
        public string Score {get; set;}
        public double ScoreDecimal {get; set;}
        public string Rank {get; set;}
        public int MP {get; set;}
        public int MPMax {get; set;}
    }
}