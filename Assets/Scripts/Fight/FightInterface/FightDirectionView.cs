using Fight.HeroControllers.Generals;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Fight.FightInterface
{
    public class FightDirectionView : UiView
    {
        public FightDirectionController SelectDirection;
        public Button btnSpell, btnWait;
        public RectTransform panelControllers;
    }
}