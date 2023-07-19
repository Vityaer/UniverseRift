using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class EditorUtils
    {
        public static List<T> LoadList<T>()
        {
            var text = GetTextFromLocalStorage<T>();
            var data = JsonConvert.DeserializeObject<List<T>>(text, Constants.Common.SerializerSettings);
            if (data == null || data.Count == 0)
            {
                data = new List<T>();
            }

            return data;
        }

        public static T Load<T>()
        {
            var path = GetConfigPath<T>();
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            var text = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<T>(text, Constants.Common.SerializerSettings);
            return data;
        }

        public static void Save<T>(List<T> data)
        {
            File.WriteAllText(GetConfigPath<T>(),
                JsonConvert.SerializeObject(data, Constants.Common.SerializerSettings));
        }

        public static void Save<T>(T data)
        {
            File.WriteAllText(GetConfigPath<T>(),
                JsonConvert.SerializeObject(data, Constants.Common.SerializerSettings));
        }

        public static string GetConfigPath<T>()
        {
            var path = Path.Combine(Constants.Common.DictionariesPath, $"{typeof(T).Name}.json");
            return path;
        }

        private static string GetTextFromLocalStorage<T>()
        {
            var path = GetConfigPath<T>();
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            var text = File.ReadAllText(path);
            return text;
        }

        public static void GetArrowPoints(Vector3 startPoint, Vector3 endPoint, out Vector3 headRight,
            out Vector3 headLeft, out Vector3 headUp, out Vector3 headDown)
        {
            var direction = (endPoint - startPoint).normalized;
            var arrowLenghtPercent = 0.2f;
            var arrowHeadLength = Vector3.Distance(startPoint, endPoint) * arrowLenghtPercent;
            var arrowHeadAngle = 20.0f;

            headRight = endPoint + Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) *
                Vector3.back * arrowHeadLength;
            headLeft = endPoint + Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) *
                Vector3.back * arrowHeadLength;
            headUp = endPoint + Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) *
                Vector3.back * arrowHeadLength;
            headDown = endPoint + Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) *
                Vector3.back * arrowHeadLength;
        }

        public static void DrawArrow(Vector3 startPoint, Vector3 endPoint, Color color)
        {
            GetArrowPoints(startPoint, endPoint, out Vector3 headRight, out Vector3 headLeft, out Vector3 headUp,
                out Vector3 headDown);
            
            Handles.BeginGUI();
            Handles.color = color;
            Handles.DrawLine(startPoint, endPoint);
            Handles.DrawLine(endPoint, headRight);
            Handles.DrawLine(endPoint, headLeft);
            Handles.DrawLine(endPoint, headUp);
            Handles.DrawLine(endPoint, headDown);
            Handles.EndGUI();
        }
    }
}