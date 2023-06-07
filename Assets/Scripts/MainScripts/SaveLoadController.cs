using Models;
using Models.Common;
using Models.Heroes;
using System.Collections.Generic;
using System.IO;
using UIController.Inventory;
using UnityEngine;

namespace MainScripts
{
    public class SaveLoadController : MonoBehaviour
    {
        private static string NameListHeroFile = "FileListHero";
        private static string NameGameFile = "FileGame";
        private static string NameInventoryFile = "FileInventory";

        private static MessageController MessageController = GameObject.Find("MessageController").GetComponent<MessageController>();

        //API
        //Hero
        public static void SaveListHero(List<HeroModel> listHero)
        {
            StreamWriter sw = CreateStream(NameListHeroFile, false);
            foreach (HeroModel hero in listHero)
            {
                var heroSave = new HeroData(hero);
                sw.WriteLine(JsonUtility.ToJson(heroSave));
            }
            sw.Close();
        }


        public static void LoadListHero(List<HeroModel> ResultList)
        {
            HeroData heroSave = new HeroData();
            List<string> rows = ReadFile(NameListHeroFile);
            if (rows.Count > 0)
            {
                foreach (string row in rows)
                {
                    JsonUtility.FromJsonOverwrite(row, heroSave);
                    ResultList.Add(new HeroModel(heroSave));
                }
            }
        }
        //Game status
        public static void SaveGame(GameModel game)
        {
            StreamWriter sw = CreateStream(NameGameFile, false);
            sw.WriteLine(JsonUtility.ToJson(game));
            sw.Close();
        }


        public static void LoadGame(GameModel game)
        {
            List<string> rows = ReadFile(NameGameFile);
            if (rows.Count > 0)
            {
                game.CreateGame(JsonUtility.FromJson<GameModel>(rows[0]));
            }
        }
        //Inventory
        public static void SaveInventory(Inventory inventory)
        {
            InventorySave inventorySave = new InventorySave(inventory);
            StreamWriter sw = CreateStream(NameInventoryFile, false);
            sw.WriteLine(JsonUtility.ToJson(inventorySave));
            sw.Close();
        }
        public static Inventory LoadInventory()
        {
            InventorySave inventorySave = null;
            Inventory inventory = null;
            List<string> rows = ReadFile(NameInventoryFile);
            if (rows.Count > 0)
            {
                inventorySave = JsonUtility.FromJson<InventorySave>(rows[0]);
                inventory = new Inventory(inventorySave);
            }
            if (inventory == null) inventory = new Inventory();
            return inventory;
        }

        //Core
        private static string GetPrefix()
        {
            string prefixNameFile;
#if UNITY_EDITOR_WIN
            prefixNameFile = Application.dataPath;
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			prefixNameFile = Application.persistentDataPath;	
#endif
            return prefixNameFile;
        }
        private static List<string> ReadFile(string NameFile)
        {
            CheckFile(NameFile);
            List<string> ListResult = new List<string>();
            try
            {
                ListResult = new List<string>(File.ReadAllLines(GetPrefix() + "/" + NameFile + ".data"));
            }
            catch { }
            return ListResult;
        }
        private static StreamWriter CreateStream(string NameFile, bool AppendFlag)
        {
            return new StreamWriter(GetPrefix() + "/" + NameFile + ".data", append: AppendFlag);

        }
        public static void CheckFile(string NameFile)
        {
            if (!File.Exists(GetPrefix() + "/" + NameFile + ".data"))
            {
                CreateFile(NameFile);
            }
        }
        public static void CreateFile(string NameFile)
        {
            StreamWriter sw = CreateStream(NameFile, false);
            sw.Close();
        }
    }
}