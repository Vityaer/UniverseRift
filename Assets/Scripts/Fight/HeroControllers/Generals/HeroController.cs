using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fight.Common.AI;
using Fight.Common.Strikes;
using Fight.Common.Grid;
using Fight.Common.HeroControllers.Generals.Attacks;
using Fight.Common.HeroControllers.Generals.Movements;
using Fight.Common.HeroStates;
using Fight.Common.Misc;
using Fight.Common.Rounds;
using Hero;
using UniRx;
using UnityEngine;
using Utils.AsyncUtils;
using VContainer;

namespace Fight.Common.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        [Inject] public Common.FightController FightController;
        [Inject] protected BotProvider BotProvider;
        [Inject] protected GridController GridController;

        [Header("Components")] [SerializeField]
        private HeroStatus _statusState;

        [SerializeField] private AbstractMovement _movement;

        private AbstractAttack m_attack;

        [Header("Characteristics")] [SerializeField]
        private float _speedMove = 2f;

        private bool m_isFacingRight = true;
        private float m_speedAnimation = 1f;
        private Side m_side = Side.Left;

        private GameHeroFight m_hero;
        protected Transform Self;
        protected bool OnGround = true;
        protected Rigidbody Rigidbody;
        protected bool needFlip = false;
        protected Vector3 delta = new Vector2(-0.6f, 0f);
        protected int hitCount = 0;
        protected int CurrentCountCounterAttack = 1;
        protected HexagonCell MyPlace;

        private bool m_canWait;
        private HeroController m_selectHero;
        private float m_damageFromStrike;
        private bool m_isDeath = false;
        private CancellationTokenSource m_actionTokenSource;
        private readonly CompositeDisposable m_disposables = new();
        private IDisposable m_currentActionDisposable;
        private bool m_myTurn;

        private bool m_isFastFight;

        public GameHeroFight Hero => m_hero;
        public bool IsDeath => m_isDeath;
        public Vector3 GetPosition => Self.position;
        public bool CanRetaliation => m_hero.Model.Characteristics.Main.CanRetaliation && CurrentCountCounterAttack > 0;
        public HexagonCell Cell => MyPlace;
        public bool Mellee => m_attack is MelleeAttack;
        public TypeStrike TypeStrike => m_hero.Model.Characteristics.Main.AttackType;
        public int Stamina => _statusState.Stamina;
        public List<HeroController> ListTarget { get; set; } = new();
        public bool CanWait => m_canWait;
        public bool IsFacingRight => m_isFacingRight;
        public float SpeedMove => _speedMove;
        public Side Side => m_side;
        public HeroStatus StatusState => _statusState;
        public bool IsFastFight => m_isFastFight;

        private void Awake()
        {
            ListTarget = new();
            Self = base.transform;
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _onStartFight.Execute(this);
            IsSide(m_side);
        }

        public void SetData(GameHero gameHero, HexagonCell place, Side side, bool isFastFight)
        {
            m_isFastFight = isFastFight;
            _bodyParent.localRotation = Quaternion.Euler(0, 90, 0);
            FightController.OnEndRound.Subscribe(_ => RefreshOnEndRound()).AddTo(m_disposables);
            MyPlace = place;
            place.SetHero(this);
            m_side = side;
            m_hero = new GameHeroFight(gameHero, _statusState);
            m_hero.PrepareSkills(this);

            m_attack.Init(this);
            _movement.Init(GridController, this, MyPlace, _currentHeroVisualModel.Animator);

            _movement.OnChangePlace
                .Subscribe(newPlace => MyPlace = newPlace)
                .AddTo(m_disposables);
        }

        public void Refresh()
        {
            m_canWait = true;
        }

        public void DoTurn()
        {
            if(m_myTurn)
                return;
            
            m_myTurn = true;
            AddFightRecordActionMe();
            if (!m_isDeath && _statusState.PermissionAction())
                PrepareOnStartTurn();
            else
                EndTurn();
        }

        protected void EndTurn()
        {
            if(!m_myTurn)
                return;
            
            m_myTurn = false;
            if (m_isFacingRight ^ (m_side == Side.Left))
                FlipX();

            ClearAction();
            OnEndAction();
            Animator.Play(ANIMATION_IDLE);
            FightController.RemoveHeroWithActionAll(this);
        }


        protected virtual void FindAvailableCells()
        {
            if (!m_isFastFight)
            {
                MyPlace.StartCheckMove(
                    m_hero.Model.Characteristics.Main.Speed,
                    this,
                    !BotProvider.CheckMeOnSubmission(m_side)
                );
                if (!BotProvider.CheckMeOnSubmission(m_side))
                    ShowHeroesPlaceInteractive();
            }
            else
            {
                MyPlace.StartCheckMove(
                    m_hero.Model.Characteristics.Main.Speed,
                    this,
                    false
                );
            }
        }

        protected virtual void WaitingSelectTarget()
        {
            HexagonCell.RegisterOnClick(SelectHexagonCell);
        }

        protected virtual void SelectHexagonCell(HexagonCell cell)
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
                    if (CanAttackHero(cell.Hero))
                    {
                        m_selectHero = cell.Hero;
                        if (m_attack is MelleeAttack)
                        {
                            if (CanMelleeAttack())
                                if (cell.GetCanAttackCell)
                                {
                                    cell.RegisterOnSelectDirection(SelectDirectionAttack);
                                    HexagonCell.UnregisterOnClick(SelectHexagonCell);
                                }
                        }
                        else
                        {
                            StartDistanceAttackOtherHero(m_selectHero);
                        }
                    }
            }
        }

        private void SelectDirectionAttack(HexagonCell targetCell)
        {
            HexagonCell.UnregisterOnClick(SelectHexagonCell);
            StartMelleeAttackOtherHero(targetCell, m_selectHero);
        }

        public void SelectDirectionAttack(HexagonCell targetCell, HeroController otherHero)
        {
            HexagonCell.UnregisterOnClick(SelectHexagonCell);
            StartMelleeAttackOtherHero(targetCell, otherHero);
        }

        //Attack
        private void StartMelleeAttackOtherHero(HexagonCell targetCell, HeroController enemy)
        {
            OnEndSelectCell();
            m_actionTokenSource.TryCancel();
            m_actionTokenSource = new();
            IMelleeAttackOtherHero(targetCell, enemy, m_actionTokenSource.Token).Forget();
        }

        public void StartDistanceAttackOtherHero(HeroController enemy)
        {
            OnEndSelectCell();
            m_actionTokenSource.TryCancel();
            m_actionTokenSource = new();
            IDistanceAttackOtherHero(enemy, 20, m_actionTokenSource.Token).Forget();
        }

        private async UniTask IMelleeAttackOtherHero(HexagonCell targetCell, HeroController heroForAttack,
            CancellationToken token)
        {
            if (targetCell != MyPlace) await _movement.Move(targetCell);

            if (heroForAttack != null)
            {
                if (!m_isFastFight)
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

                await m_attack.Attacking(heroForAttack, 30);

                if (!heroForAttack.IsDeath) await heroForAttack.GetRetaliation(this);
            }

            if (!m_isFastFight)
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

            SetFaceDefault();
            EndTurn();
        }

        private async UniTask IDistanceAttackOtherHero(HeroController otherHero, int bonusStamina = 20,
            CancellationToken token = default)
        {
            hitCount = 0;
            if (!IsFastFight)
                await CheckFlipX(otherHero);

            m_currentActionDisposable = _currentHeroVisualModel.AttackController
                .OnRangeDamage.Subscribe(AddHitCount);

            m_attack.Attacking(otherHero, bonusStamina);

            if (!IsFastFight)
                await PlayAnimation(Constants.Visual.ANIMATOR_DISTANCE_ATTACK_NAME_HASH);

            if (!IsFastFight)
                await UniTask.WaitUntil(() => hitCount == 1, cancellationToken: token);

            //RemoveFightRecordActionMe();

            SetFaceDefault();

            _onStrike.Execute(new List<HeroController> { otherHero });
            EndTurn();
        }

        private void SetFaceDefault()
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

        protected virtual async UniTask GetRetaliation(HeroController attackedHero)
        {
            if (IsDeath)
                return;

            if (!CanRetaliation)
                return;

            await IGetRetaliation(attackedHero);
        }

        protected virtual async UniTask IGetRetaliation(HeroController attackedHero)
        {
            if (CanCounterAttack(this, attackedHero))
            {
                CurrentCountCounterAttack -= 1;
                await m_attack.Attacking(attackedHero, 15);
                SetFaceDefault();
            }
        }

        private void AddHitCount(HeroController target)
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
        private void ClearAction()
        {
            OnEndSelectCell();
            OutlineController.SwitchOff();
        }

        public virtual void ApplyDamage(Strike strike)
        {
            if (_statusState.PermissionGetStrike(strike))
            {
                _onTakeDamage.Execute(this);
                m_hero.GetDamage(strike);
                if (m_hero.Health > 0f)
                {
                    PlayAnimation(Constants.Visual.ANIMATOR_GET_DAMAGE_NAME_HASH).Forget();
                }
                else
                {
                    m_isDeath = true;
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
            m_hero.GetHeal(heal, typeNumber);
            _statusState.ChangeHP(m_hero.Health);
            FightEffectController.Instance.CreateHealObject(Self);
            _onHeal.Execute((this, heal));
        }

        public virtual void ChangeMaxHP(int amountChange, RoundTypeNumber typeNumber = RoundTypeNumber.Num)
        {
            m_hero.ChangeMaxHP(amountChange, typeNumber);
            _statusState.ChangeMaxHP(amountChange);
        }

        public void DeleteHero()
        {
            MyPlace?.ClearSublject();
            _sequenceAnimation.Kill();
            m_actionTokenSource.TryCancel();
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
            m_damageFromStrike += damage;
        }

        protected virtual void CreateDamage(HeroController enemy)
        {
            m_currentActionDisposable.Dispose();
            enemy.ApplyDamage(new Strike(m_hero.Model.Characteristics.Damage, m_hero.Model.Characteristics.Main.Attack,
                typeStrike: TypeStrike));
        }

        //Event
        public void GetListForSpell(List<HeroController> listTarget)
        {
            _statusState.ChangeStamina(-100);
            _onListSpell.Execute(listTarget);
        }

        private void AddFightRecordActionMe()
        {
            FightController.AddHeroWithAction(this);
        }

        private void RemoveFightRecordActionMe()
        {
            FightController.RemoveHeroWithAction(this);
        }
    }
}