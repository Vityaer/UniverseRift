using Cysharp.Threading.Tasks;
using Fight.FightInterface;
using Fight.HeroControllers.Generals;
using Models.Grid;
using Models.Heroes;
using System;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using Utils.AsyncUtils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Fight.Grid
{
    public class GridController : UiController<GridView>, IInitializable, IDisposable
    {
        public static bool PlayerCanController = false;

        [Inject] private readonly GridFactory _gridSpawner;
        [Inject] private readonly FightDirectionController _fightDirectionController;

        public ReactiveCommand OnFinishFoundWay = new ReactiveCommand();

        private CancellationTokenSource _tokenSource;
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
            Debug.Log("GridController initialized");
            _grid = _gridSpawner.CreateGrid(View.Prefab, View.GridParent);
            _grid.FindAllCell();
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
        
        public void ClearWays()
        {
            _grid.ClearWays();
        }

        private void StartFight()
        {
            Camera.main.orthographic = false;
            View.CinemachineVirtual.Priority = 11;
            _tokenSource = new CancellationTokenSource();
            CheckClick(_tokenSource.Token).Forget();
        }

        private async UniTaskVoid CheckClick(CancellationToken cancellationToken)
        {
            RaycastHit2D hit;
            var cellLayer = LayerMask.GetMask("Grid", "Hero");
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Input.GetMouseButtonDown(0) && PlayerCanController)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var raycast, Mathf.Infinity, cellLayer))
                    {
                        if (raycast.collider.TryGetComponent<HexagonCell>(out var hexagonCell))
                        {
                            hexagonCell.ClickOnMe();
                        }
                        else if (raycast.collider.TryGetComponent<HeroController>(out var heroController))
                        {
                            heroController.ClickOnMe();
                        }
                    }
                }
                await UniTask.Yield(cancellationToken: cancellationToken);
            }

        }

        public virtual Stack<HexagonCell> FindWay(HexagonCell startCell, HexagonCell finishCell, TypeMovement typeMovement = TypeMovement.Walk)
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
            _tokenSource.TryCancel();
            _grid.CloseGrid();

            Camera.main.orthographic = true;
            View.CinemachineVirtual.Priority = 0;
        }

        public void ShowAttackDirections(Action<CellDirectionType> action, HexagonCell cell, List<NeighbourCell> neighbours)
        {
            //Debug.Log($"show attack directions: {neighbours.Count}");
            _fightDirectionController.MelleeAttackController.RegisterOnSelectDirection(action, cell, neighbours);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }


    }
}