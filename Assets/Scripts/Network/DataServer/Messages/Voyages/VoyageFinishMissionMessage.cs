using UnityEngine;

namespace Network.DataServer.Messages.Voyages
{
    public class VoyageFinishMissionMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Voyage/FinishMission";

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
