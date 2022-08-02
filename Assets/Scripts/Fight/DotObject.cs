using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotObject : MonoBehaviour{
	void Start(){
		transform.Find("Sprite").GetComponent<SpriteRenderer>().DOFade(0f, duration: 1f);
		Destroy(gameObject, 1.25f);
	}
}
