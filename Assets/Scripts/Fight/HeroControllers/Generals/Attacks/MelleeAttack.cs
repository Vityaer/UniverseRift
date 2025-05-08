using Cysharp.Threading.Tasks;
using Fight.Common.Strikes;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Attacks
{
    public class MelleeAttack : AbstractAttack
    {
        public override async UniTask Attacking(HeroController target, int bonusStamina = 30)
        {
            if (Hero.StatusState.PermissionMakeStrike(TypeStrike.Physical))
                if (!Hero.IsFastFight)
                {
                    await Hero.CheckFlipX(target);
                    Hero.StatusState.ChangeStamina(bonusStamina);

                    TryDispose();
                    Disposable = OnMakeDamage.Subscribe(_ => Attack(target));

                    await Hero.PlayAnimation(Constants.Visual.ANIMATOR_ATTACK_NAME_HASH);
                }
                else
                {
                    Attack(target);
                    Hero.StatusState.ChangeStamina(bonusStamina);
                }
        }
    }
}