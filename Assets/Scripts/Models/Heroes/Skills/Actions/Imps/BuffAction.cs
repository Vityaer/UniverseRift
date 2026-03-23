using Fight.Common.HeroControllers.Generals;
using Hero;
using Models.Heroes.Actions;
using HeroController = Fight.HeroControllers.Generals.HeroController;

namespace Models.Heroes.Skills.Actions.Imps
{
    public class BuffAction : AbstractAction
    {
        public Attachment Attachment;
        public override void ExecuteAction()
        {
            foreach (HeroController heroController in ListTarget)
                heroController.Hero.SetHate(Attachment, Amount);
        }
    }
}
