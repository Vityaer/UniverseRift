using UnityEngine;

namespace Network.DataServer.Messages.City.FortuneWheels
{
    public class FortuneWheelRotate : INetworkMessage
    {
        public int PlayerId;
        public int Count;
        public string Route => "FortuneWheel/Rotate";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("Count", Count);
                return form;
            }
        }
    }
}
