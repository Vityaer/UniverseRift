using UnityEngine;

namespace Network.DataServer.Messages.DailyRewards
{
    public class GetDailyRewardMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "DailyReward/GetNextReward";

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
