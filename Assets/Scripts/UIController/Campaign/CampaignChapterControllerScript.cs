using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignChapterControllerScript : MonoBehaviour{

	public Image image; 
	public Text name;
	public CampaignChapter chapter;
	public bool isOpen = false;

	void Start(){
		name.text = string.Concat(chapter.numChapter.ToString(), ". ", chapter.Name );
	}
	public void Select(){
		CampaignScript.Instance.OpenChapter(this.chapter);
	}

	public void Open(){
		image.color = Color.white;
		isOpen = true;
	}
}
