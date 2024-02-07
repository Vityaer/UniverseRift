using UnityEngine;

namespace Network.DataServer.Messages.Friendships
{
    public class GetAvailableFriendsMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Friendship/GetAvailableFriends";

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
