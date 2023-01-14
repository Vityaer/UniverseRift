using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class SliderTimeScript : MonoBehaviour{
    [SerializeField] private Slider slider;
	[SerializeField] private Image fillImage;
	private DateTime finishTime, startTime;
	public TextMeshProUGUI textTime;
	public TypeSliderTime typeSlider = TypeSliderTime.Remainder; 
	public Color lowValue, fillValue;
	int waitSeconds = 0;
	DateTime deltaTime;
	TimeSpan interval, generalInterval;
	float secondsInterval = 1f; 
	float t = 0f;
	private Coroutine coroutineTimer;
	private bool isSetData = false;
	private bool _isFinish = false;
	public void ChangeValue()
	{
		switch(typeSlider){
			case TypeSliderTime.Remainder:
				interval = generalInterval - (DateTime.Now - startTime);
				waitSeconds = (int) interval.TotalSeconds;
				t =  (float) (waitSeconds / secondsInterval);
				if(waitSeconds <= 0) 
					SetFinish();
				break;

			case TypeSliderTime.Accumulation:
				interval = DateTime.Now - startTime;
				waitSeconds = (int) interval.TotalSeconds;
				t = (float) (waitSeconds / secondsInterval);
				Debug.Log($"accumulation: {waitSeconds}, all time: {secondsInterval}, t = {t}");
				if(t >= 1)
				{
					interval = generalInterval;
					SetFinish();
				}
				else
				{
					_isFinish = false;
				}
				break;	
		}
		if(!_isFinish)
		{
			fillImage.color = Color.Lerp(lowValue, fillValue, t);
			slider.value = t;
			textTime.text = FunctionHelp.TimeSpanConvertToSmallString(interval);
		}
	}
	
	public void SetMaxValue(TimeSpan requireTime)
	{
		textTime.text = FunctionHelp.TimeSpanConvertToSmallString(requireTime); 
	}

	public void SetData(DateTime startTime, TimeSpan requireTime)
	{
		this.startTime = startTime;
		this.finishTime = startTime + requireTime;
		generalInterval = requireTime;
		secondsInterval = (float) requireTime.TotalSeconds;
		ChangeValue();
		isSetData = true;

		if(gameObject.activeInHierarchy)
			StartTimer();
	}

	private void StartTimer()
	{
		if(!_isFinish)
		{
			if(coroutineTimer == null)
				coroutineTimer = StartCoroutine(CoroutineTimer());
		}
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
		if(!_isFinish){
			_isFinish = true;
			StopTimer();
			textTime.text = "Готово!";
			OnFinish();
		}
	}
	IEnumerator CoroutineTimer(){
	 	while(true){
	 		ChangeValue();
			yield return new WaitForSeconds(GetSecondForUpdateTimer(interval));
	 	}
    }
    void OnEnable(){
    	if(isSetData && gameObject.activeInHierarchy) StartTimer();
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
    	return result;
    }

    private Action observerFinish;
    public void RegisterOnFinish(Action d){observerFinish += d;}
    public void UnregisterOnFinish(Action d){observerFinish -= d;}
	
    private void OnFinish()
	{
		if(observerFinish != null)
		{
			observerFinish(); 
			observerFinish = null;
		}
	}
}
public enum TypeSliderTime{
	Remainder,
	Accumulation
}