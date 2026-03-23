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
            AnswerModel answer = null;

            try
            {
                UnityWebRequest asyncRequest = await request.SendWebRequest();
                answer = JsonConvert.DeserializeObject<AnswerModel>(asyncRequest.downloadHandler.text);
            }
            catch(Exception exception)
            {
               Debug.LogError(exception); 
            }


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
            string url = string.Concat(Constants.Common.GAME_SERVER_ADDRESS, message.Route);
            string result = null;

            try
            {
                UnityWebRequest request = UnityWebRequest.Post(url, message.Form);
                UnityWebRequest asyncRequest = await request.SendWebRequest();
                result = asyncRequest.downloadHandler.text;
            }
            catch(Exception exception)
            {
                Debug.LogError(exception); 
            }

            return result;
        }
    }
}
