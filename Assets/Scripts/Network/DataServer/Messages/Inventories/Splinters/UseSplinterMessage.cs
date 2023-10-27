using UnityEngine;

namespace Network.DataServer.Messages.Inventories.Splinters
{
    public class UseSplinterMessage : INetworkMessage
    {
        public int PlayerId;
        public string SplinterId;
        public int Count;

        public string Route => "Cheats/UseSplinters";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("SplinterId", SplinterId);
                form.AddField("Count", Count);
                return form;
            }
        }
    }
}
