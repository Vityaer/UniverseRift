using City.Buildings.Abstractions;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.PageCycleEvent.MonthlyEvents
{
    public class MonthlyEventView : BaseBuildingView
    {
        [field: SerializeField] public Button ArenaOpenButton { get; private set; }
        [field: SerializeField] public Button TravelOpenButton { get; private set; }
        [field: SerializeField] public Button EvolutionOpenButton { get; private set; }
        [field: SerializeField] public Button TaskBoardsOpenButton { get; private set; }
    }
}
