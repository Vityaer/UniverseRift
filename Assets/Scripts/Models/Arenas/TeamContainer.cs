using System.Collections.Generic;

namespace Models.Arenas
{
    public class TeamContainer
    {
        public string Id;
        public Dictionary<int, int> Heroes = new();

        public TeamContainer(string id)
        {
            Id = id;
        }
    }
}
