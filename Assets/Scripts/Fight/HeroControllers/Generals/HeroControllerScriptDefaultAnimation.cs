using DG.Tweening;
using Fight.Common.Strikes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.HeroControllers.Generals
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
        [SerializeField] bool flagAnimFinish = false;
        Dictionary<string, bool> animationsExist = new Dictionary<string, bool>();
        Tween sequenceAnimation;

        protected void PlayAnimation(string nameAnimation, Action defaultAnimation = null, bool withRecord = true)
        {
            if (withRecord == true)
                AddFightRecordActionMe();

            flagAnimFinish = false;
            if (CheckExistAnimation(nameAnimation))
            {
                anim.Play(nameAnimation);
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
                bool animExist = anim.HasState(0, stateId);
                animationsExist.Add(nameAnimation, animExist);
                result = animExist;
            }
            return result;
        }

        private void DefaultAnimDistanceAttack(List<HeroController> enemies)
        {
            GameObject arrow;
            hitCount = 0;
            this.listTarget = enemies;
            foreach (HeroController target in listTarget)
            {
                arrow = Instantiate(hero.PrefabArrow, tr.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().SetTarget(target, new Strike(hero.characts.Damage, hero.characts.GeneralAttack, typeStrike: typeStrike, isMellee: false));
                arrow.GetComponent<Arrow>().RegisterOnCollision(HitCount);
            }
        }

        protected void DefaultAnimAttack(HeroController enemy)
        {
            Debug.Log("default anim attack");
            sequenceAnimation?.Kill();
            Vector3 rotateAttack = Vector3.zero;
            rotateAttack = new Vector3(0, 0, isFacingRight ? 45 : -45);
            sequenceAnimation = DOTween.Sequence()
                        .Append(tr.DORotate(rotateAttack, 0.25f))
                        .Append(tr.DORotate(Vector3.zero, 0.25f).OnComplete(() => { GiveDamage(enemy); FinishAnimation(); }));
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
            sequenceAnimation?.Kill();

            bool attackFromLeft = NeedFlip(FightController.Instance.GetCurrentHero());
            Vector3 rotateGiveDamage = new Vector3(0, 0, attackFromLeft ? -45 : 45);
            sequenceAnimation = DOTween.Sequence().Append(tr.DORotate(rotateGiveDamage, 0.25f))
                    .Append(tr.DORotate(Vector3.zero, 0.25f).OnComplete(() => { FinishAnimation(); }));
        }

        private void DefaultAnimDeath()
        {
            isDeath = true;
            sequenceAnimation?.Kill();
            sequenceAnimation = DOTween.Sequence()
                .Append(tr.DOScaleY(0f, 0.5f).OnComplete(() => { FinishAnimation(); Death(); }));
        }
    }
}