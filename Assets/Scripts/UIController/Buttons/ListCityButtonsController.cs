using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ListCityButtonsController : MonoBehaviour{
	[SerializeField] private float speedAnimation = 4f;
	[SerializeField] private RectTransform rectParent, buttonController;
	private bool isOpen = true;
	float height = 0f;
	void Start(){
		height = rectParent.rect.height;
	}
	[ContextMenu("ChangeState")]
	public void ChangeState(){
		if(coroutineChangeState != null) StopCoroutine(coroutineChangeState);
		if(isOpen){
			AnimationClose();
		}else{
			AnimationOpen();
		}
		Vector3 scale = buttonController.localScale;
		scale.y *= -1f;
		buttonController.localScale = scale;
		isOpen = !isOpen;
	}
	void AnimationClose(){
		coroutineChangeState = StartCoroutine(IChangeState(height));
	}
	void AnimationOpen(){
		coroutineChangeState = StartCoroutine(IChangeState(0f));
	}
	Coroutine coroutineChangeState;
	IEnumerator IChangeState(float targetHeight){
		Vector2 currentSize = rectParent.offsetMin;
		float startHeight = currentSize.y;
		float t = 0;
		while(t <= 1f){
			t += Time.deltaTime * speedAnimation;
			currentSize.y = Mathf.Lerp(startHeight, targetHeight, t);
			rectParent.offsetMin = currentSize;
			yield return null;
		} 
		currentSize.y = targetHeight;
		rectParent.offsetMin = currentSize;
	}
}