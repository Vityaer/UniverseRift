using Mirror;
using UnityEngine;

namespace Network.GameServer
{
    public class ConnectionController : MonoBehaviour
    {
        public NetworkManager Manager;
        public bool IsServer;

        private void Start()
        {
#if UNITY_ANDROID
            if (IsServer)
            {
                Manager.StartServer();
            }
            else
            {
                Manager.StartClient();
            }
#else
            Manager.StartServer();
#endif
        }
    }
}
