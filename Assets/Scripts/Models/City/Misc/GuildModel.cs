namespace Models.City.Misc
{
    public class GuildModel : BaseModel
    {
        public string Name;
        public int Level;

        public GuildModel(string id, string name, int level)
        {
            Id = id;
            Name = name;
            Level = level;
        }
    }
}
