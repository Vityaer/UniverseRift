using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Sirenix.Serialization;
public class RaceUIScript : MonoBehaviour{
	[SerializeField] private Image imageRace; 
	public void SetData(Race newRace){ imageRace.sprite = SystemSprites.Instance.GetSprite(newRace); }
}