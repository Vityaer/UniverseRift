﻿using UnityEngine;

namespace Network.DataServer.Messages.Hires
{
    public class MagicCircleHire : INetworkMessage
    {
        public int PlayerId;
        public int Count;
        public string RaceName;

        public string Route => "MagicCircle/GetHeroes";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("Count", Count);
                form.AddField("raceName", RaceName);
                return form;
            }
        }
    }
}
