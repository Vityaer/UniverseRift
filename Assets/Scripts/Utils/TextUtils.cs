﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using Misc.Json;
using Models;
using Network.DataServer;
using Network.DataServer.Jsons;
using Network.Misc;
using Newtonsoft.Json;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils
{
    public static class TextUtils
    {
        public static ReactiveCommand<FileLoadingProgress> DownloadProgress = new ReactiveCommand<FileLoadingProgress>();
        private static FileLoadingProgress fileProgress = new FileLoadingProgress(0);

        private const string MAIN_URL = "https://localhost:7065/";

        public static string GetTextFromLocalStorage<T>()
        {
            var path = GetConfigPath<T>();
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            var text = File.ReadAllText(path);
            return text;
        }

        public static string GetDataFromLocalStorage<T>()
        {
            var path = GetDataPath<T>();
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            var text = File.ReadAllText(path);
            return text;
        }

        public static void SaveGameData<T>(T data)
        {
            File.WriteAllText(GetDataPath<T>(),
                JsonConvert.SerializeObject(data, Constants.Common.SerializerSettings));
        }

        public static string GetDataPath<T>()
        {
            var path = Path.Combine(Constants.Common.GameDataPath, $"{typeof(T).Name}.json");
            return path;
        }

        public static string GetConfigPath<T>()
        {
            var path = Path.Combine(Constants.Common.DictionariesPath, $"{typeof(T).Name}.json");
            return path;
        }

        public static void Save<T>(List<T> data)
        {
            var json = JsonConvert.SerializeObject(data, Constants.Common.SerializerSettings);
            File.WriteAllText(GetConfigPath<T>(), json);
        }


        public static void Save<T>(T data)
        {
            File.WriteAllText(GetConfigPath<T>(),
                JsonConvert.SerializeObject(data, Constants.Common.SerializerSettings));
        }

        public static bool IsLoadedToLocalStorage<T>()
        {
            var path = GetConfigPath<T>();
            return File.Exists(path);
        }

        private static void ReportProgress(float progress)
        {
            fileProgress.Progress = progress;
            DownloadProgress.Execute(fileProgress);
            Debug.Log($"DownloadJsonData {fileProgress.FileName} {progress}%");
        }

        //public static async UniTask<string> DownloadJsonData(string modelType)
        //{
        //    fileProgress.SetNameFile(modelType);
        //    var url = $"{Constants.Common.GAME_DATA_SERVER_ADDRESS}{modelType}.json";
        //    Debug.Log(url);
        //    var progress = Progress.CreateOnlyValueChanged<float>(f => ReportProgress(f));
        //    var asyncRequest = await UnityWebRequest.Get(url).SendWebRequest().ToUniTask(progress);
        //    return asyncRequest.downloadHandler.text;
        //}

        public static async UniTask<string> DownloadJsonData(string modelType)
        {
            var message = new GetJsonDataMessage { Name = modelType };
            var result = await DataServer.GetFileText(message);
            Debug.Log(result);
            return result;
        }

        public static Dictionary<string, T> FillDictionary<T>(string jsonData, IJsonConverter converter)
            where T : BaseModel
        {
            //Save<T>(jsonData);
            //jsonData = GetTextFromLocalStorage<T>();
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var fromJson = JsonConvert.DeserializeObject<List<T>>(jsonData, settings);
            var result = new Dictionary<string, T>();

            if (fromJson == null)
                return result;

            foreach (var model in fromJson)
            {
                result.Add(model.Id, model);
            }

            return result;
        }

        public static T FillModel<T>(string jsonData, IJsonConverter jsonConverter)
        {
            var data = JsonConvert.DeserializeObject<T>(jsonData, Constants.Common.SerializerSettings);
            return data;
        }

        public static List<T> Save<T>(string jsonData, IJsonConverter converter) where T : BaseModel
        {
            var fromJson = converter.Deserialize<List<T>>(jsonData);
            Save(fromJson);
            return fromJson;
        }

        public static void Save<T>(string jsonData)
        {
            File.WriteAllText(GetConfigPath<T>(), jsonData);
        }

        public static StreamWriter GetFileWriterStream(string path, string fileName, bool append)
        {
            var filePath = Path.Combine(path, fileName);

            if (!File.Exists(filePath))
            {
                if (!File.Exists(path))
                    Directory.CreateDirectory(path);
            }

            return new StreamWriter(filePath, append: append);
        }
    }
}