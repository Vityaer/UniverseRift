using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network.DataServer.Messages.Players
{
    public class PlayerNewLevelMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Players/PlayerLevelUp";

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
