using Fight.Common.Strikes;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Fight.HeroControllers.Generals.Attacks
{
    public class MelleeAttack : AbstractAttack
    {
        public override IEnumerator Attacking(HeroController target, int bonusStamina = 30)
        {
            if (Hero.StatusState.PermissionMakeStrike(TypeStrike.Physical))
            {
                yield return StartCoroutine(Hero.CheckFlipX(target));
                Hero.StatusState.ChangeStamina(bonusStamina);

                TryDispose();
                Disposable = OnMakeDamage.Subscribe(_ => Attack(target));

                yield return StartCoroutine(Hero.PlayAnimation(Constants.Visual.ANIMATOR_ATTACK_NAME_HASH));

            }
        }
    }
}
