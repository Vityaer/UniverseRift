using Assets.Scripts.Fight.Common;
using Cysharp.Threading.Tasks;
using Fight.Comparers;
using Fight.Factories;
using Fight.FightInterface;
using Fight.Grid;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using Fight.WarTable;
using Models.Fights.Campaign;
using System;
using System.Collections.Generic;
using UIController.FightUI;
using UniRx;
using UnityEngine;
using VContainer;
using VContainerUi.Abstraction;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace Fight
{
    public partial class FightController : UiController<FightView>
    {
        [Inject] private readonly HeroFactory _heroFactory;
        [Inject] private readonly GridController _gridController;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;
        [Inject] private readonly FightDirectionController _fightDirectionController;

        private const int MAX_ROUND_COUNT = 15;
        private const int TICK_DELAY = 750;

        [Header("Place heroes")]
        private List<Warrior> _leftTeam = new List<Warrior>();
        private List<Warrior> _rightTeam = new List<Warrior>();
        private List<HeroController> _listInitiative = new List<HeroController>();
        private List<HeroController> _listHeroesWithAction = new List<HeroController>();

        public ReactiveCommand OnEndRound = new ReactiveCommand();
        public ReactiveCommand OnStartFight = new ReactiveCommand();
        public ReactiveCommand OnFinishFight = new ReactiveCommand();
        public ReactiveCommand AfterCreateFight = new();

        private bool _isFightFinish = false;
        private int _currentHeroIndex = -1;
        private int _round = 1;
        private MissionModel _mission;
        private ReactiveCommand<FightResultType> _onFightResult = new ReactiveCommand<FightResultType>();

        public List<Warrior> GetLeftTeam => _leftTeam;
        public List<Warrior> GetRightTeam => _rightTeam;
        public IObservable<FightResultType> OnFigthResult => _onFightResult;
        public HeroController GetCurrentHero() => _listInitiative[_currentHeroIndex];

        public void SetMission(MissionModel mission, List<WarriorPlace> leftWarriorPlace, List<WarriorPlace> rightWarriorPlace)
        {
            _messagesPublisher.OpenWindowPublisher.OpenWindow<FightWindow>(openType: OpenType.Exclusive);
            _isFightFinish = false;
            _mission = mission;
            //View.LocationController.OpenLocation(mission.Location);
            _gridController.OpenGrid();
            CreateTeams(leftWarriorPlace, rightWarriorPlace);
        }

        private void CreateTeams(List<WarriorPlace> leftWarriorPlace, List<WarriorPlace> rightWarriorPlace)
        {
            CreateTeam(_gridController.GetLeftTeamPos, leftWarriorPlace, Side.Left, _leftTeam);
            CreateTeam(_gridController.GetRightTeamPos, rightWarriorPlace, Side.Right, _rightTeam);
            WaitDelayBeforeStartFight().Forget();
        }

        private async UniTaskVoid WaitDelayBeforeStartFight()
        {
            await UniTask.Delay(1000);
            AfterCreateFight.Execute();

            for (int i = 3; i > 0; i--)
            {
                View.NumRoundText.text = $"{i}";
                await UniTask.Delay(TICK_DELAY);

            }
            View.NumRoundText.text = "Fight!";

            await UniTask.Delay(500);

            View.NumRoundText.text = $"Round {_round}";
            OnStartFight.Execute();
            _listInitiative.Sort(new HeroInitiativeComparer());
            StartFight();
        }

        private void CreateTeam(List<HexagonCell> teamPos, List<WarriorPlace> team, Side side, List<Warrior> warriorTeam)
        {
            for (var i = 0; i < team.Count; i++)
            {
                if (team[i].Hero != null)
                {
                    var hero = _heroFactory.Create(team[i].Hero, teamPos[i], side, _gridController.RootTemplateObjects);
                    warriorTeam.Add(new Warrior(hero));
                    hero.SetData(team[i].Hero, teamPos[i], side);
                    _listInitiative.Add(hero);
                }
            }
        }

        //Fight loop    	
        public void StartFight()
        {
            NextHero();
        }

        public void AddHeroWithAction(HeroController newHero)
        {
            _listHeroesWithAction.Add(newHero);
            //Debug.Log($"AddHeroWithAction {newHero.name} стало: {_listHeroesWithAction.Count}");
        }

        public void RemoveHeroWithAction(HeroController removeHero)
        {
            _listHeroesWithAction.Remove(removeHero);
            //Debug.Log($"RemoveHeroWithAction {removeHero.name} осталось: {_listHeroesWithAction.Count}");
            if (_listHeroesWithAction.Count == 0)
                NextHero();
        }

        public void RemoveHeroWithActionAll(HeroController removeHero)
        {
            _listHeroesWithAction.RemoveAll(x => x == removeHero);
            //Debug.Log($"RemoveHeroWithActionAll {removeHero.name}, осталось: {_listHeroesWithAction.Count}");

            if (_listHeroesWithAction.Count == 0)
                NextHero();
        }

        private void NextHero()
        {
            if ((_currentHeroIndex + 1) < _listInitiative.Count)
            {
                _currentHeroIndex++;
            }
            else
            {
                NewRound();
            }

            if (!_isFightFinish)
            {
                _listInitiative[_currentHeroIndex].DoTurn();
            }
        }

        private void NewRound()
        {
            UpdateListInitiative();
            _currentHeroIndex = 0;
            _round++;
            View.NumRoundText.text = $"Round {_round}";
            OnEndRound.Execute();
            _fightDirectionController.ClearData();
            if (_round == MAX_ROUND_COUNT)
            {
                Win(Side.Right);
            }
        }

        private void UpdateListInitiative()
        {
            _listInitiative.Clear();

            for (int i = 0; i < _leftTeam.Count; i++)
            {
                if (_leftTeam[i].heroController != null)
                {
                    _listInitiative.Add(_leftTeam[i].heroController);
                }
                else
                {
                    Debug.Log("left team hero null");
                }
            }

            for (int i = 0; i < _rightTeam.Count; i++)
            {
                if (_rightTeam[i].heroController != null)
                {
                    _listInitiative.Add(_rightTeam[i].heroController);
                }
                else
                {
                    Debug.Log("Right team hero null");
                }
            }
            _listInitiative.Sort(new HeroInitiativeComparer());
        }

        public void WaitTurn()
        {
            var hero = GetCurrentHero();
            _listInitiative.Remove(hero);
            _currentHeroIndex -= 1;
            _listInitiative.Add(hero);
            hero.StartWait();
        }

        //Result fight
        private void CheckFinishFight()
        {
            if ((_leftTeam.Count == 0) || (_rightTeam.Count == 0))
                Win(_leftTeam.Count > 0 ? Side.Left : Side.Right);
        }

        private void Win(Side side)
        {
            _isFightFinish = true;
            _fightDirectionController.CloseControllers();
            FinishFightCountdown(side).Forget();
        }

        private async UniTaskVoid FinishFightCountdown(Side side)
        {
            View.NumRoundText.text = "Конец боя!";

            if (side == Side.Right) CheckSaveResult();
            await UniTask.Delay(2500);

            var fightResult = (side == Side.Left) ? FightResultType.Win : FightResultType.Defeat;
            OnFinishFight.Execute();
            View.NumRoundText.text = string.Empty;
            ClearAll();

            _messagesPublisher.MessageCloseWindowPublisher.CloseWindow<FightWindow>();
            if (side == Side.Left)
            {
                //MessageController.Instance.AddMessage("Ты выиграл!");
                _onFightResult.Execute(FightResultType.Win);
            }
            else
            {
                //MessageController.Instance.AddMessage("Ты проиграл!");
                _onFightResult.Execute(FightResultType.Defeat);
            }
        }

        void ClearAll()
        {
            _gridController.FinishFight();
            DeleteTeam(_rightTeam);
            DeleteTeam(_leftTeam);
            _listInitiative.Clear();
            _listHeroesWithAction.Clear();
            _currentHeroIndex = -1;
            _round = 1;
        }

        void DeleteTeam(List<Warrior> team)
        {
            for (int i = 0; i < team.Count; i++)
            {
                team[i].heroController.DeleteHero();
            }
            team.Clear();
        }

        void CheckSaveResult()
        {
            //if ((_mission is BossMission))
            //    ((BossMission)_mission).SaveResult();
        }

        //API
        public void MessageAboutDamageForAttacker(float damage)
        {
            _listInitiative[_currentHeroIndex].MessageDamageAfterStrike(damage);
        }

        public void DeleteHero(HeroController heroForDelete)
        {
            RemoveHeroWithActionAll(heroForDelete);
            Warrior warrior = _leftTeam.Find(x => x.heroController == heroForDelete);
            if (warrior != null)
            {
                _leftTeam.Remove(warrior);
            }
            else
            {
                warrior = _rightTeam.Find(x => x.heroController == heroForDelete);
                _rightTeam.Remove(warrior);
            }
            _listInitiative.Remove(heroForDelete);
            CheckFinishFight();
        }


        private void KillRightTeam()
        {
            DeleteTeam(_rightTeam);
            CheckFinishFight();
        }


    }
}

