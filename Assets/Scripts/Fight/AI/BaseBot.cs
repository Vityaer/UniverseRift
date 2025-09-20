using System.Collections.Generic;
using VContainer;
using UniRx;
using VContainer.Unity;
using System;
using Fight.Common.Grid;
using Fight.Common.HeroControllers.Generals;
using Fight.Common.Misc;

namespace Fight.Common.AI
{
    public class BaseBot : IInitializable, IDisposable
    {
        [Inject] private readonly Common.FightController _fightController;
        [Inject] private readonly GridController _gridController;

        public List<Warrior> _leftTeam;
        public List<Warrior> _rightTeam;
        public List<HexagonCell> _achievableMoveCells = new List<HexagonCell>();

        private Side _sideForAI = Side.Right;
        private HeroController _currentHero = null;
        private CompositeDisposable _disposables;

        public void Initialize()
        {
            _disposables = new();
            _fightController.OnFinishFight.Subscribe(_ => FinishFight()).AddTo(_disposables);
            HexagonCell.RegisterOnAchivableMove(AddAchivableMoveCell);
            HeroController.RegisterOnStartAction(OnHeroStartAction);
            HeroController.RegisterOnEndAction(ClearInfo);
            _rightTeam = _fightController.GetRightTeam;
            _leftTeam = _fightController.GetLeftTeam;
            ClearInfo();
        }

        public void SetSideAI(Side sideAi)
        {
            _sideForAI = sideAi;
        }

        private void ClearInfo()
        {
            _achievableMoveCells.Clear();
        }

        private void FinishFight()
        {
            _disposables.Dispose();
            HeroController.UnregisterOnStartAction(OnHeroStartAction);
            HeroController.UnregisterOnEndAction(ClearInfo);
            HexagonCell.UnregisterOnAchivableMove(AddAchivableMoveCell);
        }

        private void OnHeroStartAction(HeroController heroConroller)
        {
            if (heroConroller.Side == _sideForAI || _sideForAI == Side.All)
            {
                _currentHero = heroConroller;
                var workTeam = heroConroller.Side == Side.Right ? _leftTeam : _rightTeam;
                var availableEnemies = workTeam.FindAll(x => x.Cell.GetCanAttackCell == true);
                Warrior enemy = null;
                if (availableEnemies.Count > 0)
                    enemy = availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];

                if (heroConroller.Mellee)
                {
                    if (enemy != null && enemy.heroController != null)
                    {
                        heroConroller.SelectDirectionAttack(enemy.Cell.GetAchivableNeighbourCell(), enemy.heroController);
                    }
                    else
                    {
                        if (_achievableMoveCells.Count == 0)
                        {
                            heroConroller.Cell.AITurn();
                        }
                        else
                        {
                            SelectCellForMove(_achievableMoveCells, workTeam).AITurn();
                        }

                    }
                }
                else
                {
                    var randomEnemy = workTeam[UnityEngine.Random.Range(0, workTeam.Count)];
                    heroConroller.StartDistanceAttackOtherHero(randomEnemy.heroController);
                }
            }
        }

        public bool CheckMeOnSubmission(Side side)
        {
            return side == _sideForAI || _sideForAI == Side.All;
        }

        private void AddAchivableMoveCell(HexagonCell newCell)
        {
            _achievableMoveCells.Add(newCell);
        }

        private HexagonCell SelectCellForMove(List<HexagonCell> achievableMoveCells, List<Warrior> enemies)
        {

            var result = achievableMoveCells[UnityEngine.Random.Range(0, achievableMoveCells.Count)];
            var min = 1000;
            var way = new Stack<HexagonCell>();
            var minWay = new Stack<HexagonCell>(0);

            Warrior selectEnemy;
            for (int i = 0; i < enemies.Count; i++)
            {
                way = _gridController.FindWay(_currentHero.Cell, enemies[i].Cell);
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

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}