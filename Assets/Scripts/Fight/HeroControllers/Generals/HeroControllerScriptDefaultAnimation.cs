using DG.Tweening;
using Fight.Common.Strikes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour, IDisposable
    {
        //Animations
        private const string ANIMATION_ATTACK = "Attack",
                             ANIMATION_GET_DAMAGE = "GetDamage",
                             ANIMATION_DEATH = "Death",
                             ANIMATION_MOVE = "Move",
                             ANIMATION_IDLE = "Idle",
                             ANIMATION_DISTANCE_ATTACK = "Shoot",
                             ANIMATION_SPELL = "Spell";

        private bool _flagAnimFinish = false;
        private Dictionary<string, bool> animationsExist = new Dictionary<string, bool>();
        private Tween _sequenceAnimation;

        protected IEnumerator PlayAnimation(string nameAnimation, Action defaultAnimation = null, bool withRecord = true, Action onAnimationFinish = null)
        {
            if (withRecord == true)
                AddFightRecordActionMe();

            _flagAnimFinish = false;
            if (CheckExistAnimation(nameAnimation))
            {
                Animator.Play(nameAnimation);
            }
            else
            {
                //if (defaultAnimation != null)
                //    defaultAnimation();
            }

            if (withRecord)
            {
                var state = Animator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitForSeconds(state.length);
                RemoveFightRecordActionMe();
                onAnimationFinish?.Invoke();
            }
        }

        protected bool CheckExistAnimation(string nameAnimation)
        {
            bool result = false;
            if (animationsExist.ContainsKey(nameAnimation))
            {
                result = animationsExist[nameAnimation];
            }
            else
            {
                int stateId = Animator.StringToHash(nameAnimation);
                bool animExist = Animator.HasState(0, stateId);
                animationsExist.Add(nameAnimation, animExist);
                result = animExist;
            }
            return result;
        }

        private void FinishAnimation()
        {
            RemoveFightRecordActionMe();
            _flagAnimFinish = true;
        }

        private void DefaultAnimGetDamage(Strike strike)
        {
            _sequenceAnimation?.Kill();

            var attackFromLeft = NeedFlip(FightController.GetCurrentHero());

            var rotateGiveDamage = new Vector3(0, 0, attackFromLeft ? -45 : 45);

            _sequenceAnimation = DOTween.Sequence().Append(Self.DORotate(rotateGiveDamage, 0.25f))
                    .Append(Self.DORotate(Vector3.zero, 0.25f).OnComplete(() => { FinishAnimation(); }));
        }

        private void DefaultAnimDeath()
        {
            _isDeath = true;
            _sequenceAnimation?.Kill();
            _sequenceAnimation = DOTween.Sequence()
                .Append(Self.DOScaleY(0f, 0.5f).OnComplete(() => { FinishAnimation(); Death(); }));
        }

        public void Dispose()
        {
            _sequenceAnimation?.Kill();
        }
    }
}