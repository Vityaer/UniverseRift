using DG.Tweening;
using Fight.Common.Strikes;
using System;
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
        [SerializeField] bool flagAnimFinish = false;
        Dictionary<string, bool> animationsExist = new Dictionary<string, bool>();
        private Tween _sequenceAnimation;

        protected void PlayAnimation(string nameAnimation, Action defaultAnimation = null, bool withRecord = true)
        {
            if (withRecord == true)
                AddFightRecordActionMe();

            flagAnimFinish = false;
            if (CheckExistAnimation(nameAnimation))
            {
                Animator.Play(nameAnimation);
            }
            else
            {
                if (defaultAnimation != null)
                    defaultAnimation();
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

        private Arrow prefabArrow;
        private void DefaultAnimDistanceAttack(List<HeroController> enemies)
        {
            hitCount = 0;
            this.listTarget = enemies;
            prefabArrow ??= Resources.Load<Arrow>("CreateObjects/Bullet");

            foreach (HeroController target in listTarget)
            {
                var arrow = Instantiate(prefabArrow, transform.position, Quaternion.identity);
                arrow.SetTarget(target, new Strike(hero.Model.Characteristics.Damage, hero.Model.Characteristics.Main.Attack, typeStrike: typeStrike, isMellee: false));
                arrow.RegisterOnCollision(HitCount);
            }
        }

        protected void DefaultAnimAttack(HeroController enemy)
        {
            _sequenceAnimation?.Kill();
            Vector3 rotateAttack = Vector3.zero;
            rotateAttack = new Vector3(0, 0, isFacingRight ? 45 : -45);
            _sequenceAnimation = DOTween.Sequence()
                        .Append(Self.DORotate(rotateAttack, 0.25f))
                        .Append(Self.DORotate(Vector3.zero, 0.25f).OnComplete(() => { GiveDamage(enemy); FinishAnimation(); }));
        }

        private void DefaultAnimIdle()
        {
        }

        private void DefaultAnimMove()
        {
        }

        private void FinishAnimation()
        {
            RemoveFightRecordActionMe();
            flagAnimFinish = true;
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
            isDeath = true;
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