using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network.DataServer.Messages
{
    public class SimpleHire : INetworkMessage
    {
        public int PlayerId;
        public int Count;

        public string Route => "Heroes/GetSimpleHeroes";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("Count", Count);
                return form;
            }
        }
    }
}
