using Fight.HeroControllers.Generals;

namespace Fight.HeroControllers.Militia
{
    public class MilitiaController : HeroController
    {
        protected override void DoSpell()
        {
            statusState.ChangeStamina(-100);
            anim.Play("Spell");
        }

    }
}