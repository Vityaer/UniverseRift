using Cysharp.Threading.Tasks;
using Editor.Common;
using Network.DataServer;
using Network.DataServer.Messages;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Editor.Connections
{
    public class ConnectionPage : BasePageEditor
    {
        [Button("Registration")]
        public void Registration(string name)
        {
            CreateAccount(name).Forget();
        }

        private async UniTaskVoid CreateAccount(string name)
        {
            Debug.Log("start CreateAccount");
            var message = new PlayerRegistration { Name = name };
            var result = await DataServer.PostData(message);

            if (int.TryParse(result, out int id))
            {
                Debug.Log($"account: {id}");
            }
        }
    }
}
