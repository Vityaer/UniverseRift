using UnityEngine;

namespace Network.DataServer.Messages.Mails
{
    public class CreateLetterMessage : INetworkMessage
    {
        public int PlayerId;
        public int OtherPlayerId;
        public string Message;

        public string Route => "Mail/CreateLetter";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("OtherPlayerId", OtherPlayerId);
                form.AddField("message", Message);
                return form;
            }
        }
    }
}
