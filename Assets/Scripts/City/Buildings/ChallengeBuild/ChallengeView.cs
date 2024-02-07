using City.Buildings.Abstractions;
using UnityEngine;

namespace City.Buildings.ChallengeBuild
{
    public class ChallengeView : BaseBuildingView
    {
        [SerializeField] private Transform Content;
        [SerializeField] private GameObject PrefabChallengeUI;
    }
}
