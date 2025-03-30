using UnityEngine;

namespace Network.DataServer.Messages.Voyages
{
    public class VoyageStartMissionMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Voyage/StartMission";

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