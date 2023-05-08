using Mirror;
using UnityEngine;

namespace Network
{
    public class PlayerMessanger : NetworkBehaviour
    {
        public string message;

        [ContextMenu("Test message")]
        public void SendMessage()
        {
            ReceiveMessage(message);
        }

        [Command]
        public void ReceiveMessage(string message)
        {
            Debug.Log(message);
        }
    }
}
