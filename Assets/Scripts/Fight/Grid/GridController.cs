using Cysharp.Threading.Tasks;
using Fight.HeroControllers.Generals;
using Models.Grid;
using Models.Heroes;
using System;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Fight.Grid
{
    public class GridController : UiController<GridView>, IInitializable, IDisposable
    {
        public static bool PlayerCanController = false;

        [Inject] private readonly GridFactory _gridSpawner;

        public ReactiveCommand OnFinishFoundWay = new ReactiveCommand();

        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private CompositeDisposable _disposables = new CompositeDisposable();
        private Stack<HexagonCell> _way = new Stack<HexagonCell>();
        private HexagonCell _previousCell = null;
        private BaseGrid _grid;

        public List<HexagonCell> Cells => _grid.Cells;
        public List<HexagonCell> GetLeftTeamPos => _grid.StartCellsLeftTeam;
        public List<HexagonCell> GetRightTeamPos => _grid.StartCellsRightTeam;
        public Transform RootTemplateObjects => View.ParentTemplateObjects;

        public void Initialize()
        {
            _grid = _gridSpawner.CreateGrid(View.Prefab, View.GridParent);
            foreach (var cell in _grid.Cells)
            {
                cell.SetData(this);
            }

            _grid.CloseGrid();
        }

        public void OpenGrid()
        {
            _grid.OpenGrid();
            StartFight();
        }

        private void StartFight()
        {
            CheckClick(_tokenSource.Token).Forget();
        }

        private async UniTaskVoid CheckClick(CancellationToken cancellationToken)
        {
            RaycastHit2D hit;
            while (true)
            {
                if (Input.GetMouseButtonDown(0) && PlayerCanController)
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
                await UniTask.Yield();
            }

        }

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
            OnFinishFoundWay.Execute();
            return _way;
        }

        public void FinishFight()
        {
            _tokenSource.Cancel();
            _grid.CloseGrid();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}