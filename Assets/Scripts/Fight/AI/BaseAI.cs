using Fight.Grid;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.AI
{
    public class BaseAI : MonoBehaviour
    {
        public List<Warrior> leftTeam = new List<Warrior>();
        public List<Warrior> rightTeam = new List<Warrior>();
        public List<HexagonCell> achievableMoveCells = new List<HexagonCell>();

        private Side sideForAI = Side.Right;
        private HeroController currentHero = null;

        public void StartAI()
        {
            FightController.Instance.RegisterOnStartFight(StartFight);
            FightController.Instance.RegisterOnFinishFight(FinishFight);
            HexagonCell.RegisterOnAchivableMove(AddAchivableMoveCell);
            HeroController.RegisterOnStartAction(OnHeroStartAction);
            HeroController.RegisterOnEndAction(ClearInfo);
            ClearInfo();
        }

        void StartFight()
        {
            rightTeam = FightController.Instance.GetRightTeam;
            leftTeam = FightController.Instance.GetLeftTeam;
        }

        void ClearInfo()
        {
            achievableMoveCells.Clear();
        }

        void FinishFight()
        {
            FightController.Instance.UnregisterOnStartFight(StartFight);
            FightController.Instance.UnregisterOnFinishFight(FinishFight);
            HeroController.UnregisterOnStartAction(OnHeroStartAction);
            HeroController.UnregisterOnEndAction(ClearInfo);
            HexagonCell.UnregisterOnAchivableMove(AddAchivableMoveCell);
        }


        void OnHeroStartAction(HeroController heroConroller)
        {
            if (heroConroller.Side == sideForAI || sideForAI == Side.All)
            {
                currentHero = heroConroller;
                var workTeam = heroConroller.Side == Side.Right ? leftTeam : rightTeam;
                var availableEnemies = workTeam.FindAll(x => x.Cell.GetCanAttackCell == true);
                Warrior enemy = null;
                if (availableEnemies.Count > 0)
                    enemy = availableEnemies[Random.Range(0, availableEnemies.Count)];

                if (heroConroller.Mellee == true)
                {
                    if (enemy != null)
                    {
                        heroConroller.SelectDirectionAttack(enemy.Cell.GetAchivableNeighbourCell(), enemy.heroController);
                    }
                    else
                    {
                        SelectCellForMove(achievableMoveCells, workTeam).AITurn();
                    }
                }
                else
                {
                    if (enemy != null)
                        heroConroller.StartDistanceAttackOtherHero(enemy.heroController);
                }
            }
        }

        public bool CheckMeOnSubmission(Side side)
        {
            return side == sideForAI || sideForAI == Side.All;
        }

        void AddAchivableMoveCell(HexagonCell newCell)
        {
            achievableMoveCells.Add(newCell);
        }

        HexagonCell SelectCellForMove(List<HexagonCell> achievableMoveCells, List<Warrior> enemies)
        {
            HexagonCell result = achievableMoveCells[Random.Range(0, achievableMoveCells.Count)];
            int min = 1000;
            Stack<HexagonCell> way = new Stack<HexagonCell>(), minWay = new Stack<HexagonCell>(0);
            Warrior selectEnemy = null;
            for (int i = 0; i < enemies.Count; i++)
            {
                way = GridController.Instance.FindWay(currentHero.Cell, enemies[i].Cell);
                if (way.Count < min)
                {
                    min = way.Count;
                    selectEnemy = enemies[i];
                    minWay = way;
                }
            }
            HexagonCell workCell = null;
            for (int i = 0; i < minWay.Count; i++)
            {
                workCell = minWay.Pop();

                if (achievableMoveCells.Contains(workCell))
                {
                    result = workCell;
                }
            }
            return result;
        }

    }
}