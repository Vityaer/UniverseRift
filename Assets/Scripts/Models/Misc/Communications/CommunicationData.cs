using Models.Data.Players;
using System.Collections.Generic;

namespace Models.Misc
{
    public class CommunicationData
    {
        public List<FriendshipData> FriendshipDatas = new();
        public List<FriendshipRequest> FriendshipRequests = new();
        public Dictionary<int, PlayerData> PlayersData = new();
    }
}
