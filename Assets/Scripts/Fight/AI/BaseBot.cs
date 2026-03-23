using System;
using System.Collections.Generic;
using Fight.Common;
using Fight.Common.Misc;
using Fight.Grid;
using UniRx;
using VContainer;
using VContainer.Unity;
using HeroController = Fight.HeroControllers.Generals.HeroController;

namespace Fight.AI
{
    public class BaseBot : IInitializable, IDisposable
    {
        [Inject] private readonly FightController m_fightController;
        [Inject] private readonly GridController m_gridController;

        private List<Warrior> m_leftTeam;
        private List<Warrior> m_rightTeam;
        private readonly List<HexagonCell> m_achievableMoveCells = new();

        private Side m_sideForAI = Side.Right;
        private HeroController m_currentHero;
        private CompositeDisposable m_disposables;

        public void Initialize()
        {
            m_disposables = new();
            m_fightController.OnFinishFight.Subscribe(_ => FinishFight()).AddTo(m_disposables);
            HexagonCell.RegisterOnAchivableMove(AddAchivableMoveCell);
            HeroController.RegisterOnStartAction(OnHeroStartAction);
            HeroController.RegisterOnEndAction(ClearInfo);
            m_rightTeam = m_fightController.GetRightTeam;
            m_leftTeam = m_fightController.GetLeftTeam;
            ClearInfo();
        }

        public void SetSideAI(Side sideAi)
        {
            m_sideForAI = sideAi;
        }

        private void ClearInfo()
        {
            m_achievableMoveCells.Clear();
        }

        private void FinishFight()
        {
            m_disposables.Dispose();
            HeroController.UnregisterOnStartAction(OnHeroStartAction);
            HeroController.UnregisterOnEndAction(ClearInfo);
            HexagonCell.UnregisterOnAchivableMove(AddAchivableMoveCell);
        }

        private void OnHeroStartAction(HeroController heroController)
        {
            if (heroController.Side != m_sideForAI && m_sideForAI != Side.All) return;

            m_currentHero = heroController;
            List<Warrior> workTeam = heroController.Side == Side.Right ? m_leftTeam : m_rightTeam;

            if (heroController.Mellee)
            {
                List<Warrior> availableEnemies = workTeam.FindAll(x => x.Cell.GetCanAttackCell);
                Warrior enemy = null;
                if (availableEnemies.Count > 0)
                    enemy = availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];
                
                if (enemy != null && enemy.heroController != null)
                {
                    heroController.SelectDirectionAttack(enemy.Cell.GetAchivableNeighbourCell(), enemy.heroController);
                }
                else
                {
                    if (m_achievableMoveCells.Count == 0)
                        heroController.Cell.AITurn();
                    else
                        SelectCellForMove(m_achievableMoveCells, workTeam).AITurn();
                }
            }
            else
            {
                Warrior randomEnemy = workTeam[UnityEngine.Random.Range(0, workTeam.Count)];
                heroController.StartDistanceAttackOtherHero(randomEnemy.heroController);
            }
        }

        public bool CheckMeOnSubmission(Side side)
        {
            return side == m_sideForAI || m_sideForAI == Side.All;
        }

        private void AddAchivableMoveCell(HexagonCell newCell)
        {
            m_achievableMoveCells.Add(newCell);
        }

        private HexagonCell SelectCellForMove(List<HexagonCell> achievableMoveCells, List<Warrior> enemies)
        {
            HexagonCell result = achievableMoveCells[UnityEngine.Random.Range(0, achievableMoveCells.Count)];
            int min = 1000;
            Stack<HexagonCell> way;
            Stack<HexagonCell> minWay = new(0);

            foreach (Warrior enemy in enemies)
            {
                way = m_gridController.FindWay(m_currentHero.Cell, enemy.Cell);
                if (way.Count >= min) continue;

                min = way.Count;
                minWay = way;
            }

            for (int i = 0; i < minWay.Count; i++)
            {
                HexagonCell workCell = minWay.Pop();

                if (achievableMoveCells.Contains(workCell)) result = workCell;
            }

            return result;
        }

        void IDisposable.Dispose()
        {
            m_disposables.Dispose();
        }
    }
}