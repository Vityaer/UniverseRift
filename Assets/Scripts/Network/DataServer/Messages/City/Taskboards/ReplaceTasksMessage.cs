using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network.DataServer.Messages.City.Taskboards
{
    public class ReplaceTasksMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "TaskBoard/ReplaceTasks";

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
