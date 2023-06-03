using Models;
using System;
using System.Collections.Generic;
using UnityEngine;
public class CampaignBuilding : BuildingWithFight
{
    private MissionController infoMission;
    [SerializeField] private List<MissionController> missionControllers = new List<MissionController>();
    private CampaignMission mission;
    private int currentMission, maxMission;
    public CampaignChapter chapter;
    [SerializeField] private List<CampaignChapter> chapters = new List<CampaignChapter>();
    public BasePanelScript panelWorldCampaign;

    private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission",
                         NAME_RECORD_NUM_MAX_MISSION = "MaxMission",
                         NAME_RECORD_AUTOFIGHT_PREVIOUS_DATETIME = "AutoFight";

    protected override void OnLoadGame()
    {
        Debug.Log("campaing loading...");
        campaingSaveObject = GameController.GetCitySave.mainCampaignBuilding;
        currentMission = campaingSaveObject.GetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION, defaultNum: -1);
        maxMission = campaingSaveObject.GetRecordInt(NAME_RECORD_NUM_MAX_MISSION, defaultNum: 0);
        if (currentMission > maxMission) currentMission = maxMission;

        this.chapter = GetCampaignChapter(currentMission);
        LoadMissions(this.chapter);
    }
    private CampaignChapter GetCampaignChapter(int numMission)
    {
        CampaignChapter result = chapters.Find(x => x.numChapter == (numMission / countMission));
        if (result == null) result = chapters[chapters.Count - 1];
        return result;
    }
    //API
    int countMission = 20;
    public void LoadMissions(CampaignChapter chapter)
    {
        int current = 0;
        for (int i = 0; i < missionControllers.Count; i++)
        {
            missionControllers[i].SetMission(chapter.missions[i], (chapter.numChapter * countMission) + i + 1);
        }
        if (chapter.numChapter * countMission <= maxMission)
        {
            for (int i = 0; ((chapter.numChapter * countMission + i) <= maxMission) && (i < missionControllers.Count); i++)
            {
                missionControllers[i].CompletedMission();
            }
            if (chapter.numChapter == (maxMission / countMission))
            {
                missionControllers[maxMission % countMission].OpenMission();
            }
        }
        if (currentMission >= 0)
        {
            if (chapter.numChapter == (currentMission / countMission))
            {
                missionControllers[currentMission % countMission].SetAutoFight();
            }
            else
            {
                CampaignChapter chapterAutoFight = GetCampaignChapter(currentMission);
                AutoFight.Instance.SelectMissionAutoFight(chapterAutoFight.missions[currentMission % countMission]);
            }
        }
    }

    public void OpenChapter(CampaignChapter chapter)
    {
        this.chapter = chapter;
        LoadMissions(chapter);
        panelWorldCampaign.Close();
    }
    public void OpenNextMission()
    {
        OpenMission(currentMission + 1);
        campaingSaveObject.SetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION, currentMission);
        campaingSaveObject.SetRecordInt(NAME_RECORD_NUM_MAX_MISSION, maxMission);
        GameController.Instance.SaveGame();
    }
    public void OpenMission(int num)
    {
        currentMission = num;
        maxMission = num + 1;
        if (chapter.numChapter == (maxMission / 20))
        {
            missionControllers[maxMission % 20].OpenMission();
        }
    }
    public void OnResultFight(FightResult result)
    {
        if (result == FightResult.Win)
        {
            if (infoMission != null)
            {
                infoMission.MissionWin();
            }
            else
            {
                Debug.Log("это была не миссия компании");
            }
        }
        if (mission == null) { WarTableController.Instance.FinishMission(); }
        infoMission = null;
        mission = null;
    }

    public void SelectMission(MissionController infoMission)
    {
        Debug.Log("Select mission open");
        this.infoMission = infoMission;
        this.mission = infoMission.mission;
        WarTableController.Instance.OpenMission(infoMission.mission, this.OnOpenCloseMission);
        FightController.Instance.RegisterOnFightResult(OnResultFight);
    }
    private void OnOpenCloseMission(bool isOpen)
    {
        if (isOpen == false)
        {
            WarTableController.Instance.UnregisterOnOpenCloseMission(OnOpenCloseMission);
            MenuController.Instance.OpenMainPage();
            Open();
        }
        else
        {
            Close();
        }
    }
    private static CampaignBuilding instance;
    public static CampaignBuilding Instance { get => instance; }
    void Awake()
    {
        instance = this;
    }

    private BuildingWithFightTeamsModel campaingSaveObject;
    public DateTime GetAutoFightPreviousDate { get => campaingSaveObject.GetRecordDate(NAME_RECORD_AUTOFIGHT_PREVIOUS_DATETIME); }


    //Auto fight
    public void SaveAutoFight(DateTime newDateTime)
    {
        campaingSaveObject.SetRecordDate(NAME_RECORD_AUTOFIGHT_PREVIOUS_DATETIME, newDateTime);
    }
}
