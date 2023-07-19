using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fight.AI;
using Fight.Common.Strikes;
using Fight.Grid;
using Fight.HeroStates;
using Fight.Misc;
using Fight.Rounds;
using Hero;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using UniRx;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        [Inject] public FightController FightController;
        [Inject] protected BotProvider _botProvider;
        [Inject] protected GridController _gridController;

        [Header("Components")]
        public HeroStatus statusState;
        protected Transform Self;
        [SerializeField] protected HexagonCell myPlace;
        [HideInInspector] public List<HeroController> listTarget = new List<HeroController>();
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;

        [Header("Characteristics")]
        public float speedMove = 2f;
        public float speedAnimation = 1f;
        public GameHeroFight hero;
        protected bool onGround = true;
        protected bool needFlip = false;
        public Side Side = Side.Left;
        protected Vector3 delta = new Vector2(-0.6f, 0f);
        protected int hitCount = 0;
        public int maxCountCounterAttack = 1;
        protected int currentCountCounterAttack = 1;
        private HeroController selectHero;
        private float damageFromStrike;
        private bool isDeath = false;
        private Coroutine coroutineAttackEnemy;
        private Stack<HexagonCell> way = new Stack<HexagonCell>();
        private CompositeDisposable _disposables = new CompositeDisposable();

        public bool IsDeath { get => isDeath; }
        public Vector3 GetPosition { get => Self.position; }
        public bool CanRetaliation { get => hero.Model.Characteristics.Main.CanRetaliation && currentCountCounterAttack > 0;  }
        public HexagonCell Cell { get => myPlace; }
        public bool Mellee { get => hero.Model.Characteristics.Main.Mellee; }
        public TypeStrike typeStrike { get => hero.Model.Characteristics.Main.AttackType; }
        public int Stamina { get => statusState.Stamina; }

        void Awake()
        {
            Self = base.transform;
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        public void SetData(GameHero gameHero, HexagonCell place, Side side)
        {
            Debug.Log($"{gameObject.name}, {gameHero.Model.General.Name}");
            FightController.OnEndRound.Subscribe(_ => RefreshOnEndRound()).AddTo(_disposables);
            myPlace = place;
            place.SetHero(this);
            Side = side;
            hero = new GameHeroFight(gameHero, statusState);
        }

        void Start()
        {
            hero.PrepareSkills(this);
            OnStartFight();
            IsSide(Side);
        }

        public void DoTurn()
        {
            //Debug.Log($"Start turn {gameObject.name}", gameObject);
            AddFightRecordActionMe();
            if (!isDeath && statusState.PermissionAction())
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
            if (isFacingRight ^ (Side == Side.Left)) FlipX();
            ClearAction();
            OnEndAction();
            PlayAnimation(ANIMATION_IDLE, DefaultAnimIdle, withRecord: false);
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
                            else
                            {
                                Debug.Log("i can't attak him");
                            }
                        }
                        else
                        {
                            Debug.Log("i can shoot in him");
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
                yield return heroForAttack.GetRetaliation(this);
            }
            yield return new WaitForSeconds(0.5f);
            SetFaceDefault();
            EndTurn();
        }

        protected virtual IEnumerator IMoveToCellTarget(HexagonCell targetCell)
        {
            way = _gridController.FindWay(myPlace, targetCell, typeMovement: hero.GetBaseCharacteristic.MovementType);
            Vector3 targetPos, startPos;
            float t = 0f;
            HexagonCell currentCell = way.Pop();
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
                    Self.position = Vector2.Lerp(startPos, targetPos, t);
                    yield return null;
                }

                Self.position = targetPos;
                myPlace = currentCell;
                myPlace.SetHero(this);
            }
            SetMyPlaceColor();
        }

        protected virtual IEnumerator IAttack(HeroController otherHero, int bonusStamina = 30)
        {
            if (statusState.PermissionMakeStrike(TypeStrike.Physical))
            {
                yield return StartCoroutine(CheckFlipX(otherHero));
                statusState.ChangeStamina(bonusStamina);
                PlayAnimation(ANIMATION_ATTACK, () => DefaultAnimAttack(otherHero));

                while (flagAnimFinish == false)
                    yield return null;
            }
        }

        protected virtual IEnumerator IDistanceAttackOtherHero(HeroController otherHero, int bonusStamina = 20)
        {
            hitCount = 0;
            yield return StartCoroutine(CheckFlipX(otherHero));
            statusState.ChangeStamina(bonusStamina);
            PlayAnimation(ANIMATION_DISTANCE_ATTACK, () => DefaultAnimDistanceAttack(new List<HeroController>() { otherHero }));

            while (hitCount != 1)
                yield return null;

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
            while (flagAnimFinish == false)
                yield return null;

            Debug.Log($"I retalation attack {gameObject.name}");
            if (CanCounterAttack(this, attackedHero))
            {
                AddFightRecordActionMe();
                currentCountCounterAttack -= 1;
                yield return StartCoroutine(IAttack(attackedHero, 15));
                SetFaceDefault();
                RemoveFightRecordActionMe();
            }

        }

        protected void HitCount()
        {
            hitCount += 1;
        }

        protected virtual void DoSpell()
        {
            statusState.ChangeStamina(-100);
            OnSpell(listTarget);
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
            outlineController.SwitchOff();
        }

        public virtual void ApplyDamage(Strike strike)
        {
            if (statusState.PermissionGetStrike(strike))
            {
                OnTakingDamage();
                hero.GetDamage(strike);
                if (hero.Health > 0f)
                {
                    PlayAnimation(ANIMATION_GET_DAMAGE, () => DefaultAnimGetDamage(strike));
                }
                else
                {
                    PlayAnimation(ANIMATION_DEATH, DefaultAnimDeath);
                }
            }
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

        public virtual void GiveDamage(HeroController enemy)
        {
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