using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetControllerScript : MonoBehaviour{
	public List<Skill> Skills = new List<Skill>();
	public int level;

}

[CreateAssetMenu(fileName = "New Pet", menuName = "Custom ScriptableObject/Pet", order = 55)]
[System.Serializable]
public class PetAvatar : ScriptableObject, ICloneable{
	public string Name;
	public int ID;
	public string Description;
	public Sprite Avatar;
	public GameObject Prefab;
	public int Level;
	public int Rating;
	public List<Skill> Skills = new List<Skill>();



	public object Clone(){
		return new PetAvatar{
		};
	}
}

