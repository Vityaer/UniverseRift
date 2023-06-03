using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif
using Fight.Grid;

public class HexagonCell : MonoBehaviour
{
    public bool available = true;
    public bool availableMove = true;
    public bool achievableMove = false;
    public int step = 0;
    public SpriteRenderer spriteCell, spriteAvailable;

    [SerializeField] private List<NeighbourCell> neighbours = new List<NeighbourCell>();
    [SerializeField] HeroController heroScript;

    private GameObject subject;
    private Transform tr;
    private bool showAchievable = false;
    private static HeroController requestHero = null;
    private static Action<HexagonCell> observerClick, observerSelectDirection, observerAchivableMove;

    public Vector3 Position { get => transform.position; }
    public List<NeighbourCell> GetAvailableNeighbours { get => neighbours.FindAll(x => x.available == true); }
    public bool CanStand { get => (availableMove && (heroScript == null)); }
    public HeroController Hero { get => heroScript; }
    public bool GetCanAttackCell { get => (neighbours.Find(x => (x.achievableMove == true)) != null); }
    private Vector2 deltaSize => Constants.Fight.CellDeltaStep;

    void Awake()
    {
        tr = base.transform;
    }

    void Start()
    {
        FightController.Instance.RegisterOnFinishFight(OnEndMatch);
        spriteAvailable.color = Constants.Colors.ACHIEVABLE_CELL_COLOR;

    }

    public void StartCheckMove(int step, HeroController newRequestHero, bool playerCanController)
    {
        HexagonCell.requestHero = newRequestHero;
        GridController.PlayerCanController = playerCanController;
        this.step = step;
        achievableMove = true;
        requestHero.RegisterOnEndSelectCell(ClearCanMove);
        for (int i = 0; i < neighbours.Count; i++) neighbours[i].CheckMove(step - 1);
    }

    public void CheckMove(int step)
    {
        if (available && availableMove)
        {
            if ((achievableMove == false) || (this.step < step))
            {
                this.step = step;
                if (achievableMove == false)
                {
                    requestHero.RegisterOnEndSelectCell(ClearCanMove);
                    OnAchivableMove();
                }
                achievableMove = true;

                if ((showAchievable == false)/* && GridController.PlayerCanController*/)
                {
                    showAchievable = true;
                    spriteAvailable.enabled = true;
                    spriteAvailable.color = Constants.Colors.ACHIEVABLE_CELL_COLOR;
                }

                if (step > 0)
                    for (int i = 0; i < neighbours.Count; i++)
                        neighbours[i].CheckMove(step - 1);
            }
        }
    }

    public HexagonCell GetAchivableNeighbourCell()
    {
        var achievableNeighbours = neighbours.FindAll(x => x.achievableMove);

        if (achievableNeighbours.Count == 0)
            return null;

        return achievableNeighbours[UnityEngine.Random.Range(0, achievableNeighbours.Count)]?.Cell;
    }

    public void RegisterOnSelectDirection(Action<HexagonCell> selectDirectionForHero, bool showUIDirection = true)
    {
        observerSelectDirection = selectDirectionForHero;
        if (showUIDirection)
            ShowDirectionsAttack();
    }

    private void ShowDirectionsAttack()
    {
        FightUI.Instance.melleeAttackController.RegisterOnSelectDirection(SelectDirection, this, GetAvailableNeighbours);
    }

    private void SelectDirection(NeighbourDirection direction)
    {
        if (observerSelectDirection != null)
        {
            observerSelectDirection(GetNeighbourCellOnDirection(direction));
            observerSelectDirection = null;
        }
    }
    private HexagonCell GetNeighbourCellOnDirection(NeighbourDirection direction)
    {
        HexagonCell result = neighbours.Find(x => x.direction == direction).Cell;
        return result;
    }

    private void OnSelectDirection()
    {
        if ((observerClick != null) && (available == true))
            observerClick(this);
    }

    public void ClearCanMove()
    {
        showAchievable = false;
        this.step = 0;
        achievableMove = false;
        spriteAvailable.enabled = false;
        dist = 100;
        requestHero?.UnregisterOnEndSelectCell(ClearCanMove);
    }

    public bool MyEnemyNear(Side masterSide)
    {
        var neigboursCellWithHeroes = neighbours.FindAll(neighbour => (neighbour.GetHero != null));
        var enemy = neigboursCellWithHeroes.Find(neighbourCell => neighbourCell.GetHero.Side != masterSide);
        return (enemy != null);
    }

    public void SetSubject(GameObject newSubject)
    {
        this.subject = newSubject;
    }

    public void SetHero(HeroController hero)
    {
        heroScript = hero;
        subject = hero.gameObject;
        availableMove = false;
    }

    public void SetColor(Color color)
    {
        spriteAvailable.enabled = true;
        spriteAvailable.color = color;
    }

    public void ClearSublject()
    {
        this.subject = null;
        heroScript = null;
        availableMove = true;
        spriteAvailable.enabled = false;
    }

    public void ClickOnMe()
    {
        if (GridController.PlayerCanController)
        {
            if (achievableMove || (heroScript != null))
            {
                OnClickCell();
            }
        }
    }

    public void AITurn()
    {
        OnClickCell();
    }

    public static void RegisterOnClick(Action<HexagonCell> d) { observerClick += d; }
    public static void UnregisterOnClick(Action<HexagonCell> d) { observerClick -= d; }
    private void OnClickCell() { if ((observerClick != null) && (available == true)) observerClick(this); }


    public static void RegisterOnAchivableMove(Action<HexagonCell> d) { observerAchivableMove += d; }
    public static void UnregisterOnAchivableMove(Action<HexagonCell> d) { observerAchivableMove += d; }
    private void OnAchivableMove() { if (observerAchivableMove != null) observerAchivableMove(this); }

    public void CheckOnNeighbour(HexagonCell otherCell)
    {
#if UNITY_EDITOR_WIN
        Undo.RecordObject(this, "fill cell");
#endif
        if (Vector3.Distance(this.Position, otherCell.Position) <= (deltaSize.x * transform.localScale.x / 2 * 1.05f))
        {
            NeighbourCell newNeighbour = new NeighbourCell(this, otherCell);
            neighbours.Add(newNeighbour);
        }
    }
    public void ClearNeighbours() { neighbours.Clear(); }
    //Find way
    private int dist = 100;
    public int GetDist { get => dist; }
    private HexagonCell previousCell = null;
    public HexagonCell PreviousCell { get => previousCell; }
    bool checkNext = false;
    List<NeighbourCell> asumptionNeighbourCell = new List<NeighbourCell>();
    NeighbourDirection directionToTarget;
    public void FindWay(HexagonCell previousCell, HexagonCell target, TypeMovement typeMovement = TypeMovement.Ground, int step = 1)
    {
        if (available && (availableMove || (previousCell == null) || (this == target)))
        {
            if (step < dist)
            {
                checkNext = true;
                dist = step;
            }
            else if (step == dist)
            {
                checkNext = (UnityEngine.Random.Range(0f, 1f) > 0.5f);
            }
            else { checkNext = false; }
            if (checkNext)
            {
                if (this.previousCell == null) GridController.Instance.RegisterOnFoundWay(ClearFindWay);
                this.previousCell = previousCell;
                if ((target.GetDist > step) && (this != target))
                {
                    directionToTarget = NeighbourCell.GetDirection(this, target);
                    for (int level = 0; level <= 3; level++)
                    {
                        NeighbourCell.GetNeighboursOnLevelNear(neighbours, directionToTarget, level, asumptionNeighbourCell);
                        if (asumptionNeighbourCell.Count > 0) CheckNeighbour(previousCell, target, typeMovement, step);

                    }
                }
            }
        }
    }
    private void CheckNeighbour(HexagonCell previousCell, HexagonCell target, TypeMovement typeMovement, int step)
    {
        for (int i = 0; i < asumptionNeighbourCell.Count; i++)
        {
            if (asumptionNeighbourCell[i].Cell != previousCell) asumptionNeighbourCell[i].FindWay(this, target, typeMovement, step: (step + 1));

        }
    }
    public void ClearFindWay()
    {
        GridController.Instance.UnregisterOnFoundWay(ClearFindWay);
        dist = 100;
        previousCell = null;
        checkNext = false;
    }

    private void OnEndMatch()
    {
        observerClick = null;
        observerAchivableMove = null;
    }


    //Potision controller	
#if UNITY_EDITOR_WIN
    [ContextMenu("UpLeft")]
    public void MoveUpLeft() { Move(new Vector3(-deltaSize.x * transform.localScale.x / 2, deltaSize.y * transform.localScale.y, 0)); }

    [ContextMenu("UpRight")]
    public void MoveUpRight() { Move(new Vector3(deltaSize.x * transform.localScale.x / 2, deltaSize.y * transform.localScale.y, 0)); }

    [ContextMenu("Left")]
    public void MoveLeft() { Move(new Vector3(-deltaSize.x * transform.localScale.x, 0, 0)); }

    [ContextMenu("Right")]
    public void MoveRight() { Move(new Vector3(deltaSize.x * transform.localScale.x, 0, 0)); }

    [ContextMenu("DownLeft")]
    public void MoveDownLeft() { Move(new Vector3(-deltaSize.x * transform.localScale.x / 2, -deltaSize.y * transform.localScale.y, 0)); }

    [ContextMenu("DownRight")]
    public void MoveDownRight() { Move(new Vector3(deltaSize.x * transform.localScale.x / 2, -deltaSize.y * transform.localScale.y, 0)); }


    private void Move(Vector3 dir)
    {
        Undo.RecordObject(transform, "hexagon position");
        tr.position = tr.position + dir;
    }
#endif

}
