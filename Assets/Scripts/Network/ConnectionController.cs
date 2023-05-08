using Mirror;
using UnityEngine;

namespace Network
{
    public class ConnectionController : MonoBehaviour
    {
        public NetworkManager Manager;

        private void Start()
        {
#if UNITY_ANDROID
            Manager.StartClient();
#else
            Manager.StartServer();
#endif
        }
    }
}
