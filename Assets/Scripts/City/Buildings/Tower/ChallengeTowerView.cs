using City.Buildings.Abstractions;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Tower
{
    public class ChallengeTowerView : BaseBuildingView
    {
        [field: SerializeField] public RectTransform Content { get; private set; }
        [field: SerializeField] public TowerMissionCotroller MissionPrefab { get; private set; }
        [field: SerializeField] public ScrollRect ScrollRect { get; private set; }
    }
}
