using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RequirementTasks", menuName = "Custom ScriptableObject/RequirementTasks", order = 59)]
[System.Serializable]
public class ListRequirement : ScriptableObject{
	public List<Requirement> listRequirement = new List<Requirement>();
}