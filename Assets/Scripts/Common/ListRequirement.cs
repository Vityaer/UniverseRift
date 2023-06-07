using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [CreateAssetMenu(fileName = "RequirementTasks", menuName = "Custom ScriptableObject/RequirementTasks", order = 59)]
    [System.Serializable]
    public class ListRequirement : ScriptableObject
    {
        public List<Achievement> listRequirement = new List<Achievement>();
    }
}