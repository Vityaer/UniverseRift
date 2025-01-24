using Fight.Common.Strikes;
using Fight.HeroControllers.Generals;
using Models.Heroes.Actions;

namespace Models.Heroes.Skills.Actions.Imps.SimpleActions
{
    public class DamageAction : ContinuousAction
    {
        public override void ExecuteAction()
        {
            foreach (HeroController heroController in ListTarget)
            {
                heroController.ApplyDamage(
                    new Strike(
                        Amount,
                        heroController.Hero.Model.Characteristics.Main.Attack,
                        typeNumber: TypeNumber
                        )
                    );
            }
        }
    }
}
