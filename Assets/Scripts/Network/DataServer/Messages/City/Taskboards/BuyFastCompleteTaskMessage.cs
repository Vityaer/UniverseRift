using UnityEngine;

namespace Network.DataServer.Messages.City.Taskboards
{
    public class BuyFastCompleteTaskMessage : INetworkMessage
    {
        public int PlayerId;
        public int TaskId;

        public string Route => "TaskBoard/BuyFastCompleteTask";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("TaskId", TaskId);
                return form;
            }
        }
    }
}
