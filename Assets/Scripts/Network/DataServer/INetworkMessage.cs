using UnityEngine;

namespace Network.DataServer
{
    public interface INetworkMessage
    {
        public string Route { get; }
        public WWWForm Form { get; }
    }
}
