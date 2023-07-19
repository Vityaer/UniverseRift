using City.Buildings.Abstractions;
using UnityEngine;

namespace City.Buildings.Tower
{
    public class ChallengeTowerView : BaseBuildingView
    {
        [field: SerializeField] public RectTransform Content { get; private set; }
        [field: SerializeField] public TowerMissionCotroller MissionPrefab { get; private set; }
    }
}
