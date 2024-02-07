using UnityEngine;

namespace Network.DataServer.Messages.ChallengeTowers
{
    public class ChallengeTowerFinishMissionMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "ChallengeTower/CompleteNextMission";

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
