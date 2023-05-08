using ObjectSave;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

internal class SaveLoadController : MonoBehaviour
{
    private static string NameListHeroFile = "FileListHero";
    private static string NameGameFile = "FileGame";
    private static string NameInventoryFile = "FileInventory";

    private static MessageController MessageController = GameObject.Find("MessageController").GetComponent<MessageController>();

    //API
    //Hero
    public static void SaveListHero(List<InfoHero> listHero)
    {
        StreamWriter sw = CreateStream(NameListHeroFile, false);
        HeroSave heroSave = new HeroSave();
        foreach (InfoHero hero in listHero)
        {
            heroSave.NewData(hero);
            sw.WriteLine(JsonUtility.ToJson(heroSave));
        }
        sw.Close();
    }


    public static void LoadListHero(List<InfoHero> ResultList)
    {
        HeroSave heroSave = new HeroSave();
        List<string> rows = ReadFile(NameListHeroFile);
        if (rows.Count > 0)
        {
            foreach (string row in rows)
            {
                JsonUtility.FromJsonOverwrite(row, heroSave);
                ResultList.Add(new InfoHero(heroSave));
            }
        }
    }
    //Game status
    public static void SaveGame(Game game)
    {
        StreamWriter sw = CreateStream(NameGameFile, false);
        sw.WriteLine(JsonUtility.ToJson(game));
        sw.Close();
    }


    public static void LoadGame(Game game)
    {
        List<string> rows = ReadFile(NameGameFile);
        if (rows.Count > 0)
        {
            game.CreateGame(JsonUtility.FromJson<Game>(rows[0]));
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
