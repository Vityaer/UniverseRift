using UnityEngine;

namespace Network.DataServer.Messages.Arenas
{
    public class ArenaSetDefendersMessage : INetworkMessage
    {
        public int PlayerId;
        public string HeroesIdsContainer;

        public string Route => "Arena/SetDefenders";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("HeroesIdsContainer", HeroesIdsContainer);
                return form;
            }
        }
    }
}
