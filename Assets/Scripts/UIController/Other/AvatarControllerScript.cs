using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AvatarControllerScript : MonoBehaviour{
	public Image mainImage, borderImage;
	public int levelVip;

	public void SetAvatar(Sprite newAvatar){
		mainImage.sprite = newAvatar;
	}
}
