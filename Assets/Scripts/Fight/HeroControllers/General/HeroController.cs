using DG.Tweening;
using Fight.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HeroController : MonoBehaviour
{
    [Header("Components")]
    public HeroStatus statusState;
    protected Transform tr;
    [SerializeField] protected HexagonCell myPlace;
    [HideInInspector] public List<HeroController> listTarget = new List<HeroController>();
    protected Rigidbody2D rb;
    protected Animator anim;

    [Header("Characteristics")]
    public float speedMove = 2f;
    public float speedAnimation = 1f;
    public Hero hero;
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

    public bool IsDeath { get => isDeath; }
    public Vector3 GetPosition { get => tr.position; }
    public bool CanRetaliation { get => hero.characts.baseCharacteristic.CanRetaliation; }
    public HexagonCell Cell { get => myPlace; }
    public bool Mellee { get => hero.characts.baseCharacteristic.Mellee; }
    public TypeStrike typeStrike { get => hero.characts.baseCharacteristic.typeStrike; }
    public int Stamina { get => statusState.Stamina; }

    void Awake()
    {
        statusState = GetComponent<HeroStatus>();
        hero.statusState = statusState;
        tr = base.transform;
        BodyParent = transform.Find("BodyParent").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        hero.PrepareSkills(this);
        OnStartFight();
        FightController.Instance.RegisterOnEndRound(RefreshOnEndRound);
    }

    public void DoTurn()
    {
        Debug.Log($"Start turn{gameObject.name}", gameObject);
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
        Debug.Log($"End turn{gameObject.name}", gameObject);
        if (isFacingRight ^ (Side == Side.Left)) FlipX();
        ClearAction();
        OnEndAction();
        PlayAnimation(ANIMATION_IDLE, DefaultAnimIdle, withRecord: false);
        FightController.Instance.RemoveHeroWithActionAll(this);
    }


    protected virtual void FindAvailableCells()
    {
        myPlace.StartCheckMove(
            hero.characts.baseCharacteristic.Speed,
            this,
            playerCanController: !AIController.Instance.CheckMeOnSubmission(Side)
        );
        if (!AIController.Instance.CheckMeOnSubmission(Side))
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
        way = GridController.Instance.FindWay(myPlace, targetCell, typeMovement: hero.GetBaseCharacteristic.typeMovement);
        Vector3 targetPos, startPos;
        float t = 0f;
        HexagonCell currentCell = way.Pop();
        while (way.Count > 0)
        {
            myPlace.ClearSublject();
            currentCell = way.Pop();
            startPos = tr.position;
            targetPos = currentCell.Position;

            if (isFacingRight ^ ((targetPos.x - startPos.x) > 0))
                FlipX();

            t = 0f;

            while (t <= 1f)
            {
                t += Time.deltaTime * speedMove;
                tr.position = Vector2.Lerp(startPos, targetPos, t);
                yield return null;
            }

            tr.position = targetPos;
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
            hero.characts.CountTargetForSimpleAttack = 1;
        }
        FightController.Instance.ChooseEnemies(Side, countTarget, listTarget);

    }

    //End action 	
    protected void ClearAction()
    {
        OnEndSelectCell();
        outlineController.SwitchOff();
    }

    //API
    public void SetHero(InfoHero infoHero, HexagonCell place, Side side)
    {
        myPlace = place;
        place.SetHero(this);
        IsSide(side);
        this.hero.SetHero(infoHero);
        statusState.SetMaxHealth(this.hero.characts.HP);
    }

    public virtual void GetDamage(Strike strike)
    {
        if (statusState.PermissionGetStrike(strike))
        {
            OnTakingDamage();
            hero.GetDamage(strike);
            statusState.ChangeHP(hero.characts.HP);
            statusState.ChangeStamina(10);
            if (hero.characts.HP > 0.1f)
            {
                PlayAnimation(ANIMATION_GET_DAMAGE, () => DefaultAnimGetDamage(strike));
            }
            else
            {
                PlayAnimation(ANIMATION_DEATH, DefaultAnimDeath);
            }
        }
    }

    public virtual void GetHeal(float heal, TypeNumber typeNumber = TypeNumber.Num)
    {
        hero.GetHeal(heal, typeNumber);
        statusState.ChangeHP(hero.characts.HP);
        FightEffectController.Instance.CreateHealObject(tr);
        OnHeal();
    }

    public virtual void ChangeMaxHP(int amountChange, TypeNumber typeNumber = TypeNumber.Num)
    {
        hero.ChangeMaxHP(amountChange, typeNumber);
        statusState.ChangeMaxHP(amountChange);
    }

    public void DeleteHero()
    {
        myPlace.ClearSublject();
        if (coroutineAttackEnemy != null) StopCoroutine(coroutineAttackEnemy);
        if (sequenceAnimation != null) sequenceAnimation.Kill();
        FightController.Instance.UnregisterOnEndRound(RefreshOnEndRound);
        DeleteAllDelegate();
        Destroy(gameObject);
    }

    public void ClickOnMe()
    {
        myPlace?.ClickOnMe();
    }

    private void Death()
    {
        statusState.Death();
        ClearAction();
        FightController.Instance.DeleteHero(this);
        DeleteHero();
    }

    public void MessageDamageAfterStrike(float damage)
    {
        damageFromStrike += damage;
    }

    public virtual void GiveDamage(HeroController enemy)
    {
        enemy.GetDamage(new Strike(hero.characts.Damage, hero.characts.GeneralAttack, typeStrike: typeStrike));
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
        FightController.Instance.AddHeroWithAction(this);
    }

    protected void RemoveFightRecordActionMe()
    {
        FightController.Instance.RemoveHeroWithAction(this);
    }
}