using Effects;
using Fight.FightInterface;
using Fight.Grid;
using Fight.HeroControllers.VisualModels;
using Fight.Misc;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        [Inject] private FightDirectionController _directionController;

        [SerializeField] bool isFacingRight = true;

        [SerializeField] private Transform BodyParent;

        [SerializeField] private List<HeroVisualModel> _stages = new(); 

        private SpriteRenderer _spriteRenderer;
        private HeroVisualModel _currentHeroVisualModel;

        public Sprite Avatar => _currentHeroVisualModel.Avatar;
        public OutlineController OutlineController => _currentHeroVisualModel.OutlineController;
        public Animator Animator => _currentHeroVisualModel.Animator;
        public bool SpellExist => CheckExistAnimation(ANIMATION_SPELL);
        public List<HeroVisualModel> Stages => _stages;

        public SpriteRenderer GetSpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = BodyParent.Find("Body").GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        public void SetStage(int index)
        {
            if (!ReferenceEquals(_currentHeroVisualModel, null))
                _currentHeroVisualModel.Deactivate();

            index = Mathf.Clamp(index, 0, _stages.Count);
            _currentHeroVisualModel = _stages[index];
            _currentHeroVisualModel.Activate();
        }

        protected virtual void PrepareOnStartTurn()
        {
            needFlip = false;
            FindAvailableCells();
            ListTarget.Clear();

            if (this.Side == Side.Left)
            {
                _directionController.OpenControllers(this);
                OutlineController.SwitchOn();
                WaitingSelectTarget();
            }
            else
            {
                _directionController.CloseControllers();
            }

            OnStartAction();
        }

        private bool NeedFlip(HeroController enemy)
        {
            bool result = false;
            CellDirectionType direction = NeighbourCell.GetDirection(myPlace, enemy.Cell);
            switch (direction)
            {
                case CellDirectionType.UpLeft:
                case CellDirectionType.Left:
                case CellDirectionType.BottomLeft:
                    result = (isFacingRight == true);
                    break;
                case CellDirectionType.UpRight:
                case CellDirectionType.Right:
                case CellDirectionType.BottomRight:
                    result = (isFacingRight == false);
                    break;
            }

            return result;
        }

        protected void FlipX()
        {
            isFacingRight = !isFacingRight;
            Vector3 locScale = BodyParent.localScale;
            locScale.z *= -1;
            BodyParent.localScale = locScale;
        }

        //Fight helps
        protected virtual bool CanAttackHero(HeroController otherHero) => (this.Side != otherHero.Side);

        protected void RefreshOnEndRound()
        {
            CurrentCountCounterAttack = hero.GetBaseCharacteristic.CountCouterAttack;
        }

        protected void ShakeCamera()
        {
            CameraShake.Shake(0.8f, 2f, CameraShake.ShakeMode.XY);
        }

        protected void IsSide(Side side)
        {
            this.Side = side;
            delta = (side == Side.Left) ? new Vector2(-0.6f, 0f) : new Vector2(0.6f, 0f);
            if (side == Side.Right) FlipX();
        }

        protected bool CanCounterAttack(HeroController heroForCounterAttack, HeroController heroWasAttack)
        {
            bool result = false;
            if ((CurrentCountCounterAttack > 0) && (statusState.PermissionAction() == true) && !_isDeath)
            {
                result = true;
            }
            return result;
        }

        private void CreateSpell()
        {
            ListTarget.Clear();
            OnSpell(ListTarget);
            EndTurn();
        }

        private void ShowHeroesPlaceInteractive()
        {
            foreach (var warrior in FightController.GetLeftTeam)
            {
                Color color;
                if (warrior.Cell.GetAchivableNeighbourCell() == null || !CanShoot())
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
                if (warrior.Cell.GetAchivableNeighbourCell() == null || !CanShoot())
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

        private void SetMyPlaceColor()
        {
            if (Side == Side.Left)
            {
                myPlace.SetColor(Constants.Colors.NOT_ACHIEVABLE_FRIEND_CELL_COLOR);
            }
            else
            {
                myPlace.SetColor(Constants.Colors.NOT_ACHIEVABLE_ENEMY_CELL_COLOR);
            }
        }

        private bool CanShoot()
        {
            return (hero.Model.Characteristics.Main.Mellee == true) || (!hero.Model.Characteristics.Main.Mellee && myPlace.MyEnemyNear(this.Side));
        }

        [ContextMenu("Add 100 stamina")]
        private void AddBonus100Stamina()
        {
            statusState.ChangeStamina(100);
        }
    }
}