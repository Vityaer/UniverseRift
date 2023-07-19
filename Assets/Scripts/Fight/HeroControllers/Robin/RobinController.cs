using Fight.Common.Strikes;
using Fight.HeroControllers.Generals;
using UnityEngine;

namespace Fight.HeroControllers.Robin
{
    public class RobinController : HeroController
    {
        private int _hitSpecialCount = 0;
        [Header("Spell Data")]
        public GameObject robinArrow;

        private void CreateRobinArrow()
        {
            FightController.ChooseEnemies(Side, 3, listTarget);
            foreach (HeroController target in listTarget)
            {
                var arrow = Instantiate(robinArrow, Self.position, Quaternion.identity).GetComponent<RobinArrow>();
                arrow.SetTarget(target, new Strike(hero.Model.Characteristics.Damage, hero.Model.Characteristics.Main.Attack, typeStrike: typeStrike, isMellee: false));
                arrow.RegisterOnCollision(OnSpecialHit);
            }
        }

        private void OnSpecialHit()
        {
            _hitSpecialCount++;
            if (_hitSpecialCount == listTarget.Count)
            {
                RemoveFightRecordActionMe();
                OnSpell(listTarget);
                EndTurn();
            }
        }

        protected override void DoSpell()
        {
            AddFightRecordActionMe();
            _hitSpecialCount = 0;
            statusState.ChangeStamina(-100);
            Animator.Play("Spell");
        }
    }
}