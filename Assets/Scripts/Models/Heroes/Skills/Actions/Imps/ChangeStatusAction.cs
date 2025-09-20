using Fight.Common.HeroControllers.Generals;
using Fight.Common.HeroStates;
using Models.Heroes.Actions;

namespace Models.Heroes.Skills.Actions.Imps
{
    public class ChangeStatusAction : AbstractAction
    {
        public State StateType;

        public override void ExecuteAction()
        {
            foreach (HeroController heroController in ListTarget)
                heroController.StatusState.SetDebuff(StateType, (int)Amount);
        }
    }
}
