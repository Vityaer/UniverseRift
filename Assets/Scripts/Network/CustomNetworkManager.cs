using Mirror;
using UnityEngine;

namespace Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log("OnServerAddPlayer");
            base.OnServerAddPlayer(conn);
        }

        public override void OnClientConnect()
        {
            Debug.Log("client connect");
            base.OnClientConnect();
        }
    }
}
