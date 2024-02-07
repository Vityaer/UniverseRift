using UnityEngine;

namespace Network.DataServer.Messages.ChallengeTowers
{
    public class ChallengeTowerTryMissionMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "ChallengeTower/TryMission";

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
