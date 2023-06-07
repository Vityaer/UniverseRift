using Assets.Scripts.City.General;
using UIController;
using UnityEngine;

public class AutoFight : MainPage
{
    public Canvas canvasAutoFight;
    public MissionController missionAutoFight;
    public GoldHeap heap;
    [Header("UI")]
    public GameObject background;
    private static AutoFight instance;
    public static AutoFight Instance { get => instance; }

    protected override void Awake()
    {
        instance = this;
        canvasAutoFight = GetComponent<Canvas>();
        base.Awake();
    }

    public void SelectMissionAutoFight(MissionController infoMission)
    {
        missionAutoFight?.StopAutoFight();
        missionAutoFight = infoMission;
        missionAutoFight.StartAutoFight();
        heap.SetNewReward(missionAutoFight.mission.AutoFightReward);
    }

    public void SelectMissionAutoFight(CampaignMission mission)
    {
        heap.SetNewReward(mission.AutoFightReward);
    }

    public override void Open()
    {
        base.Open();
        canvasAutoFight.enabled = true;
        BackgroundController.Instance.OpenBackground(background);
        heap.OnOpenSheet();
    }
    public override void Close()
    {
        heap.OnCloseSheet();
        canvasAutoFight.enabled = false;
    }
}
