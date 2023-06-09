using UnityEngine;
namespace Network.DataServer.Messages
{
    public abstract class AbstractHireMessage : AbstractMessage, INetworkMessage
    {
        public int Count;

        public string Route => throw new System.NotImplementedException();

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("Count", Count);
                return form;
            }
        }
    }
}
