using UnityEngine;

namespace Network.DataServer.Messages.Mails
{
    public class GetRewardFromLetterMessage : INetworkMessage
    {
        public int PlayerId;
        public int LetterId;

        public string Route => "Mail/GetRewardFromLetter";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("LetterId", LetterId);
                return form;
            }
        }
    }
}
