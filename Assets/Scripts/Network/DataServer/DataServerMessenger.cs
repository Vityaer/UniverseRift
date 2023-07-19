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
        public static ReactiveCommand<string> OnError = new ReactiveCommand<string>();

        public static async UniTask<string> PostData<T>(T message) where T : INetworkMessage
        {
            var url = string.Concat(Constants.Common.GAME_SERVER_ADDRESS, message.Route);
            Debug.Log(url);
            UnityWebRequest request = UnityWebRequest.Post(url, message.Form);
            var asyncRequest = await request.SendWebRequest();
            var answer = JsonConvert.DeserializeObject<AnswerModel>(asyncRequest.downloadHandler.text);

            if (!string.IsNullOrEmpty(answer.Error))
            {
                Debug.LogError($"Server error: {answer.Error}");
                OnError.Execute(answer.Error);
            }

            return answer.Result;
        }
    }
}
