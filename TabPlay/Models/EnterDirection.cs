namespace TabPlay.Models
{
    public class EnterDirection
    {
        public int SectionID { get; set; }
        public int TableNumber { get; set; }
        public string Direction { get; set; }
        public int[] PairNumber { get; set; }
        public bool Confirm { get; set; }
    }
}