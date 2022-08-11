using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using IdleGame.Touch;
public class SliderCityScript : MonoBehaviour{

	Vector2 leftPos, rightPos;
	void Start(){
		leftPos = new Vector2(-ScreenSize.X, 0);
		rightPos = new Vector2(ScreenSize.X, 0);
		curSheet = ListCitySheet.Count / 2;
		SetStartPosition();
		MainTouchControllerScript.Instance?.RegisterOnObserverSwipe(OnSwipe);
	}
	private void OnSwipe(TypeSwipe typeSwipe){
		switch(typeSwipe){
			case TypeSwipe.Left:
				SwipeLeft();
				break;
			case TypeSwipe.Right:
				SwipeRight();
				break;	
		}
	}
	void OnDisable(){
		MainTouchControllerScript.Instance.UnregisterOnObserverSwipe(OnSwipe);
	}
    void OnEnable(){
		MainTouchControllerScript.Instance?.RegisterOnObserverSwipe(OnSwipe);
    }
	public int curSheet;
	public float scaleX = 3f, timeAnimMove = 0.25f;
	public List<Transform> ListCitySheet = new List<Transform>();
	public void SwipeLeft(){
			ListCitySheet[curSheet].DOMove(rightPos, timeAnimMove);
			if(curSheet > 0) curSheet--;
			ListCitySheet[curSheet].DOMove(Vector2.zero, timeAnimMove);
	}
	public void SwipeRight(){
			ListCitySheet[curSheet].DOMove(leftPos, timeAnimMove);
			if(curSheet < ListCitySheet.Count - 1) curSheet++;
			ListCitySheet[curSheet].DOMove(Vector2.zero, timeAnimMove);
	}
	private void SetStartPosition(){
		for(int i=0; i < curSheet; i++){
			ListCitySheet[i].DOMove(leftPos, 0f);
		}
		for(int i=curSheet + 1; i < ListCitySheet.Count; i++){
			ListCitySheet[i].DOMove(rightPos, 0f);
		}
	}

}
