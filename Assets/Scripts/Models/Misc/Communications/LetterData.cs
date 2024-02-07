namespace Models.Misc
{
    public class LetterData
    {
        public int Id { get; set; }
        public int SenderPlayerId { get; set; }
        public int ReceiverPlayerId { get; set; }
        public string Message { get; set; } = string.Empty;
        public int RewardId { get; set; } = -1;
        public bool IsOpened { get; set; }
        public bool IsRewardReceived { get; set; }
    }
}
