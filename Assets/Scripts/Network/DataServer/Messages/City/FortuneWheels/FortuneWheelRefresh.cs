using UnityEngine;

namespace Network.DataServer.Messages
{
    public class FortuneWheelRefresh : INetworkMessage
    {
        public int PlayerId;

        public string Route => "FortuneWheel/RefreshRewards";

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
