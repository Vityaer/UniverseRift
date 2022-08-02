using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class DamageHealTextScript : MonoBehaviour{
	private bool inWork = false;
	public bool InWork{get => inWork;}
	[SerializeField] private TextMeshProUGUI textComponent;
	[SerializeField] private RectTransform rectTransform;
	public Color colorDamage, colorHeal;
	public Vector2 delta = new Vector2(0, 100f);
	public float speed = 1f;
	public void PlayDamage(float damage, Vector2 pos){
		PlayInfo(damage, pos, colorDamage);
	}
	public void PlayHeal(float heal, Vector2 pos){
		PlayInfo(heal, pos, colorHeal);
	}
	private void PlayInfo(float amount, Vector2 pos, Color color){
		if(inWork == false){
			inWork = true;
			textComponent.color = colorDamage;  
			textComponent.text  = amount.ToString();
			gameObject.SetActive(true); 
			textComponent.DOFade(1f, 0.05f);
			rectTransform.anchoredPosition = pos;
			rectTransform.DOAnchorPos(new Vector2(pos.x + delta.x, pos.y + delta.y), speed).OnComplete(Disable);
		}
	}
	public void Disable(){
		textComponent.DOFade(0f, 0.25f).OnComplete(ClearText);
		inWork = false;
	} 
	public void ClearText(){
		gameObject.SetActive(false); 
		textComponent.text = string.Empty;
	}
}
