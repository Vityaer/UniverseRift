using UnityEngine;

namespace Network.DataServer.Messages.Battlepases
{
    public class GetBattlepasRewardMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Battlepas/GetNextReward";

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
