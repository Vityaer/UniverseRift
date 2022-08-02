using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
public class OpenClosePanel : MonoBehaviour{
	public void Open(){
		gameObject.SetActive(true);
	}
	public void Close(){
		gameObject.SetActive(false);
		OnClose();
	}

//Observers
	private Action observerClose;
	public void RegisterOnClose(Action d){observerClose += d;}
	public void UnregisterOnClose(Action d){observerClose -= d;}
	private void OnClose(){if(observerClose != null) observerClose();}
}
public enum AnimationTypeChangeActive{
	ComeLeft,
	ComeRight,
	ComeUp,
	ComeDown
}