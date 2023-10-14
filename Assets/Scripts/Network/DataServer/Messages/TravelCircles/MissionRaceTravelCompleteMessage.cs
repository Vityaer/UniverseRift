using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network.DataServer.Messages.TravelCircles
{
    public class MissionRaceTravelCompleteMessage : INetworkMessage
    {
        public int PlayerId;
        public int TravelId;

        public string Route => "TravelCircle/CompleteMission";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("TravelId", TravelId);
                return form;
            }
        }
    }
}
