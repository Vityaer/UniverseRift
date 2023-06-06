using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class TowerMissionCotrollerScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
	private Mission mission;
	[Header("UI")]
	public TextMeshProUGUI textNumMission;
	public Image backgoundMission, enemyImage;
	public RewardUIController rewardController;
	public GameObject blockPanel;

	public bool canOpenMission = false;
	int numMission = 0;
	public void SetData(Mission mission, int numMission, bool canOpenMission = false){
		this.mission        = mission;
		this.numMission     = numMission;
		this.canOpenMission = canOpenMission;
		UpdateUI();
	}
	private void UpdateUI(){
		textNumMission.text = numMission.ToString();
		if(this.mission != null)
			rewardController.ShowReward(mission.WinReward);
			blockPanel.SetActive(canOpenMission == false);
	}
	public void OpenMission(){
		if(canOpenMission){
			ChallengeTower.Instance.OpenMission(this.mission);
		}else{
			MessageController.Instance.AddMessage("Миссия ещё не открыта");		
		}
	}

    public void OnBeginDrag(PointerEventData eventData){
    	ChallengeTower.Instance.scrollRectController.OnBeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData){
    	ChallengeTower.Instance.scrollRectController.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData){
    	ChallengeTower.Instance.scrollRectController.OnEndDrag(eventData);
    }
}
