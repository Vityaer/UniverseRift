using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StartLoadingControllerScript : MonoBehaviour{
	public GameObject panel;
	public SliderScript sliderLoadingGame;
	private TextMeshProUGUI textCurrentStageLoading;
	public void Close(){
		Debug.Log("close");
		sliderLoadingGame.UnregisterOnFillSliderInMax(Close);
		PlayerScript.Instance.UnregiterOnRegisterOnLoadGame(ChangeLoadedStage);
		panel.SetActive(false);
	}
	
	[SerializeField] private Vector2Int data = new Vector2Int(0, 0);
	private void ChangeLoadedStage(Vector2Int newData){
		data = newData;
		sliderLoadingGame.SetData(newData.x, newData.y);
	}
	void Start(){
		PlayerScript.Instance.RegiterOnRegisterOnLoadGame(ChangeLoadedStage);
		sliderLoadingGame.RegisterOnFillSliderInMax(Close);
	}
	void Awake(){
		if(instance == null){
			instance = this;
		}else{
			Debug.Log("twice component on " + gameObject.name);
		}
	} 
	private static StartLoadingControllerScript instance; 
	public static StartLoadingControllerScript Instance{get => instance;} 
}
