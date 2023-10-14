using Network.DataServer.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Network.DataServer.Messages
{
    public class FireHeroesMessage : INetworkMessage
    {
        public int PlayerId;
        public FireContainer Container;

        public string Route => "Altar/RemoveHeroes";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                Debug.Log(JsonConvert.SerializeObject(Container));
                form.AddField("jsonContainer", JsonConvert.SerializeObject(Container));
                return form;
            }
        }
    }
}
