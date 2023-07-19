using System;
using UnityEngine;

namespace Network.DataServer.Messages
{
    public class AbstractMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => throw new NotImplementedException();

        public WWWForm Form => throw new NotImplementedException();
    }
}
