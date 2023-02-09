using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FooterButtonScript : MonoBehaviour{
	private Image btnBackGround;
	private RectTransform btnSprite;
	public Vector2 startSize;
    public Color colorSelected, colorUnSelected;
    
    void Awake(){
    	btnSprite = transform.Find("ButtonImage").GetComponent<RectTransform>();
    	btnBackGround = GetComponent<Image>();
    	startSize = btnSprite.localScale;
    }

	public void Select(){
		btnSprite.DOScale(startSize * 1.1f, 0.25f);
		btnBackGround.color = colorSelected;
		Change(isOpen: true);
    }

    public void UnSelect(){
		btnSprite.DOScale(startSize, 0.25f);
    	btnBackGround.color = colorUnSelected;
    	Change(isOpen: false);
    }

    public delegate void Del(bool isOpen);
	private Del delsOpenClose;
	public void RegisterOnChange  (Del d){ delsOpenClose += d; }
	public void UnRegisterOnChange(Del d){ delsOpenClose -= d; }
	public void Change(bool isOpen){
		if(delsOpenClose != null)
			delsOpenClose(isOpen);	
	} 
}
