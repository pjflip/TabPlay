namespace TabPlay.Models
{
    public class PlayerStatus
    {
        public string Status { get; set; }
        public bool[] Registered { get; private set; }
        public string[] PlayerName { get; private set; }

        public PlayerStatus(int myDirection, TableStatus tableStatus)
        {
            Status = "PlayerUpdate";
            Registered = new bool[4];
            PlayerName = new string[4];
            for (int i = 0; i < 4; i++)
            {
                // Convert from absolute (North=0) to relative (my direction=0) direction numbers
                Registered[i] = tableStatus.Registered[(myDirection + i) % 4];
                PlayerName[i] = tableStatus.PlayerName[(myDirection + i) % 4];
            }
        }
    }
}