using Assets.Scripts.Fight;
using Models.Fights.Campaign;
using TMPro;
using UIController;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textNameMission;
    public TextMeshProUGUI textAutoRewardGold, textAutoRewardExperience, textAutoRewardStone;
    public Image backgoundMission;
    public GameObject infoFotter;
    public GameObject blockPanel;
    public GameObject imageAutoFight;
    public GameObject btnGoFight;
    public TextMeshProUGUI textBtnGoFight;
    public RewardUIController rewardController;
    [Header("Contollers")]
    public StatusMission statusMission;
    public CampaignMission mission;
    private int numMission;

    //API
    public void SetMission(CampaignMission mission, int numMission)
    {
        this.mission = (CampaignMission)mission.Clone();
        this.numMission = numMission;
        backgoundMission.sprite = LocationController.Instance.GetBackgroundForMission(this.mission.Location);
        textNameMission.text = numMission.ToString();
        statusMission = StatusMission.NotOpen;
        UpdateUI();
    }

    public void SetAutoFight()
    {
        AutoFight.Instance.SelectMissionAutoFight(this);
    }

    public void ClickOnMission()
    {
        if (statusMission != StatusMission.Complete)
        {
            if (statusMission == StatusMission.Open)
            {
                CampaignBuilding.Instance.SelectMission(this);
            }
        }
        else
        {
            if (statusMission != StatusMission.InAutoFight)
            {
                AutoFight.Instance.SelectMissionAutoFight(this);
            }
        }
    }

    public void UpdateAutoRewardUI()
    {
        if ((statusMission == StatusMission.Complete) || (statusMission == StatusMission.InAutoFight))
        {
            infoFotter.SetActive(true);
            textAutoRewardGold.text = string.Concat(this.mission.AutoFightReward.resources.List.Find(x => x.Name == TypeResource.Gold).ToString(), "/ 5sec");
            textAutoRewardExperience.text = string.Concat(this.mission.AutoFightReward.resources.List.Find(x => x.Name == TypeResource.Exp).ToString(), "/ 5sec");
            textAutoRewardStone.text = string.Concat(this.mission.AutoFightReward.resources.List.Find(x => x.Name == TypeResource.ContinuumStone).ToString(), "/ 5sec");
        }
    }

    public void StartAutoFight()
    {
        statusMission = StatusMission.InAutoFight;
        imageAutoFight.SetActive(true);
        btnGoFight.SetActive(false);
    }

    public void StopAutoFight()
    {
        statusMission = StatusMission.Complete;
        imageAutoFight.SetActive(false);
        btnGoFight.SetActive(true);
    }

    public void MissionWin()
    {
        statusMission = StatusMission.InAutoFight;
        AutoFight.Instance.SelectMissionAutoFight(this);
        UpdateAutoRewardUI();
        UpdateUI();
        StartAutoFight();
        CampaignBuilding.Instance.OpenNextMission();
    }

    public void CompletedMission()
    {
        statusMission = StatusMission.Complete;
        UpdateUI();
        UpdateAutoRewardUI();
    }

    public void OpenMission()
    {
        statusMission = StatusMission.Open;
        blockPanel.SetActive(false);
        UpdateUI();
    }

    private void UpdateUI()
    {
        switch (statusMission)
        {
            case StatusMission.NotOpen:
                blockPanel.SetActive(true);
                infoFotter.SetActive(false);
                btnGoFight.SetActive(false);
                rewardController.CloseReward();
                break;
            case StatusMission.Open:
                rewardController.ShowReward(this.mission.WinReward);
                rewardController.OpenReward();
                textBtnGoFight.text = "Вызвать";
                break;
            case StatusMission.Complete:
            case StatusMission.InAutoFight:
                rewardController.ShowAutoReward(this.mission.AutoFightReward);
                rewardController.OpenReward();
                textBtnGoFight.text = "Авто";
                break;
        }
        int status = (int)statusMission;
        btnGoFight.SetActive((status == 1) || (status == 2));
        blockPanel.SetActive(false);
    }
}