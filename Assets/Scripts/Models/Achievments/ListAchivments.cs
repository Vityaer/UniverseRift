using System.Collections.Generic;
using UnityEngine;

namespace Models.Achievments
{
    [CreateAssetMenu(fileName = "RequirementTasks", menuName = "Custom ScriptableObject/RequirementTasks", order = 59)]
    [System.Serializable]
    public class ListAchivments : ScriptableObject
    {
        public List<AchievmentModel> listRequirement = new List<AchievmentModel>();
    }
}