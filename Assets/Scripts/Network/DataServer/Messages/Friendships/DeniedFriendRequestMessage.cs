using UnityEngine;

namespace Network.DataServer.Messages.Friendships
{
    public class DeniedFriendRequestMessage : INetworkMessage
    {
        public int PlayerId;
        public int RequestId;

        public string Route => "Friendship/DenyFriendRequest";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("RequestId", RequestId);
                return form;
            }
        }
    }
}
