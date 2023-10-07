using UnityEngine;

namespace Network.DataServer.Messages.City.Taskboards
{
    public class StartTaskMessage : INetworkMessage
    {
        public int PlayerId;
        public int TaskId;

        public string Route => "TaskBoard/StartTask";

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
