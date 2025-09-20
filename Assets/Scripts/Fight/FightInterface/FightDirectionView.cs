using Fight.Common.HeroControllers.Generals;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Fight.Common.FightInterface
{
    public class FightDirectionView : UiView
    {
        public MelleeAtackDirectionController melleeAttackController;
        public Button btnSpell, btnWait;
        public GameObject panelControllers;
    }
}