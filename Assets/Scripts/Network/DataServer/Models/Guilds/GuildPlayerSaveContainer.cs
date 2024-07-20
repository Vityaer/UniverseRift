using System.Collections.Generic;

namespace Network.DataServer.Models.Guilds
{
    public class GuildPlayerSaveContainer
    {
        public GuildData GuildData;
        public List<RecruitData> GuildRecruits = new();
        public List<GuildPlayerRequest> Requests = new();
    }
}
