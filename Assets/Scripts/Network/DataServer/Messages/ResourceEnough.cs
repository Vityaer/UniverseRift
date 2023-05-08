using UnityEngine;

namespace Network.DataServer.Messages
{
    public class ResourceEnough : INetworkMessage
    {
        public int PlayerId;
        public int ResourceType;
        public float Count;
        public int E10;

        public string Route => "Resources/CheckCount";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("ResourceType", ResourceType);
                form.AddField("Count", Count.ToString());
                form.AddField("E10", E10);
                return form;
            }
        }
    }
}
