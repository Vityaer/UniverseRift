using Network.DataServer.Messages;
using UnityEngine;

namespace Network.DataServer.DevelopmentMessages
{
    public class ChangeGameCycleMessage : AbstractMessage, INetworkMessage
    {
        public new string Route => "ServerController/FinishGameCycle";

        public new WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                return form;
            }
        }
    }
}
