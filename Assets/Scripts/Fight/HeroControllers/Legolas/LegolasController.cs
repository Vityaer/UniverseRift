using Fight.Grid;
using Fight.HeroControllers.Generals;
using Fight.HeroControllers.Robin;
using UnityEngine;

namespace Fight.HeroControllers.Legolas
{
    public class LegolasController : HeroController
    {
        [SerializeField] private GameObject _arrow;
        private HeroController _selectedEnemy;

        protected override void DoSpell()
        {
            AddFightRecordActionMe();
            HexagonCell.UnregisterOnClick(SelectHexagonCell);
            HexagonCell.RegisterOnClick(SelectEnemy);
        }

        public virtual void SelectEnemy(HexagonCell cell)
        {
            if (cell.Hero == null)
                return;

            if (cell.Hero.Side == this.Side)
                return;

            _selectedEnemy = cell.Hero;

            HexagonCell.UnregisterOnClick(SelectEnemy);
            statusState.ChangeStamina(-100);
            Animator.Play("Spell");
        }

        private void CreateLegolasArrow()
        {
            var arrow = Instantiate(_arrow, Self.position, Quaternion.identity).GetComponent<RobinArrow>();
        }
    }
}
