using UnityEngine;

namespace Network.DataServer.Messages.Friendships
{
    public class SendAndReceivedHeartAllFriendsMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Friendship/SendAndReceivedHeartAllFriends";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                return form;
            }
        }
    }
}
