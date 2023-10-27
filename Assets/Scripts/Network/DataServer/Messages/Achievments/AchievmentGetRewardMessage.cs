using UnityEngine;

namespace Network.DataServer.Messages.Achievments
{
    public class AchievmentGetRewardMessage : INetworkMessage
    {
        public int PlayerId;
        public int AchievmentId;

        public string Route => "Achievments/GetRewardAchievment";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("AchievmentId", AchievmentId);
                return form;
            }
        }
    }
}
