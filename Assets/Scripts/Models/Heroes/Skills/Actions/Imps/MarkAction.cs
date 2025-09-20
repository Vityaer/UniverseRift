using Fight.Common.HeroControllers.Generals;
using Fight.Common.HeroStates;
using Models.Heroes.Skills.Actions.Imps.SimpleActions;

namespace Models.Heroes.Skills.Actions.Imps
{
    public class MarkAction : ContinuousAction
    {
        public MarkType MarkType;

        public override void ExecuteAction()
        {

            foreach (HeroController heroController in ListTarget)
                heroController.StatusState.SetMark(MarkType, Amount, Rounds);
        }
    }
}
