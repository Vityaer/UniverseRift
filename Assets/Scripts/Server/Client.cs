using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Client : MonoBehaviour{
	public DateTime GetServerTime(){
		return DateTime.Now;
	}
	// public DateTime GetCurrentDay(){
	// 	return currentDay;
	// }
	// public DateTime GetCurrentWeek(){

	// }
	// public DateTime GetCurrentMonth(){

	// }

	void Awake(){ instance = this; }
	private static Client instance;
	public static Client Instance{get => instance;}
}
