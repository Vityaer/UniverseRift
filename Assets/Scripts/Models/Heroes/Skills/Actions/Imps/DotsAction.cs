using Fight.Common.HeroControllers.Generals;
using Fight.Common.HeroStates;
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
                heroController.StatusState.SetDot(DotType, Amount, TypeNumber, Rounds);
        }    }
}
