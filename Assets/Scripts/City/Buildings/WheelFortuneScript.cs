using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class WheelFortuneScript : Building{

	[Header("Controller")]
	[SerializeField] private ButtonCostScript buttonOneRotate, buttonTenRotate;
 	[Header("Images reward")]
	public List<SubjectCellControllerScript> places = new List<SubjectCellControllerScript>();

	[Header("List reward")]
	private List<FortuneReward> rewards = new List<FortuneReward>();

	public List<FortuneReward<Resource>> resources = new List<FortuneReward<Resource>>();
	public List<FortuneReward<Item>>     items     = new List<FortuneReward<Item>>();
	public List<FortuneReward<Splinter>> splinters = new List<FortuneReward<Splinter>>();

	[Header("Arrow")]
	public RectTransform arrowRect;
	public float arrowSpeed;
	private float previousTilt = 0f;

	private float generalProbability = 0f;
	[Header("Test")]
	public float testRandom = 0;
	protected override void OpenPage(){
		FillWheelFortune();
		CalculateProbability();

	}
	private Quaternion startRotation;
	Coroutine coroutineRotate;
	public void PlaySimpleRoulette(int coin = 1){
		coroutineRotate = StartCoroutine(IRotateArrow(GetRandom()));
		OnSimpleRotate(coin);
	}
	int numReward = 0;
	private float GetRandom(){
		float result = 0f;
		int k = 0;
		float rand = UnityEngine.Random.Range(0f, generalProbability);
		for(int i = 0; i < rewards.Count; i++){
			result += rewards[i].probability;
			if(result < rand){ k++; } else { break; } 
		}
		numReward = k;
		return (k * 45f + UnityEngine.Random.Range(-22.4f, 22.4f));
	}
	private void CalculateProbability(){
		generalProbability = 0f;
		foreach(FortuneReward reward in rewards)
			generalProbability += reward.probability;
	}
	private void FillWheelFortune(){
		for(int i = 0; i < rewards.Count; i++){
			switch(rewards[i]){
				case FortuneReward<Resource> product:
					places[i].SetItem((product as FortuneReward<Resource>).subject);
					break;
				case FortuneReward<Item> product:
					places[i].SetItem(product.subject);
					break;
				case FortuneReward<Splinter> product:
					places[i].SetItem(product.subject);
					break;		

			}
		}
	}

    IEnumerator IRotateArrow(float targetTilt){
    	float startTilt = (((int) (previousTilt / 360)) + 360f);
    	targetTilt = startTilt + targetTilt;
		float t = 0;
		Debug.Log("rotate stage 0");
		while(previousTilt > startTilt) previousTilt -= 360f;
		float delta = (startTilt - previousTilt)/360;
    	while(t <= 1){
		    arrowRect.rotation = Quaternion.Euler(0, 0, - Mathf.Lerp(previousTilt, startTilt, t) );
		    if(t < 0.36){
		    	t += Time.deltaTime * arrowSpeed * (1f/delta) * Mathf.Max(t, 0.1f);
		    }else{
		    	t += Time.deltaTime * arrowSpeed * (1f/delta);
		    }
	    	yield return null; 
		}
		Debug.Log("rotate stage 1");
		t = 0;
		while(t <= 1){
		    arrowRect.rotation = Quaternion.Euler(0, 0, - Mathf.Lerp(startTilt, 360 + startTilt, t) );
		    t += Time.deltaTime * arrowSpeed;
	    	yield return null; 
		}
		Debug.Log("rotate stage 2");
		t = 0;
		while(startTilt > targetTilt) startTilt -= 360f;
		delta = (targetTilt - startTilt)/360;
		while(t <= 1){
		    arrowRect.rotation = Quaternion.Euler(0, 0, - Mathf.Lerp(startTilt, targetTilt, t ) );
		    if(t < 0.64){
				t += Time.deltaTime * arrowSpeed * (1f/delta);
		    }else{
		    	t += Time.deltaTime * arrowSpeed * (1f/delta) * Mathf.Max(1 - t, 0.01f);
		    }
	    	yield return null; 
		}
		Debug.Log("finish rotate");
    	previousTilt = targetTilt;
    	GetReward(); 
	}
	private void GetReward(){
		switch( rewards[numReward] ){
			case FortuneReward<Resource> res:
				Resource rewardRes = (rewards[numReward] as FortuneReward<Resource>).subject as Resource;
				PlayerScript.Instance.AddResource( rewardRes );
				MessageControllerScript.Instance.AddMessage("Поздравляем! Награда - " + rewardRes.GetTextAmount() +" " + rewardRes.GetName() );
				break;
			case FortuneReward<Item> item:
				Item rewardItem = (rewards[numReward] as FortuneReward<Item>).subject as Item;
				MessageControllerScript.Instance.AddMessage("Поздравляем! Награда - предмет" );
				InventoryControllerScript.Instance.AddItem(rewardItem);
				break;
			case FortuneReward<Splinter> splinter:
			break;
		}
	}
	private void OneRotate(int count){
		PlaySimpleRoulette(1);
	}
	private void TenRotate(int count){
		PlaySimpleRoulette(10);
	}

	protected override void OnStart(){
		buttonOneRotate.RegisterOnBuy(OneRotate);
		buttonTenRotate.RegisterOnBuy(TenRotate);

		for(int i = 0; i < resources.Count; i++) rewards.Add(resources[i]);
		for(int i = 0; i < items.Count; i++) rewards.Add(items[i]);
		for(int i = 0; i < splinters.Count; i++) rewards.Add(splinters[i]);
		FortuneReward x = null;
		for(int i = 0; i < rewards.Count - 1; i++){
			for(int j = i + 1; j < rewards.Count; j++){
				if(rewards[i].posID > rewards[j].posID){
					x = rewards[i];
					rewards[i] = rewards[j];
					rewards[j] = x;
				}
			}
		}
		startRotation = arrowRect.rotation;
	}
	[ContextMenu("StartPosition")]
	public void StartPosition(){
		arrowRect.rotation = startRotation;
		if(coroutineRotate != null){
			StopCoroutine(coroutineRotate);
			coroutineRotate = null;
		}

	}
	private static WheelFortuneScript instance;
	public static WheelFortuneScript Instance{get => instance;}
	void Awake(){
		instance  = this;
	}
//Observers
	private Action<BigDigit> observerSimpleRotate;
	public void RegisterOnSimpleRotate(Action<BigDigit> d){observerSimpleRotate += d;}	 
	private void OnSimpleRotate(int amount) {if(observerSimpleRotate != null) observerSimpleRotate(new BigDigit(amount));}	 

}

[System.Serializable]
public class FortuneReward{
	public float probability;
	public int posID;
}
[System.Serializable]
public class FortuneReward<T>: FortuneReward where T : BaseObject{
	public T subject;
} 