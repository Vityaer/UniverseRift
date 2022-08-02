using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class SliderTimeScript : MonoBehaviour{
    [SerializeField] private Slider slider;
	[SerializeField] private Image fillImage;
	private DateTime requireTime, startTime;
	public TextMeshProUGUI textTime;
	public TypeSliderTime typeSlider = TypeSliderTime.Remainder; 
	public Color lowValue, fillValue;
	int waitSeconds = 0;
	DateTime deltaTime;
	TimeSpan interval, generalInterval;
	float t = 0f;
	public void ChangeValue(){
		switch(typeSlider){
			case TypeSliderTime.Remainder:
				deltaTime = requireTime - (DateTime.Now - startTime);
				interval  = new TimeSpan(deltaTime.Hour, deltaTime.Minute, deltaTime.Second);
				generalInterval = new TimeSpan(requireTime.Hour, requireTime.Minute, requireTime.Second);  
				waitSeconds = (int) interval.TotalSeconds;
				t =  1f - (float) (waitSeconds / generalInterval.TotalSeconds);
				if(waitSeconds == 0) StopTimer();
				break;
			case TypeSliderTime.Accumulation:
				interval = DateTime.Now - startTime;
				generalInterval = new TimeSpan(requireTime.Hour, requireTime.Minute, requireTime.Second);  
				waitSeconds = (int) interval.TotalSeconds;
				t = (float) (waitSeconds / generalInterval.TotalSeconds);
				if(t >= 1){
					interval = generalInterval;
					StopTimer();
				}
				break;	
		}
		fillImage.color = Color.Lerp(lowValue, fillValue, t);
		slider.value = t;
		textTime.text = FunctionHelp.TimeSpanConvertToSmallString(interval);
	}
	public void SetMaxValue(DateTime requireTime){
		this.requireTime = requireTime;
		textTime.text = FunctionHelp.TimeSpanConvertToSmallString(interval); 
	}
	Coroutine coroutineTimer;
	private bool isSetData = false;
	public void SetData(DateTime startTime, DateTime requireTime){
		this.startTime = startTime;
		this.requireTime = requireTime;
		ChangeValue();
		isSetData = true;
		if(gameObject.activeSelf){
			StartTimer();
		}
	}
	private void StartTimer(){
		if(coroutineTimer == null)
			coroutineTimer = StartCoroutine(CoroutineTimer());
	}
	public void StopTimer(){
		if(coroutineTimer != null){
			StopCoroutine(coroutineTimer);
			coroutineTimer = null;
		}
	} 
	public void SetInfo(string str){
		textTime.text = str;
	}
	public void SetFinish(){
		StopTimer();
		textTime.text = "Готово!";
	}
	IEnumerator CoroutineTimer(){
	 	while(true){
	 		ChangeValue();
			yield return new WaitForSeconds(GetSecondForUpdateTimer(interval));
	 	}
    }
    void OnEnable(){
    	if(isSetData) StartTimer();
    }
    void OnDisable(){
    	StopTimer();
    }
    private float GetSecondForUpdateTimer(TimeSpan interval){
    	float result = 1f;
    	switch(typeSlider){
			case TypeSliderTime.Remainder:
		    	if(interval.Days > 0){
		    		result = interval.Hours * 3600 + interval.Minutes * 60 + interval.Seconds;
		    	}else if(interval.Hours > 0){
		    		result = interval.Minutes * 60 + interval.Seconds;
		    	}else if(interval.Minutes > 0){
		    		result = interval.Seconds;
		    	}else{
		    		result = 1f;
		    	}
		    	break;
		    case TypeSliderTime.Accumulation:
		    	if(interval.Days > 0){
		    		result = 3600 - (interval.Minutes * 60 + interval.Seconds);
		    	}else if(interval.Hours > 0){
		    		result = 60 - interval.Seconds;
		    	}else{
		    		result = 1f;
		    	}
		    	break;
		} 			
    	Debug.Log("GetSecondForUpdateTimer: " + result.ToString());
    	return result;
    }
}
public enum TypeSliderTime{
	Remainder,
	Accumulation
}