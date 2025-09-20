using Effects;
using System.Collections.Generic;
using Fight.Common.FightInterface;
using Fight.Common.Grid;
using Fight.Common.HeroControllers.VisualModels;
using Fight.Common.Misc;
using UnityEngine;
using VContainer;

namespace Fight.Common.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        [Inject] private FightDirectionController _directionController;

        [SerializeField] private Transform _bodyParent;

        [SerializeField] private List<HeroVisualModel> _stages = new(); 

        private SpriteRenderer _spriteRenderer;
        private HeroVisualModel _currentHeroVisualModel;

        public HeroVisualModel HeroVisualModel => _currentHeroVisualModel;
        public Sprite Avatar => _currentHeroVisualModel.Avatar;
        public OutlineController OutlineController => _currentHeroVisualModel.OutlineController;
        public Animator Animator => _currentHeroVisualModel.Animator;
        public bool SpellExist => CheckExistAnimation(Constants.Visual.ANIMATOR_SPELL_NAME_HASH);
        public List<HeroVisualModel> Stages => _stages;

        public SpriteRenderer GetSpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = _bodyParent.Find("Body").GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        public void SetStage(int index)
        {
            _currentHeroVisualModel?.Deactivate();

            index = Mathf.Clamp(index, 0, _stages.Count);
            _currentHeroVisualModel = _stages[index];
            _currentHeroVisualModel.Activate();

            m_attack = _currentHeroVisualModel.AttackController;
            if (_currentHeroVisualModel.OverrideMovement)
                _movement = _currentHeroVisualModel.MovementController;
        }

        protected virtual void PrepareOnStartTurn()
        {
            needFlip = false;
            FindAvailableCells();
            ListTarget.Clear();

            if (this.m_side == Misc.Side.Left)
            {
                _directionController.OpenControllers(this);
                OutlineController.SwitchOn();
                WaitingSelectTarget();
            }
            else
            {
                _directionController.CloseControllers();
                if (m_isFastFight)
                {
                    WaitingSelectTarget();
                }
            }

            OnStartAction();
        }

        private bool NeedFlip(HeroController enemy)
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
        protected virtual bool CanAttackHero(HeroController otherHero) => (this.m_side != otherHero.m_side);

        protected void RefreshOnEndRound()
        {
            CurrentCountCounterAttack = m_hero.GetBaseCharacteristic.CountCouterAttack;
        }

        protected void ShakeCamera()
        {
            CameraShake.Shake(0.8f, 2f, CameraShake.ShakeMode.XY);
        }

        protected void IsSide(Side side)
        {
            this.m_side = side;
            delta = (side == Misc.Side.Left) ? new Vector2(-0.6f, 0f) : new Vector2(0.6f, 0f);
            if (side == Misc.Side.Right) FlipX();
        }

        protected bool CanCounterAttack(HeroController heroForCounterAttack, HeroController heroWasAttack)
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
            _onSpell.Execute(ListTarget);
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
                Color color = Color.red;
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