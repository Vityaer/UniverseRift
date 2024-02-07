using UnityEngine;

namespace Network.DataServer.Messages.Friendships
{
    public class GetAllFriendRequestsMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Friendship/GetAllFriendRequests";

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
