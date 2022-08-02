using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUIControllerScript : MonoBehaviour{

	public List<SubjectCellControllerScript> cells = new List<SubjectCellControllerScript>();
	public Transform panelRewards;
	public GameObject btnAllReward;
	private Reward reward;
	public void ShowReward(Reward reward, bool lengthReward = false){
		this.reward  = reward;
		if(cells.Count == 0) GetCells();
		if(btnAllReward != null) btnAllReward.SetActive((reward.Count > 4) && (lengthReward == false));
		for(int i = 0; (i < 4) && (i < reward.Count); i++) cells[i].SetItem(reward.GetList[i] as VisualAPI);
		for(int i = reward.Count; i < cells.Count; i++) cells[i].OffCell();
	}
	public void ShowAllReward(Reward reward){
		this.reward  = reward;
		if(cells.Count == 0) GetCells();
		for(int i = 0; i < reward.Count; i++){cells[i].SetItem(reward.GetList[i]);}
		for(int i = reward.Count; i < cells.Count; i++) cells[i].OffCell();
	}
	public void ShowAutoReward(AutoReward autoReward){
		if(cells.Count == 0) GetCells();
		List<PosibleRewardObject> listPosibleObject = new List<PosibleRewardObject>();
		autoReward.GetListPosibleRewards(listPosibleObject);
		if(btnAllReward != null) btnAllReward.SetActive(listPosibleObject.Count > 4);
		for(int i = 0; (i < 4) && (i < listPosibleObject.Count); i++) cells[i].SetItem(listPosibleObject[i]);
		for(int i = listPosibleObject.Count; i < cells.Count; i++) cells[i].OffCell();
	}
    void GetCells(){
    	foreach(Transform cell in panelRewards){
			cells.Add(cell.GetComponent<SubjectCellControllerScript>());
		}
    }
    public void OpenAllReward(){ BoxAllRewards.Instance.ShowAll(this.reward); }
    public void CloseReward(){ gameObject.SetActive(false);}
    public void OpenReward(){gameObject.SetActive(true);}
}
