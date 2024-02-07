using UnityEngine;

namespace Network.DataServer.Messages.Friendships
{
    public class BreakFriendshipMessage : INetworkMessage
    {
        public int PlayerId;
        public int FriendshipId;

        public string Route => "Friendship/BreakFriendship";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("FriendshipId", FriendshipId);
                return form;
            }
        }
    }
}
