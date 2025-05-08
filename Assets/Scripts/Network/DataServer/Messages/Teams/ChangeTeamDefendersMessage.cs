using UnityEngine;

namespace Network.DataServer.Messages.Teams
{
    public class ChangeTeamDefendersMessage : INetworkMessage
    {
        public int PlayerId;
        public string HeroesIdsContainer;
        public string TeamsContainerName;
        
        public string Route => "Heroes/SetDefenders";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("HeroesIdsContainer", HeroesIdsContainer);
                form.AddField("TeamsContainerName", TeamsContainerName);
                return form;
            }
        }
    }
}