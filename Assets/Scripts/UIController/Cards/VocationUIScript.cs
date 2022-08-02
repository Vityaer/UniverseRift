using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;
public class VocationUIScript : MonoBehaviour{
	[SerializeField] private Image imageVocation; 
	public void SetData(Vocation newVocation){ imageVocation.sprite = SystemSprites.Instance.GetSprite(newVocation); }
}
