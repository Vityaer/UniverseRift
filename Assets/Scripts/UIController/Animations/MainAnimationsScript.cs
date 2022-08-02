using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MainAnimationsScript : MonoBehaviour{
	public RectTransform rect;
	private Vector2 startSize = new Vector2(1f, 1f);
	private float minSize = 0.95f, maxSize = 1.05f;
	public float time = 0.1f;
	void Start(){
		if(rect == null) rect = GetComponent<RectTransform>();
	}
	Tween sequenceScalePulse;
	public void ScalePulse(){
		sequenceScalePulse = DOTween.Sequence()
						.Append(rect.DOScale(startSize * minSize, time))
						.Append(rect.DOScale(startSize, time));
	}
	public void Squezze(){
		if(sequenceScalePulse != null) sequenceScalePulse.Kill();
		sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize * minSize, time));
						
	}
	public void SquezzeToDefault(){
		if(sequenceScalePulse != null) sequenceScalePulse.Kill();
		sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize, time));
	}
	public void Expansion(){
		if(sequenceScalePulse != null) sequenceScalePulse.Kill();
		sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize * maxSize, time));
	}
	public void ExpansionToDefault(){
		if(sequenceScalePulse != null) sequenceScalePulse.Kill();
		sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize, time));
	}
}
