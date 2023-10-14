using UnityEngine;

namespace Network.DataServer.Messages.TravelCircles
{
    public class MissionRaceTravelSmashMessage : INetworkMessage
    {
        public int PlayerId;
        public int TravelId;
        public int MissionIndex;
        public int Count;

        public string Route => "TravelCircle/SmashMission";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("TravelId", TravelId);
                form.AddField("MissionIndex", MissionIndex);
                form.AddField("Count", Count);
                return form;
            }
        }
    }
}
