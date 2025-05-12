using UnityEngine;

namespace Network.DataServer.Messages
{
    public class ReplaceHeroMessage : INetworkMessage
    {
        public int PlayerId;
        public int HeroId;
        public string TargetRace;

        public string Route => "Sanctuary/ReplaceHero";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("HeroId", HeroId);
                form.AddField("TargetRace", TargetRace);
                return form;
            }
        }
    }
}
