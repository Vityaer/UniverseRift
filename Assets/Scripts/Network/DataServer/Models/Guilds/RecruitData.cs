namespace Network.DataServer.Models.Guilds
{
    public class RecruitData
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int GuildId { get; set; }
        public float DonateMantissa { get; set; }
        public int DonateE10 { get; set; }
        public float ResultMantissa { get; set; }
        public float ResultE10 { get; set; }
        public string IntroductionDateTime { get; set; } = string.Empty;
        public int CountRaidBoss { get; set; }
        public string DateTimeFirstRaidBoss { get; set; } = string.Empty;
    }
}
