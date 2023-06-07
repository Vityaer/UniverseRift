using Assets.Scripts.Fight;
using Fight.Common.Strikes;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Fight.HeroControllers.Legolas
{
    public class LegolasArrow : Arrow
    {
        [SerializeField] private float _increaseMultiplier = 1f;
        private float _multiplier = 1f;
        private RaycastHit2D[] _targets;
        private int _currentHit;

        public override void SetTarget(HeroController target, Strike strike)
        {
            var heroLayer = LayerMask.GetMask("Hero");
            _targets = Physics2D.RaycastAll(transform.position, target.GetPosition - tr.position, Mathf.Infinity, heroLayer);

            base.SetTarget(target, strike);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == _targets[_currentHit])
            {
                CollisionTarget(other.GetComponent<HeroController>());

                if (_currentHit + 1 < _targets.Length)
                {
                    _currentHit++;
                }
                else
                {
                    if (delsCollision != null)
                        delsCollision();
                    OffArrow();
                }
            }
        }

        protected override void CollisionTarget(HeroController target)
        {
            strike.bonusPercent = _multiplier * 100f;

            if (strike != null)
                target.GetDamage(strike);

            _multiplier += _increaseMultiplier;

        }
    }
}