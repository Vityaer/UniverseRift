using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEventController : MonoBehaviour{
	public List<EventPanel> listSimpleEvent = new List<EventPanel>();
	public void Open(){
		foreach(EventPanel panel in listSimpleEvent){
			panel.Show();
		}
	}
}