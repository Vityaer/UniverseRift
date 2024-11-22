using Assets.Scripts.Fight.Common;
using City.TrainCamp.HeroInstances;
using Cysharp.Threading.Tasks;
using Fight.Comparers;
using Fight.Factories;
using Fight.FightInterface;
using Fight.Grid;
using Fight.HeroControllers.Generals;
using Fight.Misc;
using Fight.UI;
using Fight.WarTable;
using LocalizationSystems;
using Models.Fights.Campaign;
using System;
using System.Collections.Generic;
using UI.Utils.Localizations.Extensions;
using UIController.FightUI;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using VContainer;
using VContainerUi.Abstraction;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace Fight
{
    public partial class FightController : UiController<FightView>
    {
        private const string START_FIGHT_LOCALIZATION_KEY = "StartFightLabel";
        private const string FINISH_FIGHT_LOCALIZATION_KEY = "FinishFightLabel";
        private const string START_TIME_LOCALIZATION_KEY = "CurrentTimeStartFightLabel";
        private const string ROUND_LOCALIZATION_KEY = "CurrentRoundLabel";
        private const int MAX_ROUND_COUNT = 15;
        private const int TICK_DELAY = 750;

        [Inject] private readonly HeroFactory _heroFactory;
        [Inject] private readonly GridController _gridController;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;
        [Inject] private readonly FightDirectionController _fightDirectionController;
        [Inject] private readonly HeroInstancesController _heroInstancesController;
        [Inject] private readonly ILocalizationSystem _localizationSystem;
        private FightPanelController _fightPanelController;

        [Header("Place heroes")]
        private List<Warrior> _leftTeam = new();
        private List<Warrior> _rightTeam = new();
        private List<HeroController> _listInitiative = new();
        private List<HeroController> _listHeroesWithAction = new();

        public ReactiveCommand<LocalizedString> OnChangeFightUiText = new();
        public ReactiveCommand OnEndRound = new ReactiveCommand();
        public ReactiveCommand OnStartFight = new ReactiveCommand();
        public ReactiveCommand OnFinishFight = new ReactiveCommand();
        public ReactiveCommand AfterCreateFight = new();

        private bool _isFightFinish = false;
        private int _currentHeroIndex = -1;
        private int _round = 1;
        private MissionModel _mission;
        private ReactiveCommand<FightResultType> _onFightResult = new();

        public List<Warrior> GetLeftTeam => _leftTeam;
        public List<Warrior> GetRightTeam => _rightTeam;
        public IObservable<FightResultType> OnFigthResult => _onFightResult;
        public HeroController GetCurrentHero() => _listInitiative[_currentHeroIndex];

        public void SetControllerUi(FightPanelController fightPanelController)
        {
            _fightPanelController = fightPanelController;
        }

        public void SetMission(MissionModel mission, List<WarriorPlace> leftWarriorPlace, List<WarriorPlace> rightWarriorPlace)
        {
            _messagesPublisher.OpenWindowPublisher.OpenWindow<FightWindow>(openType: OpenType.Exclusive);
            _isFightFinish = false;
            _mission = mission;
            //View.LocationController.OpenLocation(mission.Location);
            _heroInstancesController.HideLight();
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
                var localizeTime = _localizationSystem.LocalizationUiContainer.GetLocalizedContainer(START_TIME_LOCALIZATION_KEY)
                    .WithArguments( new List<object>{i} );

                OnChangeFightUiText.Execute(localizeTime);
                await UniTask.Delay(TICK_DELAY);

            }

            var localize = _localizationSystem.LocalizationUiContainer.GetLocalizedContainer(START_FIGHT_LOCALIZATION_KEY);
            OnChangeFightUiText.Execute(localize);

            await UniTask.Delay(500);

            ShowRoundInfo();
            OnStartFight.Execute();
            _listInitiative.Sort(new HeroInitiativeComparer());
            StartFight();
        }

        private void ShowRoundInfo()
        {
            var localize = _localizationSystem.LocalizationUiContainer.GetLocalizedContainer(ROUND_LOCALIZATION_KEY)
                .WithArguments( new List<object>{_round} );
            OnChangeFightUiText.Execute(localize);
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

        public void WaitTurn()
        {
            var hero = GetCurrentHero();
            _listInitiative.Remove(hero);
            _currentHeroIndex -= 1;
            _listInitiative.Add(hero);
            hero.StartWait();
        }

        public void DefenseTurn()
        {
            var hero = GetCurrentHero();
            hero.StartDefend();
        }

        public void SpellTurn()
        {
            var hero = GetCurrentHero();
            hero.UseSpecialSpell();
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
                _fightPanelController.SetHeroStatus(_listInitiative[_currentHeroIndex]);
            }
        }

        private void NewRound()
        {
            UpdateListInitiative();
            
            foreach (var hero in _listInitiative)
                hero.Refresh();
        
            _currentHeroIndex = 0;
            _round++;

            ShowRoundInfo();

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

            var localize = _localizationSystem.LocalizationUiContainer.GetLocalizedContainer(FINISH_FIGHT_LOCALIZATION_KEY);
            OnChangeFightUiText.Execute(localize);

            if (side == Side.Right) CheckSaveResult();
            await UniTask.Delay(500);
            OnFinishFight.Execute();
            _messagesPublisher.MessageCloseWindowPublisher.CloseWindow<FightWindow>();

            await UniTask.Delay(2000);
            var fightResult = (side == Side.Left) ? FightResultType.Win : FightResultType.Defeat;
            //View.NumRoundText.text = string.Empty;
            ClearAll();

            _onFightResult.Execute(fightResult);
        }

        private void ClearAll()
        {
            _gridController.FinishFight();
            _heroInstancesController.OpenLight();
            DeleteTeam(_rightTeam);
            DeleteTeam(_leftTeam);
            _listInitiative.Clear();
            _listHeroesWithAction.Clear();
            _currentHeroIndex = -1;
            _round = 1;
        }

        private void DeleteTeam(List<Warrior> team)
        {
            for (int i = 0; i < team.Count; i++)
            {
                team[i].heroController.DeleteHero();
            }
            team.Clear();
        }

        private void CheckSaveResult()
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

