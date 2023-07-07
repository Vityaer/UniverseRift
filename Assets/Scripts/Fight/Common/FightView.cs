using Fight;
using TMPro;
using UnityEngine;
using VContainer;
using VContainerUi.Abstraction;

namespace Assets.Scripts.Fight.Common
{
    public class FightView : UiView
    {
        [field: SerializeField] public TextMeshProUGUI NumRoundText { get; private set; }
        [field: SerializeField] public LocationController LocationController { get; private set; }

    }
}
