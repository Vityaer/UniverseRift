using UnityEngine;

namespace Network.DataServer.Messages.Hires
{
    public class RefreshDayMessage : AbstractMessage, INetworkMessage
    {
        public new string Route => "ServerController/FinishDay";

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
