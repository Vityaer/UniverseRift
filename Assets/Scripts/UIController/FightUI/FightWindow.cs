using Fight.Common;
using Fight.Common.FightInterface;
using Fight.Common.Grid;
using UnityEngine;
using VContainer;
using VContainerUi.Abstraction;
using FightController = Fight.Common.FightController;

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
