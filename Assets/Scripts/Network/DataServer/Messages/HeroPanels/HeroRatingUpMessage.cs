using UnityEngine;

namespace Network.DataServer.Messages.HeroPanels
{
    public class HeroRatingUpMessage : INetworkMessage
    {
        public string Route => "Heroes/RatingUp";

        public int PlayerId;
        public int HeroId;
        public string HeroesPaymentContainer;

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("HeroId", HeroId);
                form.AddField("HeroesPaymentContainer", HeroesPaymentContainer);
                return form;
            }
        }
    }
}
