using Models.Items;
using UnityEngine;

namespace Network.DataServer.Messages.Items
{
    public class TakeOffItemMessage : INetworkMessage
    {
        public int PlayerId;
        public int HeroId;
        public ItemType ItemType;

        public string Route => "Items/TakeOffItem";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("HeroId", HeroId);
                form.AddField("ItemType", (int)ItemType);
                return form;
            }
        }
    }
}
