using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fight.Common.Strikes;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Fight.Common.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
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
        private Dictionary<int, bool> _animationsExist = new();
        private Tween _sequenceAnimation;

        public async UniTask PlayAnimation(int nameAnimation, Action defaultAnimation = null, bool withRecord = true, Action onAnimationFinish = null)
        {
            if (!IsFastFight)
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

                var state = Animator.GetCurrentAnimatorStateInfo(0);
                await UniTask.Delay(TimeSpan.FromSeconds(state.length));
                onAnimationFinish?.Invoke();

                if (withRecord)
                    RemoveFightRecordActionMe();
            }
            else
            {
                if (withRecord == true)
                    AddFightRecordActionMe();
                
                onAnimationFinish?.Invoke();
                
                if (withRecord)
                    RemoveFightRecordActionMe();
            }
        }

        protected bool CheckExistAnimation(int nameHash)
        {
            var result = false;
            if (_animationsExist.ContainsKey(nameHash))
            {
                result = _animationsExist[nameHash];
            }
            else
            {
                bool animExist = Animator.HasState(0, nameHash);
                _animationsExist.Add(nameHash, animExist);
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
            _sequenceAnimation?.Kill();
            _sequenceAnimation = DOTween.Sequence()
                .Append(Self.DOScaleY(0f, 0.5f).OnComplete(() => { FinishAnimation();  }));
        }

        private void OnDestroy()
        {
            _sequenceAnimation?.Kill();
        }
    }
}