using UnityEngine;

namespace Network.DataServer.Messages.City.Taskboards
{
    public class BuySimpleTaskMessage : AbstractMessage, INetworkMessage
    {
        public int PlayerId;

        public string Route => "TaskBoard/BuySimpleTask";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                return form;
            }
        }
    }
}
