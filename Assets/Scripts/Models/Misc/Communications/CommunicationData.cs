using Models.Data.Players;
using System.Collections.Generic;

namespace Models.Misc
{
    public class CommunicationData
    {
        public List<FriendshipData> FriendshipDatas = new();
        public List<FriendshipRequest> FriendshipRequests = new();
        public Dictionary<int, PlayerData> PlayersData = new();
        public List<LetterData> LetterDatas = new();
        public List<ChatMessageData> ChatMessages = new();

        public void AddPlayers(Dictionary<int, PlayerData> playersData)
        {
            foreach (var player in playersData)
            {
                if (PlayersData.ContainsKey(player.Key))
                    continue;

                PlayersData.Add(player.Key, player.Value);
            }
        }
    }
}
