using Fight.HeroControllers.Generals;
using Fight.HeroStates;
using Models.Heroes.Actions;
using Models.Heroes.Skills.Actions.Effects;
using Models.Heroes.Skills.Actions.Imps.SimpleActions;

namespace Models.Heroes.Skills.Actions.Imps
{
    public class DotsAction : ContinuousAction
    {
        public DotType DotType;

        public override void ExecuteAction()
        {
            foreach (HeroController heroController in ListTarget)
                heroController.statusState.SetDot(DotType, Amount, TypeNumber, Rounds);
        }    }
}
