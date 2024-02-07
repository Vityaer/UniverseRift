using UnityEngine;

namespace Network.DataServer.Messages.Mails
{
    public class OpenLetterMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;
        public int RequestId;

        public string Route => "Mail/OpenLetter";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildId", GuildId);
                form.AddField("RequestId", RequestId);
                return form;
            }
        }
    }
}
