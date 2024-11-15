﻿using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using System.Collections.Generic;
using System.IO;
using UIController.Inventory;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR_WIN

public class TabPanelCheats : EditorWindow
{
    Vector2 scrollPos;
    string IDsplinter;
    string IDItem;
    GameResource res = new GameResource();

    [MenuItem("Tools/My develop/TabPanel Cheat")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        TabPanelCheats window = (TabPanelCheats)EditorWindow.GetWindow(typeof(TabPanelCheats));
        window.Show();
    }
    
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("Clear Data", EditorStyles.boldLabel);
        if (GUILayout.Button("Clear heroes")) { ClearListHeroes(); }
        if (GUILayout.Button("Clear general info")) { ClearGeneralInfo(); }
        //if (GUILayout.Button("Clear resources")) { ClearResources(); }
        if (GUILayout.Button("Clear inventory")) { ClearInventory(); }
        if (GUILayout.Button("Clear All info")) { ClearAllData(); }

        EditorGUILayout.Space();
        GUILayout.Label("Cheats", EditorStyles.boldLabel);
        //if (GUILayout.Button("+ 50 Simple Hire")) { AddSimpleHire(); }
        //if (GUILayout.Button("+ 500k gold")) { Add500kGold(); }
        //if (GUILayout.Button("+ 50 Simple tasks")) { AddSimpleTask(); }
        //if (GUILayout.Button("+ 50 Special tasks")) { AddSpecialTask(); }
        //if (GUILayout.Button("+ 50 race hire")) { AddRaceHire(); }
        //if (GUILayout.Button("+ 50 spin coin")) { AddSpinCoin(); }
        GUILayout.BeginHorizontal("box");
        //res.Type = (ResourceType)EditorGUILayout.EnumPopup("TypeResource:", res.Type);
        //res.Count = EditorGUILayout.FloatField("Count:", res.Count);
        //res.E10 = EditorGUILayout.IntField("E10:", res.E10);
        //if (GUILayout.Button("Add Resource")) { AddCustomResource(); }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        IDsplinter = EditorGUILayout.TextField("ID:", IDsplinter);
        //if (GUILayout.Button("+ Splinters")) { AddSplinters(); }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        IDItem = EditorGUILayout.TextField("ID:", IDItem);
        //if (GUILayout.Button("+ Item")) { AddItem(); }
        GUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }


    private static string NameListHeroFile = "FileListHero";
    private static string NameGameFile = "FileGame";
    private static string NameInventoryFile = "FileInventory";
    void ClearAllData()
    {
        ClearListHeroes();
        ClearGeneralInfo();
        ClearInventory();
    }
    void ClearListHeroes()
    {
        StreamWriter sw = CreateStream(NameListHeroFile, false);
        sw.Close();
    }
    void ClearGeneralInfo()
    {
        StreamWriter sw = CreateStream(NameGameFile, false);
        sw.Close();
    }
    void ClearInventory()
    {
        StreamWriter sw = CreateStream(NameInventoryFile, false);
        sw.Close();
    }
    //void ClearResources()
    //{
    //    GameController.Instance.ClearAllResource();
    //}
    //void Add500kGold()
    //{
    //    if (GameController.Instance != null) GameController.Instance.AddResource(new GameResource(ResourceType.Gold, 500, 3));
    //}
    //void AddSimpleHire()
    //{
    //    if (GameController.Instance != null) GameController.Instance.AddResource(new GameResource(ResourceType.SimpleHireCard, 50, 0));
    //}
    //void AddSimpleTask()
    //{
    //    if (GameController.Instance != null) GameController.Instance.AddResource(new GameResource(ResourceType.SimpleTask, 50, 0));
    //}
    //void AddSpecialTask()
    //{
    //    if (GameController.Instance != null) GameController.Instance.AddResource(new GameResource(ResourceType.SpecialTask, 50, 0));
    //}
    //void AddRaceHire()
    //{
    //    if (GameController.Instance != null) GameController.Instance.AddResource(new GameResource(ResourceType.RaceHireCard, 50, 0));
    //}
    //void AddSpinCoin()
    //{
    //    if (GameController.Instance != null) GameController.Instance.AddResource(new GameResource(ResourceType.CoinFortune, 50, 0));
    //}
    //void AddCustomResource()
    //{
    //    if (GameController.Instance != null) GameController.Instance.AddResource(res);
    //}

    //void AddSplinters()
    //{
    //    if (InventoryController.Instance != null)
    //    {
    //        if (splintersList == null)
    //            splintersList = Resources.Load<SplintersList>("Items/ListSplinters");

    //        var name = IDsplinter;

    //        GameSplinter splinter = splintersList.GetSplinter(name);
    //        if (splinter != null)
    //            InventoryController.Instance.AddSplinter(new SplinterController(splinter, splinter.RequireAmount));
    //    }
    //}

    //void AddItem()
    //{
    //    if (InventoryController.Instance != null)
    //    {
    //        if (itemsList == null) itemsList = Resources.Load<ItemsList>("Items/ListItems");
    //        GameItem item = itemsList?.GetItem(IDItem);
    //        if (item != null)
    //        {
    //            InventoryController.Instance.AddItem(new ItemController(item, 1));
    //        }
    //        else
    //        {
    //            Debug.Log("хуйня");
    //        }
    //    }
    //}


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
#endif