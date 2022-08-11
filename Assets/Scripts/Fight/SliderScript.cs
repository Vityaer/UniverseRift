using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class SliderScript : MonoBehaviour{
	
	[SerializeField] private Slider slider;
	[SerializeField] private Image fillImage;
	public float maxValue = 1f;
	public Color lowValue;
	public Color fillValue;
	Tween sequenceChangeValue;
	public void SetData(float curValue, float maxValue){
		SetMaxValue(maxValue);
		ChangeValue(curValue);
	}
	public void ChangeValue(float value){
		if(maxValue > 0){
			float t = (float) value / maxValue;
			if(sequenceChangeValue != null) sequenceChangeValue.Kill();
			sequenceChangeValue = DOTween.Sequence()
			.Append(slider.DOValue(t, 0.5f))
			.Join(fillImage.DOColor(Color.Lerp(lowValue, fillValue, t), 0.5f).OnComplete(() => CheckMaxFill(value)));
		}
	}
	private void CheckMaxFill(float value){ if(value >= maxValue){ OnFillMaxSlider(); }}
	public void SetMaxValue(float maxValue){
		if(gameObject.activeSelf == false) gameObject.SetActive(true);
		this.maxValue = maxValue;
		ChangeValue(this.maxValue);
	} 
	public void Death(){
		sequenceChangeValue = DOTween.Sequence()
				.Append(slider.DOValue(0f, 0.5f))
				.Join(fillImage.DOColor(lowValue, 0.5f).OnComplete(OffSlider));
	
	}
	public void OffSlider(){
		gameObject.SetActive(false);
	}

	Action observerMaxFill;
	public void RegisterOnFillSliderInMax(Action d){ observerMaxFill += d; }
	public void UnregisterOnFillSliderInMax(Action d){ observerMaxFill -= d; }
	private void OnFillMaxSlider(){if(observerMaxFill != null) observerMaxFill();}
}
