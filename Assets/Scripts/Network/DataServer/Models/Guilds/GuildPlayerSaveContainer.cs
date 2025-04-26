using System.Collections.Generic;
using Models.Data.Buildings.Taskboards;

namespace Network.DataServer.Models.Guilds
{
    public class GuildPlayerSaveContainer
    {
        public GuildData GuildData;
        public List<RecruitData> GuildRecruits = new();
        public List<GuildPlayerRequest> Requests = new();
        public TaskBoardData TasksData;
    }
}
