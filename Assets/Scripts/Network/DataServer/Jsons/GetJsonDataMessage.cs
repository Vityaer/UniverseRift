using UnityEngine;

namespace Network.DataServer.Jsons
{
    public class GetJsonDataMessage : INetworkMessage
    {
        public string Name;

        public string Route => "Json/GetJsonFile";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("name", Name);
                return form;
            }
        }
    }
}
