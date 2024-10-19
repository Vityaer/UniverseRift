using Fight.HeroControllers.Generals;
using Fight.HeroStates;
using Models.Heroes.Actions;

namespace Models.Heroes.Skills.Actions.Imps
{
    public class ChangeStatusAction : AbstractAction
    {
        public State StateType;

        public override void ExecuteAction()
        {
            foreach (HeroController heroController in ListTarget)
                heroController.statusState.SetDebuff(StateType, (int)Amount);
        }
    }
}
