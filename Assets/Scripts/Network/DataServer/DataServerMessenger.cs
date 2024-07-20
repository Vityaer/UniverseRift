using Cysharp.Threading.Tasks;
using Network.DataServer.Common;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Network.DataServer
{
    public class DataServer
    {
        public static ReactiveCommand<string> OnError = new();

        public static async UniTask<string> PostData<T>(T message) where T : INetworkMessage
        {
            var url = string.Concat(Constants.Common.GAME_SERVER_ADDRESS, message.Route);
            //Debug.Log(url);
            UnityWebRequest request = UnityWebRequest.Post(url, message.Form);
            var asyncRequest = await request.SendWebRequest();
            var answer = JsonConvert.DeserializeObject<AnswerModel>(asyncRequest.downloadHandler.text);

            if (ReferenceEquals(answer, null))
                return string.Empty;

            if (!string.IsNullOrEmpty(answer.Error))
            {
                Debug.LogError($"message: {message.Route}, Server error: {answer.Error}");
                OnError.Execute(answer.Error);
            }

            return answer.Result;
        }

        public static async UniTask<string> GetFileText<T>(T message) where T : INetworkMessage
        {
            var url = string.Concat(Constants.Common.GAME_SERVER_ADDRESS, message.Route);
            Debug.Log(url);
            UnityWebRequest request = UnityWebRequest.Post(url, message.Form);
            var asyncRequest = await request.SendWebRequest();
            return asyncRequest.downloadHandler.text;
        }
    }
}
