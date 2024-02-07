using UnityEngine;

namespace Network.DataServer.Messages.City.LongTravels
{
    public class WinMissionLongTravelMessage : INetworkMessage
    {
        public int PlayerId;
        public int TravelType;
        public int MissionIndex;
        public int Result;

        public string Route => "LongTravel/MissionComplete";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("TravelType", TravelType);
                form.AddField("MissionIndex", MissionIndex);
                form.AddField("Result", Result);
                return form;
            }
        }
    }
}
