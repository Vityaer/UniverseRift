using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterControllerScript : MonoBehaviour{
	public List<CampaignChapter> listChapters = new List<CampaignChapter>();
	public CampaignChapter GetCampaignChapter(int numMission){ return listChapters[numMission / 20]; }
	void Awake(){
		instance = this;
	}
	private static ChapterControllerScript instance;
	public static ChapterControllerScript Instance{get => instance;}
}
