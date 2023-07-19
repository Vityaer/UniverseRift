using UnityEngine;

namespace Network.DataServer.Messages.Campaigns
{
    public class GetAutoFightRewardMessage : INetworkMessage
    {
        public int PlayerId;
        public int NumMission;

        public string Route => "Campaign/GetAutoFightReward";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("NumMission", NumMission);
                return form;
            }
        }
    }
}
