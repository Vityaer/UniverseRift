using Fight.HeroControllers.Generals;
using Models.Grid;
using Models.Heroes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.Grid
{
    public class GridController : MonoBehaviour
    {
        public static bool PlayerCanController = false;

        public Transform ParentTemplateObjects;
        public GridFactory GridSpawner;

        protected Action ObserverFoundWay;

        private Stack<HexagonCell> _way = new Stack<HexagonCell>();
        private HexagonCell _previousCell = null;
        private BaseGrid _grid;
        private Coroutine _coroutineCheckClick;

        public List<HexagonCell> Cells => _grid.Cells;
        public List<HexagonCell> GetLeftTeamPos => _grid.StartCellsLeftTeam;
        public List<HexagonCell> GetRightTeamPos => _grid.StartCellsRightTeam;

        public virtual Stack<HexagonCell> FindWay(HexagonCell startCell, HexagonCell finishCell, TypeMovement typeMovement = TypeMovement.Ground)
        {
            _way.Clear();
            _previousCell = null;
            startCell.FindWay(previousCell: null, target: finishCell, typeMovement: typeMovement);
            _way.Push(finishCell);
            do
            {
                if (finishCell != null)
                {
                    _previousCell = finishCell.PreviousCell;
                    if (_previousCell != null) _way.Push(_previousCell);
                }
                finishCell = _previousCell;
            } while ((finishCell != startCell) && (finishCell != null));
            OnFoundWay();
            return _way;
        }

        public void RegisterOnFoundWay(Action d)
        {
            ObserverFoundWay += d;
        }

        public void UnregisterOnFoundWay(Action d)
        {
            ObserverFoundWay -= d;
        }

        private void OnFoundWay()
        {
            if (ObserverFoundWay != null)
                ObserverFoundWay();
        }

        private void Start()
        {
            FightController.Instance.RegisterOnStartFight(StartFight);
            FightController.Instance.RegisterOnFinishFight(FinishFight);
            _grid = GridSpawner.CreateGrid();
            CloseGrid();
        }

        private void OnDestroy()
        {
            FightController.Instance.UnregisterOnStartFight(StartFight);
            FightController.Instance.UnregisterOnFinishFight(FinishFight);
        }

        private void StartFight()
        {
            _coroutineCheckClick = StartCoroutine(ICheckClick());
        }

        private IEnumerator ICheckClick()
        {
            RaycastHit2D hit;
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);
                    if (hit != null)
                    {
                        if (hit.transform != null)
                        {
                            if (hit.transform.CompareTag("HexagonCell"))
                            {
                                HexagonCell HexagonCell = hit.collider.transform.GetComponent<HexagonCell>();
                                HexagonCell.ClickOnMe();
                            }
                            if (hit.transform.CompareTag("Hero"))
                            {
                                hit.collider.transform.GetComponent<HeroController>().ClickOnMe();
                            }
                        }
                    }
                }
                yield return null;
            }

        }

        private void FinishFight()
        {
            StopCoroutine(_coroutineCheckClick);
            _coroutineCheckClick = null;
            CloseGrid();
        }

        public void OpenGrid()
        {
            _grid.OpenGrid();
        }

        public void CloseGrid()
        {
            _grid.CloseGrid();
        }

        private static GridController instance;
        public static GridController Instance => instance;
        void Awake()
        {
            instance = this;
        }
    }
}