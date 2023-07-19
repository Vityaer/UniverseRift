namespace Models.Data.Players
{
    //Player info
    [System.Serializable]
    public class PlayerData : BaseDataModel
    {
        public new int Id = 0;
        public string Name = string.Empty;
        public int Level = 1;
        public int VipLevel;
        public int IDGuild;
        public string AvatarPath;
    }
}
