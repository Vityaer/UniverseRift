using System.Collections.Generic;
using UnityEngine;
using System;
using Models.Heroes;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using UniRx;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif

namespace Fight.Grid
{
    public class HexagonCell : MonoBehaviour
    {
        private GridController _gridController;

        public bool available = true;
        public bool availableMove = true;
        public bool achievableMove = false;
        public int step = 0;
        public SpriteRenderer SpriteCell;
        public SpriteRenderer SpriteAvailable;

        [SerializeField] private List<NeighbourCell> neighbours = new List<NeighbourCell>();
        [SerializeField] private HeroController heroScript;
        [SerializeField] private Transform tr;

        private GameObject subject;
        private bool showAchievable = false;
        private static HeroController requestHero = null;
        private static Action<HexagonCell> observerClick, observerSelectDirection, observerAchivableMove;
        private int _dist = 100;
        private HexagonCell _previousCell = null;
        private IDisposable _disposable;

        public HexagonCell PreviousCell { get => _previousCell; }
        bool checkNext = false;
        List<NeighbourCell> asumptionNeighbourCell = new List<NeighbourCell>();
        CellDirectionType directionToTarget;

        public int GetDist { get => _dist; }
        public Vector3 Position { get => transform.position; }
        public List<NeighbourCell> GetAvailableNeighbours { get => neighbours.FindAll(x => x.available == true); }
        public bool CanStand { get => availableMove && heroScript == null; }
        public HeroController Hero { get => heroScript; }
        public bool GetCanAttackCell { get => neighbours.Find(x => x.achievableMove == true) != null; }
        private Vector2 deltaSize => Constants.Fight.CellDeltaStep;

        void Awake()
        {
            tr = transform;
        }

        void Start()
        {
            SpriteAvailable.color = Constants.Colors.ACHIEVABLE_CELL_COLOR;
        }

        public void SetData(GridController gridController)
        {
            _gridController = gridController;

        }

        public void StartCheckMove(int step, HeroController newRequestHero, bool playerCanController)
        {
            requestHero = newRequestHero;
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
                if (achievableMove == false || this.step < step)
                {
                    this.step = step;
                    if (achievableMove == false)
                    {
                        requestHero.RegisterOnEndSelectCell(ClearCanMove);
                        OnAchivableMove();
                    }
                    achievableMove = true;

                    if (showAchievable == false/* && GridController.PlayerCanController*/)
                    {
                        showAchievable = true;
                        SpriteAvailable.enabled = true;
                        SpriteAvailable.color = Constants.Colors.ACHIEVABLE_CELL_COLOR;
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
            _gridController.ShowAttackDirections(SelectDirection, this, GetAvailableNeighbours);
        }

        private void SelectDirection(CellDirectionType direction)
        {
            if (observerSelectDirection != null)
            {
                observerSelectDirection(GetNeighbourCellOnDirection(direction));
                observerSelectDirection = null;
            }
        }

        private HexagonCell GetNeighbourCellOnDirection(CellDirectionType direction)
        {
            HexagonCell result = neighbours.Find(x => x.direction == direction).Cell;
            return result;
        }

        private void OnSelectDirection()
        {
            if (observerClick != null && available == true)
                observerClick(this);
        }

        public void ClearCanMove()
        {
            showAchievable = false;
            step = 0;
            achievableMove = false;
            SpriteAvailable.enabled = false;
            _dist = 100;
            requestHero?.UnregisterOnEndSelectCell(ClearCanMove);
        }

        public bool MyEnemyNear(Side masterSide)
        {
            var neigboursCellWithHeroes = neighbours.FindAll(neighbour => neighbour.GetHero != null);
            var enemy = neigboursCellWithHeroes.Find(neighbourCell => neighbourCell.GetHero.Side != masterSide);
            return enemy != null;
        }

        public void SetSubject(GameObject newSubject)
        {
            subject = newSubject;
        }

        public void SetHero(HeroController hero)
        {
            heroScript = hero;
            subject = hero.gameObject;
            availableMove = false;
        }

        public void SetColor(Color color)
        {
            SpriteAvailable.enabled = true;
            SpriteAvailable.color = color;
        }

        public void ClearSublject()
        {
            subject = null;
            heroScript = null;
            availableMove = true;
            SpriteAvailable.enabled = false;
        }

        public void ClickOnMe()
        {
            if (GridController.PlayerCanController)
            {
                if (achievableMove || heroScript != null)
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
        private void OnClickCell() { if (observerClick != null && available == true) observerClick(this); }


        public static void RegisterOnAchivableMove(Action<HexagonCell> d) { observerAchivableMove += d; }
        public static void UnregisterOnAchivableMove(Action<HexagonCell> d) { observerAchivableMove += d; }
        private void OnAchivableMove() { if (observerAchivableMove != null) observerAchivableMove(this); }

        public void CheckOnNeighbour(HexagonCell otherCell)
        {
#if UNITY_EDITOR_WIN
            Undo.RecordObject(this, "fill cell");
#endif
            if (Vector3.Distance(Position, otherCell.Position) <= deltaSize.x * transform.localScale.x / 2 * 1.05f)
            {
                NeighbourCell newNeighbour = new NeighbourCell(this, otherCell);
                neighbours.Add(newNeighbour);
            }
        }
        public void ClearNeighbours()
        {
            neighbours.Clear();
        }
        //Find way


        public void FindWay(HexagonCell previousCell, HexagonCell target, TypeMovement typeMovement = TypeMovement.Ground, int step = 1)
        {
            if (available && (availableMove || previousCell == null || this == target))
            {
                if (step < _dist)
                {
                    checkNext = true;
                    _dist = step;
                }
                else if (step == _dist)
                {
                    checkNext = UnityEngine.Random.Range(0f, 1f) > 0.5f;
                }
                else
                {
                    checkNext = false;
                }

                if (!checkNext)
                    return;

                if (_previousCell == null)
                    _disposable = _gridController.OnFinishFoundWay.Subscribe(_ => ClearFindWay());

                _previousCell = previousCell;
                if (target.GetDist > step && this != target)
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
        private void CheckNeighbour(HexagonCell previousCell, HexagonCell target, TypeMovement typeMovement, int step)
        {
            for (int i = 0; i < asumptionNeighbourCell.Count; i++)
            {
                if (asumptionNeighbourCell[i].Cell != previousCell) asumptionNeighbourCell[i].FindWay(this, target, typeMovement, step: step + 1);

            }
        }

        public void ClearFindWay()
        {
            _disposable.Dispose();
            _dist = 100;
            _previousCell = null;
            checkNext = false;
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
}