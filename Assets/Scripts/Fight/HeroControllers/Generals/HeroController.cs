using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fight.AI;
using Fight.Common.Strikes;
using Fight.Grid;
using Fight.HeroControllers.Generals.Attacks;
using Fight.HeroControllers.Generals.Movements;
using Fight.HeroStates;
using Fight.Misc;
using Fight.Rounds;
using Hero;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using Utils.AsyncUtils;
using VContainer;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        [Inject] public FightController FightController;
        [Inject] protected BotProvider _botProvider;
        [Inject] protected GridController _gridController;

        [Header("Components")]
        [SerializeField] private HeroStatus _statusState;
        [SerializeField] private AbstractMovement _movement;

        private AbstractAttack _attack;

        [Header("Characteristics")]
        [SerializeField] private float _speedMove = 2f;
        [SerializeField] private int _maxCountCounterAttack = 1;

        private bool _isFacingRight = true;
        private float _speedAnimation = 1f;
        private Side _side = Side.Left;

        [SerializeField] private GameHeroFight _hero;
        protected Transform Self;
        protected bool onGround = true;
        protected Rigidbody Rigidbody;
        protected bool needFlip = false;
        protected Vector3 delta = new Vector2(-0.6f, 0f);
        protected int hitCount = 0;
        protected int CurrentCountCounterAttack = 1;
        protected HexagonCell MyPlace;

        private bool _canWait;
        private HeroController _selectHero;
        private float _damageFromStrike;
        private bool _isDeath = false;
        private CancellationTokenSource _actionTokenSource;
        private CompositeDisposable _disposables = new();
        private IDisposable _currentActionDisposable;

        private bool _isFastFight;
        
        public GameHeroFight Hero => _hero;
        public bool IsDeath => _isDeath;
        public Vector3 GetPosition => Self.position;
        public bool CanRetaliation => _hero.Model.Characteristics.Main.CanRetaliation && CurrentCountCounterAttack > 0;
        public HexagonCell Cell => MyPlace;
        public bool Mellee => _attack is MelleeAttack;
        public TypeStrike TypeStrike => _hero.Model.Characteristics.Main.AttackType;
        public int Stamina => _statusState.Stamina;
        public List<HeroController> ListTarget { get; set; } = new();
        public bool CanWait => _canWait;
        public bool IsFacingRight => _isFacingRight;
        public float SpeedMove => _speedMove;
        public Side Side => _side;
        public HeroStatus StatusState => _statusState;
        public bool IsFastFight => _isFastFight;
        
        void Awake()
        {
            ListTarget = new();
            Self = base.transform;
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _onStartFight.Execute(this);
            IsSide(_side);
        }

        public void SetData(GameHero gameHero, HexagonCell place, Side side, bool isFastFight)
        {
            _isFastFight = isFastFight;
            _bodyParent.localRotation = Quaternion.Euler(0, 90, 0);
            FightController.OnEndRound.Subscribe(_ => RefreshOnEndRound()).AddTo(_disposables);
            MyPlace = place;
            place.SetHero(this);
            _side = side;
            _hero = new GameHeroFight(gameHero, _statusState);
            _hero.PrepareSkills(this);

            _attack.Init(this);
            _movement.Init(_gridController, this, MyPlace, _currentHeroVisualModel.Animator);

            _movement.OnChangePlace.Subscribe(newPlace => MyPlace = newPlace).AddTo(_disposables);
        }

        public void Refresh()
        {
            _canWait = true;
        }

        public void DoTurn()
        {
            AddFightRecordActionMe();
            if (!_isDeath && _statusState.PermissionAction())
            {
                PrepareOnStartTurn();
            }
            else
            {
                EndTurn();
            }
        }

        protected void EndTurn()
        {
            if (_isFacingRight ^ (_side == Side.Left))
                FlipX();

            ClearAction();
            OnEndAction();
            Animator.Play(ANIMATION_IDLE);
            FightController.RemoveHeroWithActionAll(this);
        }


        protected virtual void FindAvailableCells()
        {
            if (!_isFastFight)
            {
                MyPlace.StartCheckMove(
                    _hero.Model.Characteristics.Main.Speed,
                    this,
                    playerCanController: !_botProvider.CheckMeOnSubmission(_side)
                );
                if (!_botProvider.CheckMeOnSubmission(_side))
                    ShowHeroesPlaceInteractive();
            }
            else
            {
                MyPlace.StartCheckMove(
                    _hero.Model.Characteristics.Main.Speed,
                    this,
                    false
                );
            }
        }

        protected virtual void WaitingSelectTarget()
        {
            HexagonCell.RegisterOnClick(SelectHexagonCell);
        }

        public virtual void SelectHexagonCell(HexagonCell cell)
        {
            if (cell == MyPlace)
            {
                EndTurn();
                return;
            }
                
            if (cell.CanStand)
            {
                StartMelleeAttackOtherHero(cell, null);
            }
            else
            {
                if (cell.Hero != null)
                {
                    if (CanAttackHero(cell.Hero))
                    {
                        _selectHero = cell.Hero;
                        if (_attack is MelleeAttack)
                        {
                            if (CanMelleeAttack())
                            {
                                if (cell.GetCanAttackCell)
                                {
                                    cell.RegisterOnSelectDirection(SelectDirectionAttack);
                                    HexagonCell.UnregisterOnClick(SelectHexagonCell);
                                }
                            }
                        }
                        else
                        {
                            StartDistanceAttackOtherHero(_selectHero);
                        }
                    }
                }
            }
        }

        public void SelectDirectionAttack(HexagonCell targetCell)
        {
            HexagonCell.UnregisterOnClick(SelectHexagonCell);
            StartMelleeAttackOtherHero(targetCell, _selectHero);
        }

        public void SelectDirectionAttack(HexagonCell targetCell, HeroController otherHero)
        {
            HexagonCell.UnregisterOnClick(SelectHexagonCell);
            StartMelleeAttackOtherHero(targetCell, otherHero);
        }

        //Attack
        protected void StartMelleeAttackOtherHero(HexagonCell targetCell, HeroController enemy)
        {
            OnEndSelectCell();
            _actionTokenSource.TryCancel();
            _actionTokenSource = new();
            IMelleeAttackOtherHero(targetCell, enemy, _actionTokenSource.Token).Forget();
        }

        public void StartDistanceAttackOtherHero(HeroController enemy)
        {
            OnEndSelectCell();
            _actionTokenSource.TryCancel();
            _actionTokenSource = new();
            IDistanceAttackOtherHero(enemy, 20, _actionTokenSource.Token).Forget();
        }

        private async UniTask IMelleeAttackOtherHero(HexagonCell targetCell, HeroController heroForAttack,
            CancellationToken token)
        {
            if (targetCell != MyPlace)
            {
                await _movement.Move(targetCell);
            }
            
            if (heroForAttack != null)
            {
                if(!_isFastFight)
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                
                await _attack.Attacking(heroForAttack, 30);

                if (!heroForAttack.IsDeath)
                {
                    await heroForAttack.GetRetaliation(this);
                }
            }
                
            if(!_isFastFight)
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                
            SetFaceDefault();
            EndTurn();
        }

        protected async UniTask IDistanceAttackOtherHero(HeroController otherHero, int bonusStamina = 20,
            CancellationToken token = default)
        {
            hitCount = 0;
            if(!IsFastFight)
                await CheckFlipX(otherHero);
            
            _currentActionDisposable = _currentHeroVisualModel.AttackController
                .OnRangeDamage.Subscribe(AddHitCount);

            _attack.Attacking(otherHero, bonusStamina);

            if(!IsFastFight)
                await PlayAnimation(Constants.Visual.ANIMATOR_DISTANCE_ATTACK_NAME_HASH);
            
            if(!IsFastFight)
                await UniTask.WaitUntil(() => hitCount == 1, cancellationToken: token);

            //RemoveFightRecordActionMe();

            SetFaceDefault();

            _onStrike.Execute(new List<HeroController> { otherHero });
            EndTurn();
        }

        protected void SetFaceDefault()
        {
            if (needFlip)
            {
                FlipX();
                needFlip = false;
            }
        }

        public async UniTask CheckFlipX(HeroController otherHero)
        {
            needFlip = NeedFlip(otherHero);
            if (needFlip)
                FlipX();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }

        public virtual async UniTask GetRetaliation(HeroController attackedHero)
        {
            if (IsDeath)
                return;

            if(!CanRetaliation)
                return;
            
            await IGetRetaliation(attackedHero);
        }

        protected virtual async UniTask IGetRetaliation(HeroController attackedHero)
        {
            if (CanCounterAttack(this, attackedHero))
            {
                CurrentCountCounterAttack -= 1;
                await _attack.Attacking(attackedHero, 15);
                SetFaceDefault();
            }
        }

        protected void AddHitCount(HeroController target)
        {
            CreateDamage(target);
            hitCount += 1;
        }

        protected virtual void DoSpell()
        {
            _statusState.ChangeStamina(-100);
            _onSpell.Execute(ListTarget);
            EndTurn();
        }
        //End action 	
        protected void ClearAction()
        {
            OnEndSelectCell();
            OutlineController.SwitchOff();
        }

        public virtual void ApplyDamage(Strike strike)
        {
            if (_statusState.PermissionGetStrike(strike))
            {
                _onTakeDamage.Execute(this);
                _hero.GetDamage(strike);
                if (_hero.Health > 0f)
                {
                    PlayAnimation(Constants.Visual.ANIMATOR_GET_DAMAGE_NAME_HASH).Forget();
                }
                else
                {
                    _isDeath = true;
                    MyPlace.ClearSublject();
                        PlayAnimation
                        (
                            Constants.Visual.ANIMATOR_DEATH_NAME_HASH,
                            onAnimationFinish: OffAnimator
                        ).Forget();
                }
            }
        }

        private void OffAnimator()
        {
            Animator.speed = 0f;
            Animator.enabled = false;
            Death();
        }

        public virtual void GetHeal(float heal, RoundTypeNumber typeNumber = RoundTypeNumber.Num)
        {
            _hero.GetHeal(heal, typeNumber);
            _statusState.ChangeHP(_hero.Health);
            FightEffectController.Instance.CreateHealObject(Self);
            _onHeal.Execute((this, heal));
        }

        public virtual void ChangeMaxHP(int amountChange, RoundTypeNumber typeNumber = RoundTypeNumber.Num)
        {
            _hero.ChangeMaxHP(amountChange, typeNumber);
            _statusState.ChangeMaxHP(amountChange);
        }

        public void DeleteHero()
        {
            MyPlace?.ClearSublject();
            _sequenceAnimation.Kill();
            _actionTokenSource.TryCancel();
            Destroy(gameObject);
        }

        public void ClickOnMe()
        {
            MyPlace?.ClickOnMe();
        }

        private void Death()
        {
            _statusState.Death();
            ClearAction();
            FightController.DeleteHero(this);
            DeleteHero();
        }

        public void MessageDamageAfterStrike(float damage)
        {
            _damageFromStrike += damage;
        }

        public virtual void CreateDamage(HeroController enemy)
        {
            _currentActionDisposable.Dispose();
            enemy.ApplyDamage(new Strike(_hero.Model.Characteristics.Damage, _hero.Model.Characteristics.Main.Attack, typeStrike: TypeStrike));
        }
        //Event
        public void GetListForSpell(List<HeroController> listTarget)
        {
            _statusState.ChangeStamina(-100);
            _onListSpell.Execute(listTarget);
        }

        protected void AddFightRecordActionMe()
        {
            FightController.AddHeroWithAction(this);
        }

        protected void RemoveFightRecordActionMe()
        {
            FightController.RemoveHeroWithAction(this);
        }
    }
}