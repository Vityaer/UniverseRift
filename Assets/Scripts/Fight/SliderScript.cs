using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderScript : MonoBehaviour{
	
	private Slider slider;
	private Image fillImage;
	public float maxValue = 1;
	void Awake(){
		fillImage = transform.Find("Fill Area/Fill").GetComponent<Image>();
		slider = GetComponent<Slider>();
	}
	public Color lowValue;
	public Color fillValue;
	public void ChangeValue(float value){
		if(maxValue > 0){
			float t = (float) value / maxValue;
			fillImage.DOColor(Color.Lerp(lowValue, fillValue, t), 0.5f);
			slider.DOValue(t, 0.5f);
		}
	}
	public void SetMaxValue(float maxValue){
		if(gameObject.activeSelf == false) gameObject.SetActive(true);
		this.maxValue = maxValue;
		ChangeValue(this.maxValue);
	} 
	public void Death(){
		fillImage.color = lowValue;
		slider.DOValue(0f, 0.5f).OnComplete(OffSlider);
	}
	public void OffSlider(){
		gameObject.SetActive(false);
	}
}
