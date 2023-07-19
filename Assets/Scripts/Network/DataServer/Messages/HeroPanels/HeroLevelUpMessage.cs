using UnityEngine;

namespace Network.DataServer.Messages.HeroPanels
{
    public class HeroLevelUpMessage : INetworkMessage
    {
        public string Route => "Heroes/LevelUp";
        public int PlayerId;
        public int HeroId;

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("HeroId", HeroId);
                return form;
            }
        }
    }
}
