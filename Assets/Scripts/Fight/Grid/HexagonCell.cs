using System;
using System.Collections.Generic;
using Fight.Common.Grid;
using Fight.Common.HeroControllers.Generals;
using Fight.Common.Misc;
using Models.Heroes;
using UniRx;
using UnityEditor;
using UnityEngine;
using HeroController = Fight.HeroControllers.Generals.HeroController;

namespace Fight.Grid
{
    public class HexagonCell : MonoBehaviour
    {
        private GridController m_gridController;

        public bool available = true;
        public bool availableMove = true;
        public bool achievableMove = false;
        private int m_step = 0;
        public SpriteRenderer SpriteCell;
        public SpriteRenderer SpriteAvailable;

        [SerializeField] private List<NeighbourCell> neighbours = new();
        [SerializeField] private HeroController heroScript;
        [SerializeField] private Transform tr;

        private GameObject m_subject;
        private bool m_showAchievable = false;
        private static HeroController _requestHero = null;
        private static Action<HexagonCell> _observerClick, _observerSelectDirection, _observerAchivableMove;
        private int m_dist = 100;
        private HexagonCell m_previousCell = null;
        private IDisposable m_disposable;

        private bool m_checkNext = false;
        private readonly List<NeighbourCell> m_asumptionNeighbourCell = new();
        private CellDirectionType m_directionToTarget;

        private static Vector2 DeltaSize => Constants.Fight.CellDeltaStep;
        private int GetDist => m_dist;
        public Vector3 Position => transform.position;
        private List<NeighbourCell> GetAvailableNeighbours => neighbours.FindAll(x => x.available == true);
        public bool CanStand => availableMove && heroScript == null;
        public HeroController Hero => heroScript;
        public bool GetCanAttackCell => neighbours.Find(x => x.achievableMove == true) != null;
        public HexagonCell PreviousCell => m_previousCell;

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
            m_gridController = gridController;

        }

        public bool IsCellNeighbour(HexagonCell targetCell)
        {
            return neighbours.Find(neighbour => neighbour.Cell == targetCell) != null;
        }

        public void StartCheckMove(int step, HeroController newRequestHero, bool playerCanController)
        {
            m_gridController.ClearWays();
            _requestHero = newRequestHero;
            GridController.PlayerCanController = playerCanController;
            this.m_step = step;
            achievableMove = true;
            _requestHero.RegisterOnEndSelectCell(ClearCanMove);
            
            for (int i = 0; i < neighbours.Count; i++)
            {
                neighbours[i].CheckMove(step - 1);
            }
        }

        public void CheckMove(int step)
        {
            if (available && availableMove)
            {
                if (achievableMove == false || this.m_step < step)
                {
                    this.m_step = step;
                    if (achievableMove == false)
                    {
                        _requestHero.RegisterOnEndSelectCell(ClearCanMove);
                        OnAchivableMove();
                    }
                    achievableMove = true;

                    if (m_showAchievable == false/* && GridController.PlayerCanController*/)
                    {
                        m_showAchievable = true;
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
            List<NeighbourCell> achievableNeighbours = neighbours.FindAll(x => x.achievableMove);

            return achievableNeighbours.Count == 0
                ? null
                : achievableNeighbours[UnityEngine.Random.Range(0, achievableNeighbours.Count)]?.Cell;
        }

        public void RegisterOnSelectDirection(Action<HexagonCell> selectDirectionForHero, bool showUIDirection = true)
        {
            _observerSelectDirection = selectDirectionForHero;
            if (showUIDirection)
                ShowDirectionsAttack();
        }

        private void ShowDirectionsAttack()
        {
            m_gridController.ShowAttackDirections(SelectDirection, this, GetAvailableNeighbours);
        }

        private void SelectDirection(CellDirectionType direction)
        {
            if (_observerSelectDirection != null)
            {
                _observerSelectDirection(GetNeighbourCellOnDirection(direction));
                _observerSelectDirection = null;
            }
        }

        private HexagonCell GetNeighbourCellOnDirection(CellDirectionType direction)
        {
            HexagonCell result = neighbours.Find(x => x.direction == direction)?.Cell;
            return result;
        }

        public void ClearCanMove()
        {
            m_showAchievable = false;
            m_step = 0;
            achievableMove = false;
            SpriteAvailable.enabled = false;
            m_dist = 100;
            _requestHero?.UnregisterOnEndSelectCell(ClearCanMove);
        }

        public bool MyEnemyNear(Side masterSide)
        {
            var neigboursCellWithHeroes = neighbours.FindAll(neighbour => neighbour.GetHero != null);
            var enemy = neigboursCellWithHeroes.Find(neighbourCell => neighbourCell.GetHero.Side != masterSide);
            return enemy != null;
        }

        public void SetSubject(GameObject newSubject)
        {
            m_subject = newSubject;
        }

        public void SetHero(HeroController hero)
        {
            heroScript = hero;
            m_subject = hero.gameObject;
            availableMove = false;
        }

        public void SetColor(Color color)
        {
            SpriteAvailable.enabled = true;
            SpriteAvailable.color = color;
        }

        public void ClearSublject()
        {
            m_subject = null;
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

        public static void RegisterOnClick(Action<HexagonCell> d) { _observerClick += d; }
        public static void UnregisterOnClick(Action<HexagonCell> d) { _observerClick -= d; }
        private void OnClickCell() { if (_observerClick != null && available == true) _observerClick(this); }


        public static void RegisterOnAchivableMove(Action<HexagonCell> d) { _observerAchivableMove += d; }
        public static void UnregisterOnAchivableMove(Action<HexagonCell> d) { _observerAchivableMove -= d; }
        private void OnAchivableMove()
        {
            _observerAchivableMove?.Invoke(this);
        }

        public void CheckOnNeighbour(HexagonCell otherCell)
        {
#if UNITY_EDITOR_WIN
            Undo.RecordObject(this, "fill cell");
#endif
            if (Vector3.Distance(Position, otherCell.Position) <= DeltaSize.x * transform.localScale.x / 2 * 1.05f)
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


        public void FindWay(HexagonCell previousCell, HexagonCell target, TypeMovement typeMovement = TypeMovement.Walk, int step = 1)
        {
            if (available && (availableMove || previousCell == null || this == target))
            {
                if (step < m_dist)
                {
                    m_checkNext = true;
                    m_dist = step;
                }
                else if (step == m_dist)
                {
                    m_checkNext = UnityEngine.Random.Range(0f, 1f) > 0.5f;
                }
                else
                {
                    m_checkNext = false;
                }

                if (!m_checkNext)
                    return;

                if (m_previousCell == null)
                    m_disposable = m_gridController.OnFinishFoundWay.Subscribe(_ => ClearFindWay());

                m_previousCell = previousCell;
                if (target.GetDist > step && this != target)
                {
                    m_directionToTarget = NeighbourCell.GetDirection(this, target);
                    for (int level = 0; level <= 3; level++)
                    {
                        NeighbourCell.GetNeighboursOnLevelNear(neighbours, m_directionToTarget, level, m_asumptionNeighbourCell);
                        if (m_asumptionNeighbourCell.Count > 0) CheckNeighbour(previousCell, target, typeMovement, step);

                    }
                }
            }
        }
        private void CheckNeighbour(HexagonCell previousCell, HexagonCell target, TypeMovement typeMovement, int step)
        {
            foreach (NeighbourCell t in m_asumptionNeighbourCell)
            {
                if (t.Cell != previousCell) t.FindWay(this, target, typeMovement, step: step + 1);
            }
        }

        private void ClearFindWay()
        {
            m_disposable?.Dispose();
            m_dist = 100;
            m_previousCell = null;
            m_checkNext = false;
        }

        //Potision controller	
#if UNITY_EDITOR_WIN
        [ContextMenu("UpLeft")]
        private void MoveUpLeft() { Move(new Vector3(-DeltaSize.x * transform.localScale.x / 2, DeltaSize.y * transform.localScale.y, 0)); }

        [ContextMenu("UpRight")]
        private void MoveUpRight() { Move(new Vector3(DeltaSize.x * transform.localScale.x / 2, DeltaSize.y * transform.localScale.y, 0)); }

        [ContextMenu("Left")]
        private void MoveLeft() { Move(new Vector3(-DeltaSize.x * transform.localScale.x, 0, 0)); }

        [ContextMenu("Right")]
        private void MoveRight() { Move(new Vector3(DeltaSize.x * transform.localScale.x, 0, 0)); }

        [ContextMenu("DownLeft")]
        private void MoveDownLeft() { Move(new Vector3(-DeltaSize.x * transform.localScale.x / 2, -DeltaSize.y * transform.localScale.y, 0)); }

        [ContextMenu("DownRight")]
        private void MoveDownRight() { Move(new Vector3(DeltaSize.x * transform.localScale.x / 2, -DeltaSize.y * transform.localScale.y, 0)); }


        private void Move(Vector3 dir)
        {
            Undo.RecordObject(transform, "hexagon position");
            tr.position = tr.position + dir;
        }
#endif

    }
}