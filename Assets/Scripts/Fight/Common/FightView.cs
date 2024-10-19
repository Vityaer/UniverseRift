using Fight;
using UnityEngine;
using VContainerUi.Abstraction;

namespace Assets.Scripts.Fight.Common
{
    public class FightView : UiView
    {
        [field: SerializeField] public LocationController LocationController { get; private set; }
    }
}
