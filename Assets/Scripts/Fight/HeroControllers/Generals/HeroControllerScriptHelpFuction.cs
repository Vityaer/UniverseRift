using System.Collections.Generic;
using Effects;
using Fight.Common.FightInterface;
using Fight.Common.Grid;
using Fight.Common.HeroControllers.VisualModels;
using Fight.Common.Misc;
using UnityEngine;
using VContainer;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        [Inject] private FightDirectionController m_directionController;

        [SerializeField] private Transform _bodyParent;
        [SerializeField] private List<HeroVisualModel> _stages = new(); 

        private SpriteRenderer m_spriteRenderer;
        private HeroVisualModel m_currentHeroVisualModel;

        public HeroVisualModel CurrentHeroVisualModel => m_currentHeroVisualModel;
        public OutlineController OutlineController => m_currentHeroVisualModel.OutlineController;
        public Animator ViewAnimator => m_currentHeroVisualModel.Animator;
        public bool SpellExist => CheckExistAnimation(Constants.Visual.ANIMATOR_SPELL_NAME_HASH);
        public List<HeroVisualModel> Stages => _stages;

        public SpriteRenderer GetSpriteRenderer
        {
            get
            {
                if (m_spriteRenderer == null)
                    m_spriteRenderer = _bodyParent.Find("Body").GetComponent<SpriteRenderer>();
                return m_spriteRenderer;
            }
        }

        public void SetStage(int index)
        {
            if (m_currentHeroVisualModel == null)
            {
                m_currentHeroVisualModel = _stages[0];
            }

            m_currentHeroVisualModel.Deactivate();
            index = Mathf.Clamp(index, 0, _stages.Count);
            m_currentHeroVisualModel = _stages[index];
            m_currentHeroVisualModel.Activate();

            m_attack = m_currentHeroVisualModel.AttackController;
            if (m_currentHeroVisualModel.OverrideMovement)
                _movement = m_currentHeroVisualModel.MovementController;
        }

        protected virtual void PrepareOnStartTurn()
        {
            needFlip = false;
            FindAvailableCells();
            ListTarget.Clear();

            if (this.m_side == Common.Misc.Side.Left)
            {
                m_directionController.OpenControllers(this);
                OutlineController.SwitchOn();
                WaitingSelectTarget();
            }
            else
            {
                m_directionController.CloseControllers();
                if (m_isFastFight)
                {
                    WaitingSelectTarget();
                }
            }

            OnStartAction();
        }

        private bool NeedFlip(Fight.HeroControllers.Generals.HeroController enemy)
        {
            bool result = false;
            CellDirectionType direction = NeighbourCell.GetDirection(MyPlace, enemy.Cell);
            switch (direction)
            {
                case CellDirectionType.UpLeft:
                case CellDirectionType.Left:
                case CellDirectionType.BottomLeft:
                    result = (m_isFacingRight == true);
                    break;
                case CellDirectionType.UpRight:
                case CellDirectionType.Right:
                case CellDirectionType.BottomRight:
                    result = (m_isFacingRight == false);
                    break;
            }

            return result;
        }

        public void FlipX()
        {
            m_isFacingRight = !m_isFacingRight;
            Vector3 locScale = _bodyParent.localScale;
            locScale.z *= -1;
            _bodyParent.localScale = locScale;
        }

        //Fight helps
        protected virtual bool CanAttackHero(Fight.HeroControllers.Generals.HeroController otherHero) => (this.m_side != otherHero.m_side);

        private void RefreshOnEndRound()
        {
            CurrentCountCounterAttack = m_hero.GetBaseCharacteristic.CountCouterAttack;
        }

        protected void ShakeCamera()
        {
            CameraShake.Shake(0.8f, 2f, CameraShake.ShakeMode.XY);
        }

        private void IsSide(Side side)
        {
            this.m_side = side;
            delta = (side == Common.Misc.Side.Left) ? new Vector2(-0.6f, 0f) : new Vector2(0.6f, 0f);
            if (side == Common.Misc.Side.Right) FlipX();
        }

        private bool CanCounterAttack(Fight.HeroControllers.Generals.HeroController heroForCounterAttack, Fight.HeroControllers.Generals.HeroController heroWasAttack)
        {
            bool result = false;
            var isNeighbour = MyPlace.IsCellNeighbour(heroWasAttack.MyPlace);
            if (isNeighbour && (CurrentCountCounterAttack > 0) && (_statusState.PermissionAction() == true) && !m_isDeath)
            {
                result = true;
            }
            return result;
        }

        private void CreateSpell()
        {
            ListTarget.Clear();
            m_onSpell.Execute(ListTarget);
            EndTurn();
        }

        private void ShowHeroesPlaceInteractive()
        {
            foreach (var warrior in FightController.GetLeftTeam)
            {
                Color color;
                if (warrior.Cell.GetAchivableNeighbourCell() == null || !CanMelleeAttack())
                {
                    color = Constants.Colors.NOT_ACHIEVABLE_FRIEND_CELL_COLOR;
                }
                else
                {
                    color = Constants.Colors.NOT_ACHIEVABLE_FRIEND_CELL_COLOR;
                }
                warrior.Cell.SetColor(color);
            }

            foreach (var warrior in FightController.GetRightTeam)
            {
                Color color;
                if (warrior.Cell.GetAchivableNeighbourCell() == null || !CanMelleeAttack())
                {
                    color = Constants.Colors.ACHIEVABLE_ENEMY_CELL_COLOR;
                }
                else
                {
                    color = Constants.Colors.NOT_ACHIEVABLE_ENEMY_CELL_COLOR;
                }
                warrior.Cell.SetColor(color);
            }
        }

        private bool CanMelleeAttack()
        {
            return (m_hero.Model.Characteristics.Main.Mellee == true)
                || (!m_hero.Model.Characteristics.Main.Mellee && MyPlace.MyEnemyNear(this.m_side));
        }

        [ContextMenu("Add 100 stamina")]
        private void AddBonus100Stamina()
        {
            _statusState.ChangeStamina(100);
        }
    }
}