using System;
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
        public static readonly ReactiveCommand<string> OnError = new();

        public static async UniTask<string> PostData<T>(T message) where T : INetworkMessage
        {
            Uri serverUri = new Uri(Constants.Common.GAME_SERVER_ADDRESS);
            Uri targetUri =  new Uri(serverUri, message.Route);
            
            UnityWebRequest request = UnityWebRequest.Post(targetUri, message.Form);
            request.SetRequestHeader("TestHeader", "TestHeaderValue");
            var asyncRequest = await request.SendWebRequest();
            var answer = JsonConvert.DeserializeObject<AnswerModel>(asyncRequest.downloadHandler.text);

            if (ReferenceEquals(answer, null))
                return string.Empty;

            if (!string.IsNullOrEmpty(answer.Error))
            {
                Debug.LogError($"message: {message.Route}, Server error: {answer.Error}");
                OnError.Execute(answer.Error);
            }

            request.Dispose();
            
            return answer.Result;
        }

        public static async UniTask<string> GetFileText<T>(T message) where T : INetworkMessage
        {
            var url = string.Concat(Constants.Common.GAME_SERVER_ADDRESS, message.Route);
            UnityWebRequest request = UnityWebRequest.Post(url, message.Form);
            var asyncRequest = await request.SendWebRequest();
            return asyncRequest.downloadHandler.text;
        }
    }
}
