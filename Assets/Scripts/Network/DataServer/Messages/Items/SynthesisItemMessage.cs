using UnityEngine;

namespace Network.DataServer.Messages.Items
{
    public class SynthesisItemMessage : INetworkMessage
    {
        public int PlayerId;
        public string ItemId;
        public int Count;

        public string Route => "Forge/CreateItem";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("ItemId", ItemId);
                form.AddField("Count", Count);
                return form;
            }
        }
    }
}
