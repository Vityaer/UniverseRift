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
using UniRx;
using UnityEngine;
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

        private GameHeroFight _hero;
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
        private Coroutine coroutineAttackEnemy;
        private CompositeDisposable _disposables = new();
        private IDisposable _currentActionDisposable;

        public GameHeroFight Hero => _hero;
        public bool IsDeath => _isDeath;
        public Vector3 GetPosition => Self.position;
        public bool CanRetaliation => _hero.Model.Characteristics.Main.CanRetaliation && CurrentCountCounterAttack > 0;
        public HexagonCell Cell => MyPlace;
        public bool Mellee => _hero.Model.Characteristics.Main.Mellee;
        public TypeStrike TypeStrike => _hero.Model.Characteristics.Main.AttackType;
        public int Stamina => _statusState.Stamina;
        public List<HeroController> ListTarget { get; set; }
        public bool CanWait => _canWait;
        public bool IsFacingRight => _isFacingRight;
        public float SpeedMove => _speedMove;
        public Side Side => _side;
        public HeroStatus StatusState => _statusState;

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

        public void SetData(GameHero gameHero, HexagonCell place, Side side)
        {
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
            //Debug.Log($"Start turn {gameObject.name}", gameObject);
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
            //Debug.Log($"End turn{gameObject.name}", gameObject);
            if (_isFacingRight ^ (_side == Side.Left))
                FlipX();

            ClearAction();
            OnEndAction();
            Animator.Play(ANIMATION_IDLE);
            FightController.RemoveHeroWithActionAll(this);
        }


        protected virtual void FindAvailableCells()
        {
            MyPlace.StartCheckMove(
                _hero.Model.Characteristics.Main.Speed,
                this,
                playerCanController: !_botProvider.CheckMeOnSubmission(_side)
            );
            if (!_botProvider.CheckMeOnSubmission(_side))
                ShowHeroesPlaceInteractive();
        }

        protected virtual void WaitingSelectTarget()
        {
            HexagonCell.RegisterOnClick(SelectHexagonCell);
        }

        public virtual void SelectHexagonCell(HexagonCell cell)
        {
            Debug.Log($"SelectHexagonCell: {cell.name}");
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
            coroutineAttackEnemy = null;
            coroutineAttackEnemy = StartCoroutine(IMelleeAttackOtherHero(targetCell, enemy));
        }

        public void StartDistanceAttackOtherHero(HeroController enemy)
        {
            Debug.Log($"StartDistanceAttackOtherHero: {enemy.name}");
            OnEndSelectCell();
            coroutineAttackEnemy = StartCoroutine(IDistanceAttackOtherHero(enemy, 20));
        }

        IEnumerator IMelleeAttackOtherHero(HexagonCell targetCell, HeroController heroForAttack)
        {
            if (targetCell != MyPlace)
                yield return StartCoroutine(_movement.Move(targetCell));

            if (heroForAttack != null)
            {
                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(_attack.Attacking(heroForAttack, 30));

                if(!heroForAttack.IsDeath)
                    yield return heroForAttack.GetRetaliation(this);
            }
            yield return new WaitForSeconds(0.5f);
            SetFaceDefault();
            EndTurn();
        }

        protected virtual IEnumerator IDistanceAttackOtherHero(HeroController otherHero, int bonusStamina = 20)
        {
            hitCount = 0;
            yield return StartCoroutine(CheckFlipX(otherHero));
            _statusState.ChangeStamina(bonusStamina);

            _currentActionDisposable = _currentHeroVisualModel.AttackController
                .OnRangeDamage.Subscribe(AddHitCount);
            
            _currentHeroVisualModel.AttackController.Attack(otherHero);

            StartCoroutine(PlayAnimation(Constants.Visual.ANIMATOR_DISTANCE_ATTACK_NAME_HASH));
            while (hitCount != 1)
                yield return null;

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

        public IEnumerator CheckFlipX(HeroController otherHero)
        {
            needFlip = NeedFlip(otherHero);
            if (needFlip)
                FlipX();
            yield return new WaitForSeconds(0.1f);
        }

        public virtual Coroutine GetRetaliation(HeroController attackedHero)
        {
            if (IsDeath)
                return null;

            if (CanRetaliation)
            {
                return StartCoroutine(IGetRetaliation(attackedHero));
            }
            else
            {
                return null;
            }
        }

        protected virtual IEnumerator IGetRetaliation(HeroController attackedHero)
        {
            if (CanCounterAttack(this, attackedHero))
            {
                CurrentCountCounterAttack -= 1;
                yield return StartCoroutine(_attack.Attacking(attackedHero, 15));
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
        //Brain hero 

        protected virtual void ChooseEnemies(List<HeroController> listTarget, int countTarget)
        {
            listTarget.Clear();
            if (countTarget == 0)
            {
                countTarget = 1;
                _hero.Model.Characteristics.CountTargetForSimpleAttack = 1;
            }
            FightController.ChooseEnemies(_side, countTarget, listTarget);

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
                    StartCoroutine(PlayAnimation(Constants.Visual.ANIMATOR_GET_DAMAGE_NAME_HASH));
                }
                else
                {
                    _isDeath = true;
                    MyPlace.ClearSublject();
                    StartCoroutine
                    (
                        PlayAnimation
                        (
                            Constants.Visual.ANIMATOR_DEATH_NAME_HASH,
                            onAnimationFinish: OffAnimator
                        )
                    );
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
            _sequenceAnimation.Kill();
            if (coroutineAttackEnemy != null) StopCoroutine(coroutineAttackEnemy);
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