using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fight.AI;
using Fight.Common.Strikes;
using Fight.Grid;
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
        public HeroStatus statusState;

        [Header("Characteristics")]
        public float speedMove = 2f;
        public float speedAnimation = 1f;
        public GameHeroFight hero;
        public Side Side = Side.Left;
        public int maxCountCounterAttack = 1;

        protected Transform Self;
        protected bool onGround = true;
        protected Rigidbody Rigidbody;
        protected bool needFlip = false;
        protected Vector3 delta = new Vector2(-0.6f, 0f);
        protected int hitCount = 0;
        protected int CurrentCountCounterAttack = 1;
        protected HexagonCell myPlace;

        private bool _canWait;
        private HeroController selectHero;
        private float damageFromStrike;
        private bool _isDeath = false;
        private Coroutine coroutineAttackEnemy;
        private CompositeDisposable _disposables = new();
        private IDisposable _currentActionDisposable;

        public bool IsDeath => _isDeath;
        public Vector3 GetPosition => Self.position;
        public bool CanRetaliation => hero.Model.Characteristics.Main.CanRetaliation && CurrentCountCounterAttack > 0;
        public HexagonCell Cell => myPlace;
        public bool Mellee => hero.Model.Characteristics.Main.Mellee;
        public TypeStrike typeStrike => hero.Model.Characteristics.Main.AttackType;
        public int Stamina => statusState.Stamina;
        public Animator HeroAnimator => _currentHeroVisualModel.Animator;
        public List<HeroController> ListTarget { get; set; }
        public bool CanWait => _canWait;

        void Awake()
        {
            ListTarget = new();
            Self = base.transform;
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            hero.PrepareSkills(this);
            OnStartFight();
            IsSide(Side);
        }

        public void SetData(GameHero gameHero, HexagonCell place, Side side)
        {
            BodyParent.localRotation = Quaternion.Euler(0, 90, 0);
            FightController.OnEndRound.Subscribe(_ => RefreshOnEndRound()).AddTo(_disposables);
            myPlace = place;
            place.SetHero(this);
            Side = side;
            hero = new GameHeroFight(gameHero, statusState);
        }

        public void Refresh()
        {
            _canWait = true;
        }

        public void DoTurn()
        {
            //Debug.Log($"Start turn {gameObject.name}", gameObject);
            AddFightRecordActionMe();
            if (!_isDeath && statusState.PermissionAction())
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
            if (isFacingRight ^ (Side == Side.Left))
                FlipX();

            ClearAction();
            OnEndAction();
            Animator.Play(ANIMATION_IDLE);
            FightController.RemoveHeroWithActionAll(this);
        }


        protected virtual void FindAvailableCells()
        {
            myPlace.StartCheckMove(
                hero.Model.Characteristics.Main.Speed,
                this,
                playerCanController: !_botProvider.CheckMeOnSubmission(Side)
            );
            if (!_botProvider.CheckMeOnSubmission(Side))
                ShowHeroesPlaceInteractive();
        }

        protected virtual void WaitingSelectTarget()
        {
            HexagonCell.RegisterOnClick(SelectHexagonCell);
        }

        public virtual void SelectHexagonCell(HexagonCell cell)
        {
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
                        selectHero = cell.Hero;
                        if (CanShoot())
                        {
                            if (cell.GetCanAttackCell)
                            {
                                cell.RegisterOnSelectDirection(SelectDirectionAttack);
                                HexagonCell.UnregisterOnClick(SelectHexagonCell);
                            }
                        }
                        else
                        {
                            StartDistanceAttackOtherHero(selectHero);
                        }
                    }
                }
            }
        }

        public void SelectDirectionAttack(HexagonCell targetCell)
        {
            HexagonCell.UnregisterOnClick(SelectHexagonCell);
            StartMelleeAttackOtherHero(targetCell, selectHero);
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
            OnEndSelectCell();
            coroutineAttackEnemy = StartCoroutine(IDistanceAttackOtherHero(enemy, 20));
        }

        IEnumerator IMelleeAttackOtherHero(HexagonCell targetCell, HeroController heroForAttack)
        {
            if (targetCell != myPlace)
                yield return StartCoroutine(IMoveToCellTarget(targetCell));

            if (heroForAttack != null)
            {
                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(IAttack(heroForAttack));

                if(!heroForAttack.IsDeath)
                    yield return heroForAttack.GetRetaliation(this);
            }
            yield return new WaitForSeconds(0.5f);
            SetFaceDefault();
            EndTurn();
        }

        protected virtual IEnumerator IMoveToCellTarget(HexagonCell targetCell)
        {
            Animator.SetBool("Speed", true);
            var way = _gridController.FindWay(myPlace, targetCell, typeMovement: hero.GetBaseCharacteristic.MovementType);
            Vector3 targetPos, startPos;
            float t = 0f;

            var currentCell = way.Pop();
            while (way.Count > 0)
            {
                myPlace.ClearSublject();
                currentCell = way.Pop();

                startPos = Self.position;
                targetPos = currentCell.Position;

                if (isFacingRight ^ ((targetPos.x - startPos.x) > 0))
                    FlipX();

                t = 0f;

                while (t <= 1f)
                {
                    t += Time.deltaTime * speedMove;
                    Self.position = Vector3.Lerp(startPos, targetPos, t);
                    yield return null;
                }

                Self.position = targetPos;
                myPlace = currentCell;
                myPlace.SetHero(this);
            }

            Animator.SetBool("Speed", false);
            SetMyPlaceColor();
        }

        protected virtual IEnumerator IAttack(HeroController otherHero, int bonusStamina = 30)
        {
            if (statusState.PermissionMakeStrike(TypeStrike.Physical))
            {
                yield return StartCoroutine(CheckFlipX(otherHero));
                statusState.ChangeStamina(bonusStamina);
                _currentActionDisposable = _currentHeroVisualModel.AttackController
                    .OnMakeDamage.Subscribe(_ => CreateDamage(otherHero));

                _currentHeroVisualModel.AttackController.Attack(otherHero);
                yield return StartCoroutine(PlayAnimation(ANIMATION_ATTACK));
            }
        }

        protected virtual IEnumerator IDistanceAttackOtherHero(HeroController otherHero, int bonusStamina = 20)
        {
            hitCount = 0;
            yield return StartCoroutine(CheckFlipX(otherHero));
            statusState.ChangeStamina(bonusStamina);

            _currentActionDisposable = _currentHeroVisualModel.AttackController
                .OnRangeDamage.Subscribe(AddHitCount);
            
            _currentHeroVisualModel.AttackController.Attack(otherHero);

            StartCoroutine(PlayAnimation(ANIMATION_DISTANCE_ATTACK));
            while (hitCount != 1)
                yield return null;

            //RemoveFightRecordActionMe();

            SetFaceDefault();

            OnStrike();
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

        protected IEnumerator CheckFlipX(HeroController otherHero)
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
                yield return StartCoroutine(IAttack(attackedHero, 15));
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
            statusState.ChangeStamina(-100);
            OnSpell(ListTarget);
            EndTurn();
        }
        //Brain hero 

        protected virtual void ChooseEnemies(List<HeroController> listTarget, int countTarget)
        {
            listTarget.Clear();
            if (countTarget == 0)
            {
                countTarget = 1;
                hero.Model.Characteristics.CountTargetForSimpleAttack = 1;
            }
            FightController.ChooseEnemies(Side, countTarget, listTarget);

        }

        //End action 	
        protected void ClearAction()
        {
            OnEndSelectCell();
            OutlineController.SwitchOff();
        }

        public virtual void ApplyDamage(Strike strike)
        {
            if (statusState.PermissionGetStrike(strike))
            {
                OnTakingDamage();
                hero.GetDamage(strike);
                if (hero.Health > 0f)
                {
                    StartCoroutine(PlayAnimation(ANIMATION_GET_DAMAGE));
                }
                else
                {
                    _isDeath = true;
                    StartCoroutine(PlayAnimation(ANIMATION_DEATH, onAnimationFinish: OffAnimator));
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
            hero.GetHeal(heal, typeNumber);
            statusState.ChangeHP(hero.Health);
            FightEffectController.Instance.CreateHealObject(Self);
            OnHeal();
        }

        public virtual void ChangeMaxHP(int amountChange, RoundTypeNumber typeNumber = RoundTypeNumber.Num)
        {
            hero.ChangeMaxHP(amountChange, typeNumber);
            statusState.ChangeMaxHP(amountChange);
        }

        public void DeleteHero()
        {
            _sequenceAnimation.Kill();
            myPlace.ClearSublject();
            if (coroutineAttackEnemy != null) StopCoroutine(coroutineAttackEnemy);
            DeleteAllDelegate();
            Destroy(gameObject);
        }

        public void ClickOnMe()
        {
            Debug.Log("click on hero");
            myPlace?.ClickOnMe();
        }

        private void Death()
        {
            statusState.Death();
            ClearAction();
            FightController.DeleteHero(this);
            DeleteHero();
        }

        public void MessageDamageAfterStrike(float damage)
        {
            damageFromStrike += damage;
        }

        public virtual void CreateDamage(HeroController enemy)
        {
            _currentActionDisposable.Dispose();
            enemy.ApplyDamage(new Strike(hero.Model.Characteristics.Damage, hero.Model.Characteristics.Main.Attack, typeStrike: typeStrike));
        }
        //Event
        public void GetListForSpell(List<HeroController> listTarget)
        {
            statusState.ChangeStamina(-100);
            if (delsOnListSpell != null)
                delsOnListSpell(listTarget);
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