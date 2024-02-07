using UnityEngine;

namespace Network.DataServer.Messages.Friendships
{
    public class CreateFriendRequestMessage : INetworkMessage
    {
        public int PlayerId;
        public int OtherPlayerId;

        public string Route => "Friendship/CreateFriendRequest";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("OtherPlayerId", OtherPlayerId);
                return form;
            }
        }
    }
}
