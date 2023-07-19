using Newtonsoft.Json;
using UnityEngine;

public class Constants
{
    public class Common
    {
        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static string DictionariesPath
        {
            get
            {
#if UNITY_EDITOR
                return "Assets/API/Dictionaries";
#elif UNITY_STANDALONE || UNITY_SERVER
                return Application.streamingAssetsPath;
#elif UNITY_ANDROID
                return Application.persistentDataPath;
#endif
            }
        }

        public static string GameDataPath
        {
            get
            {
#if UNITY_EDITOR
                return "Assets/API/GameData";
#elif UNITY_STANDALONE || UNITY_SERVER
                return Application.streamingAssetsPath;
#elif UNITY_ANDROID
                return Application.persistentDataPath;
#endif
            }
        }

        
        public static string GAME_SERVER_IP => "45.12.75.166";
        public static string GAME_DATA_SERVER_ADDRESS => $"http://{GAME_SERVER_IP}/Data/Dictionaries/";
#if UNITY_EDITOR
        public static string GAME_SERVER_ADDRESS => $"https://localhost:7065/";
#else
        public static string GAME_SERVER_ADDRESS => $"http://{GAME_SERVER_IP}:5000/";
#endif

    }

    public static class Game
    {
        public const int TACT_TIME = 5;
    }

    public static class Fight
    {
        public static Vector2 CellDeltaStep = new Vector2(0.826f, 0.715f);
    }

    public static class Colors
    {
        public static Color ACHIEVABLE_CELL_COLOR = new Color(0, 0, 0, 0.5f);
        public static Color ACHIEVABLE_ENEMY_CELL_COLOR = new Color(255, 0, 0, 0.7f);
        public static Color NOT_ACHIEVABLE_ENEMY_CELL_COLOR = new Color(255, 0, 0, 0.3f);
        public static Color ACHIEVABLE_FRIEND_CELL_COLOR = new Color(0, 255, 0, 0.7f);
        public static Color NOT_ACHIEVABLE_FRIEND_CELL_COLOR = new Color(0, 255, 0, 0.3f);
    }

    public static class ResourcesPath
    {
        public static string HEROES_PATH = "Heroes/";
        public static string HERO_TEMPLATE_PATH = "Heroes/HeroTemplate";
        public static string SFX_PREFAB = "Sounds/ButtonClick";
    }
}