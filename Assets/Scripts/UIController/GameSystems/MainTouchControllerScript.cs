using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace IdleGame.Touch{
	public class MainTouchControllerScript : MonoBehaviour{
	    private Vector3 startPosition, endPosition;
		private Vector2 swipeDistance;
		private Action<TypeSwipe> observerSwipe;
		public float widthPercent = 25f;
		void Start(){
			swipeDistance.x = Screen.width * widthPercent / 100f;
		}
	    RaycastHit2D hit;
	    void Update(){
	    	if (Input.GetMouseButtonDown(0)){
				startPosition  = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp(0)){
				endPosition  = Input.mousePosition;
				if(Mathf.Abs(endPosition.y - startPosition.y) < swipeDistance.x){
					if(endPosition.x - startPosition.x > swipeDistance.x){
						OnSwipe(TypeSwipe.Left);
					}else if(startPosition.x - endPosition.x > swipeDistance.x){
						OnSwipe(TypeSwipe.Right);
					}
				}
			}
	    }
	    public void RegisterOnObserverSwipe(Action<TypeSwipe> d){observerSwipe += d;}
	    public void UnregisterOnObserverSwipe(Action<TypeSwipe> d){observerSwipe -= d;}
	    private void OnSwipe(TypeSwipe type){if(observerSwipe != null) observerSwipe(type);}
	    private static MainTouchControllerScript instance;
	    public static MainTouchControllerScript Instance{get => instance;}
	    void Awake(){
	    	instance = this;
	    }
	}
	public enum TypeSwipe{
		Left,
		Right
	}
} 