using UnityEngine;

namespace Network.DataServer.Messages.Players
{
    public class PlayerChangeAvatarMessage : INetworkMessage
    {
        public int PlayerId;
        public string AvatarPath;

        public string Route => "Players/ChangeAvatar";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("AvatarPath", AvatarPath);
                return form;
            }
        }
    }
}
