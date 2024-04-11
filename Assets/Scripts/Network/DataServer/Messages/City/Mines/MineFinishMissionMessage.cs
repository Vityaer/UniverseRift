using UnityEngine;

namespace Network.DataServer.Messages.City.Mines
{
    public class MineFinishMissionMessage : INetworkMessage
    {
        public int PlayerId;
        public int MissionId;

        public string Route => "Mines/FinishMission";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("MissionId", MissionId);
                return form;
            }
        }
    }
}
