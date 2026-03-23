using Cysharp.Threading.Tasks;
using Fight.Common.Strikes;
using UniRx;
using UnityEditor;
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

                    AnimationClip attackClip = null;
                    if (Hero.ViewAnimator.runtimeAnimatorController != null)
                    {
                        foreach (AnimationClip clip in Hero.ViewAnimator.runtimeAnimatorController.animationClips)
                        {
                            if (Animator.StringToHash(clip.name) == Constants.Visual.ANIMATOR_ATTACK_NAME_HASH)
                            {
                                attackClip = clip;
                                break;
                            }
                        }
                    }

                    bool eventExist = false;
                    if (attackClip != null)
                    {
                        AnimationEvent[] events = AnimationUtility.GetAnimationEvents(attackClip);

                        if (events.Length > 0)
                        {
                            eventExist = true;
                            Disposable = OnMakeDamage.Subscribe(_ => Attack(target));
                        }
                        else
                        {
                            Debug.LogError($"Animation clip: {attackClip.name} not contains any animation events {transform.parent.name}", attackClip);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Animation clip: Attack not found. {transform.parent.name}", gameObject);
                    }

                    await Hero.PlayAnimation(Constants.Visual.ANIMATOR_ATTACK_NAME_HASH);

                    if (!eventExist)
                    {
                        Debug.LogError($"Create attack on finish animation {transform.parent.name}", gameObject);
                        Attack(target);
                    }
                }
                else
                {
                    Attack(target);
                    Hero.StatusState.ChangeStamina(bonusStamina);
                }
        }
    }
}