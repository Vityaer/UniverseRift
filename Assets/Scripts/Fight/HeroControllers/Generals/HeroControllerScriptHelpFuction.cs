using Effects;
using Fight.FightInterface;
using Fight.Grid;
using Fight.Misc;
using UnityEngine;
using VContainer;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        [Inject] private FightDirectionController _directionController;

        [SerializeField] bool isFacingRight = true;

        [SerializeField] private Transform BodyParent;
        public OutlineController outlineController;

        private SpriteRenderer _spriteRenderer;
        public Sprite GetSprite => GetSpriteRenderer.sprite;
        public bool SpellExist => CheckExistAnimation(ANIMATION_SPELL);

        public SpriteRenderer GetSpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = BodyParent.Find("Body").GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        protected virtual void PrepareOnStartTurn()
        {
            needFlip = false;
            FindAvailableCells();
            listTarget.Clear();

            if (this.Side == Side.Left)
            {
                _directionController.OpenControllers(this);
                outlineController.SwitchOn();
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
            locScale.x *= -1;
            BodyParent.localScale = locScale;
        }

        //Fight helps
        protected virtual bool CanAttackHero(HeroController otherHero) => (this.Side != otherHero.Side);

        protected void RefreshOnEndRound()
        {
            currentCountCounterAttack = hero.GetBaseCharacteristic.CountCouterAttack;
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
            if ((currentCountCounterAttack > 0) && (statusState.PermissionAction() == true) && !isDeath)
            {
                result = true;
            }
            return result;
        }

        private void CreateSpell()
        {
            listTarget.Clear();
            OnSpell(listTarget);
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