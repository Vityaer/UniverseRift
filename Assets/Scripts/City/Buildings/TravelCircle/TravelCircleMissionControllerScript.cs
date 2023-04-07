using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TravelCircleMissionControllerScript : BaseMissionController
{
	[Header("UI")]
	public RewardUIControllerScript rewardController;
	public TextMeshProUGUI textNumMission, textOnButtonSelect;
	public GameObject buttonSelect, imageCloseMission;
	public Image backgoundMission;
	[SerializeField] private Button mainButton;

	private MissionWithSmashReward mission;
	private StatusMission status = StatusMission.NotOpen;
	private int numMission = 0;

	private void Start()
	{
		mainButton.onClick.AddListener(OpenMission);
	}

	public void SetData(MissionWithSmashReward mission, int numMission)
	{
		gameObject.SetActive(true);
		status = StatusMission.NotOpen;
		this.mission        = mission;
		this.numMission     = numMission;
		UpdateUI();
	}

	private void UpdateUI()
	{
		textNumMission.text = numMission.ToString();
		switch(status){
			case StatusMission.Open:
				rewardController.ShowReward(mission.WinReward);
				textOnButtonSelect.text = "Вызвать";
				buttonSelect.SetActive(true);
				imageCloseMission.SetActive(false);
				break;
			case StatusMission.InAutoFight:
				rewardController.ShowReward(mission.SmashReward);
				textOnButtonSelect.text = "Рейд";
				buttonSelect.SetActive(true);
				imageCloseMission.SetActive(false);
				break;
			case StatusMission.NotOpen:
				rewardController.ShowReward(mission.WinReward);
				buttonSelect.SetActive(false);
				imageCloseMission.SetActive(true);
				break;
		}			
	}

	public void OpenForFight()
	{
		status = StatusMission.Open;
		UpdateUI();
	}

	public void Hide()
	{
		status = StatusMission.Complete;
		gameObject.SetActive(false);
	}

	public void SetCanSmash()
	{
		status = StatusMission.InAutoFight;
		UpdateUI();
	}

	public void OpenMission()
	{
		switch(status){
			case StatusMission.Open:
				TravelCircleScript.Instance.OpenMission(this.mission);
				break;
			case StatusMission.InAutoFight:
				break;	
			case StatusMission.NotOpen:
				MessageControllerScript.Instance.AddMessage("Миссия ещё не открыта");		
				break;
		}
	}

	protected override MyScrollRect GetScrollParent()
	{
		return TravelCircleScript.Instance.scrollRectController;
	} 
}