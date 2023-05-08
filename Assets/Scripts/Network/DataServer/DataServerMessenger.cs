using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Network.DataServer
{
    public class DataServer
    {
        private const string MAIN_URL = "https://localhost:7065/";

        public static async UniTask<string> Registration(string route, string data)
        {
            var form = new WWWForm();
            form.AddField("name", data);
            var url = string.Concat(MAIN_URL, route);

            UnityWebRequest request = UnityWebRequest.Post(url, form);

            var asyncRequest = await request.SendWebRequest();

            return asyncRequest.downloadHandler.text;
        }

        public static async UniTask<string> PostData<T>(T message) where T : INetworkMessage
        {
            var url = string.Concat(MAIN_URL, message.Route);
            UnityWebRequest request = UnityWebRequest.Post(url, message.Form);
            var asyncRequest = await request.SendWebRequest();
            return asyncRequest.downloadHandler.text;
        }

        public static async UniTask<string> DownloadData(string route)
        {
            var url = string.Concat(MAIN_URL, route);
            var op = await UnityWebRequest.Get(url).SendWebRequest();
            return op.downloadHandler.text;
        }
    }
}
