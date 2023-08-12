using UnityEngine;

namespace Network.DataServer.Messages.Items
{
    public class SetItemMessage : INetworkMessage
    {
        public int PlayerId;
        public int HeroId;
        public string ItemId;

        public string Route => "Items/SetItem";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("HeroId", HeroId);
                form.AddField("ItemId", ItemId);
                return form;
            }
        }
    }
}
