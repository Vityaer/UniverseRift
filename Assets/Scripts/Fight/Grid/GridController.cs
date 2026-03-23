using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fight.Common.FightInterface;
using Fight.Common.Grid;
using Fight.Common.HeroControllers.Generals;
using Models.Grid;
using Models.Heroes;
using Plugins.VContainerUi.Runtime.Abstraction;
using UniRx;
using UnityEngine;
using Utils.AsyncUtils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;
using HeroController = Fight.HeroControllers.Generals.HeroController;

namespace Fight.Grid
{
    public class GridController : UiController<GridView>, IInitializable, IDisposable
    {
        public static bool PlayerCanController = false;

        [Inject] private readonly GridFactory m_gridSpawner;
        [Inject] private readonly FightDirectionController m_fightDirectionController;

        private readonly CompositeDisposable m_disposables = new();
        private CancellationTokenSource m_tokenSource;
        private BaseGrid m_grid;
        private readonly Stack<HexagonCell> m_way = new();
        private HexagonCell m_previousCell;

        public readonly ReactiveCommand OnFinishFoundWay = new();

        public List<HexagonCell> GetLeftTeamPos => m_grid.StartCellsLeftTeam;
        public List<HexagonCell> GetRightTeamPos => m_grid.StartCellsRightTeam;
        public Transform RootTemplateObjects => View.ParentTemplateObjects;

        public void Initialize()
        {
            m_grid = m_gridSpawner.CreateGrid(View.Prefab, View.GridParent);
            m_grid.FindAllCell();
            foreach (HexagonCell cell in m_grid.Cells) cell.SetData(this);

            m_grid.CloseGrid();
        }

        public void OpenGrid()
        {
            m_grid.OpenGrid();
            StartFight();
        }

        public void ClearWays()
        {
            m_grid.ClearWays();
        }

        private void StartFight()
        {
            Camera.main.orthographic = false;
            View.CinemachineVirtual.Priority = 11;
            m_tokenSource = new CancellationTokenSource();
            CheckClick(m_tokenSource.Token).Forget();
        }

        private async UniTaskVoid CheckClick(CancellationToken cancellationToken)
        {
            RaycastHit2D hit;
            int cellLayer = LayerMask.GetMask("Grid", "Hero");
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Input.GetMouseButtonDown(0) && PlayerCanController)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit raycast, Mathf.Infinity, cellLayer))
                    {
                        if (raycast.collider.TryGetComponent<HexagonCell>(out HexagonCell hexagonCell))
                            hexagonCell.ClickOnMe();
                        else if (raycast.collider.TryGetComponent<HeroController>(out HeroController heroController))
                            heroController.ClickOnMe();
                    }
                }

                await UniTask.Yield(cancellationToken);
            }
        }

        public virtual Stack<HexagonCell> FindWay(HexagonCell startCell, HexagonCell finishCell,
            TypeMovement typeMovement = TypeMovement.Walk)
        {
            m_way.Clear();
            m_previousCell = null;
            startCell.FindWay(null, finishCell, typeMovement);
            m_way.Push(finishCell);
            do
            {
                if (finishCell != null)
                {
                    m_previousCell = finishCell.PreviousCell;
                    if (m_previousCell != null) m_way.Push(m_previousCell);
                }

                finishCell = m_previousCell;
            } while ((finishCell != startCell) && (finishCell != null));

            OnFinishFoundWay.Execute();
            return m_way;
        }

        public void FinishFight()
        {
            m_tokenSource.TryCancel();
            m_grid.CloseGrid();

            Camera.main.orthographic = true;
            View.CinemachineVirtual.Priority = 0;
        }

        public void ShowAttackDirections(Action<CellDirectionType> action, HexagonCell cell,
            List<NeighbourCell> neighbours)
        {
            //Debug.Log($"show attack directions: {neighbours.Count}");
            m_fightDirectionController.MelleeAttackController.RegisterOnSelectDirection(action, cell, neighbours);
        }

        public void Dispose()
        {
            m_disposables.Dispose();
        }
    }
}