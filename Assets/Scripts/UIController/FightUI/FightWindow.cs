using Fight;
using Fight.FightInterface;
using Fight.Grid;
using UnityEngine;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.FightUI
{
    public class FightWindow : WindowBase
    {
        public override string Name => nameof(FightWindow);

        public FightWindow(IObjectResolver container) : base(container)
        {
        }

        protected override void AddControllers()
        {
            AddController<FightController>();
            AddController<GridController>();
            AddController<FightDirectionController>();
        }
    }
}
