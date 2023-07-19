using UnityEngine;

namespace Network.DataServer.Messages
{
    public class PlayerRegistration : INetworkMessage
    {
        public string Name;

        public string Route => "Players/Registration";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("Name", Name);
                return form;
            }
        }
    }
}
