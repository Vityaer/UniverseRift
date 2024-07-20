using City.Buildings.Abstractions;
using Common.Heroes;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using Models.Arenas;
using Models.Fights.Campaign;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace Fight.WarTable
{
    public class WarTableController : BaseBuilding<WarTableView>, IInitializable
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly FightController _fightController;

        private MissionModel _mission;
        private CompositeDisposable _disposables = new CompositeDisposable();

        private IDisposable _buttonAction;
        private Action<TeamContainer> _callback;
        private TeamContainer _playerTeam;
        private ReactiveCommand<TeamContainer> _onChangeTeam = new();

        public ReactiveCommand OnStartMission = new ReactiveCommand();
        public ReactiveCommand OnClose = new ReactiveCommand();
        public IObservable<TeamContainer> OnChangeTeam => _onChangeTeam;

        public void Initialize()
        {
            View.ListCardPanel.OnSelect.Subscribe(SelectCard).AddTo(_disposables);
            View.ListCardPanel.OnDiselect.Subscribe(UnSelectCard).AddTo(_disposables);

            View.StrengthLeftTeam.text = string.Empty;
            View.StrengthRightTeam.text = string.Empty;

            foreach (var place in View.LeftTeam)
            {
                place.OnClick.Subscribe(OnPlaceSelect).AddTo(_disposables);
            }
            Resolver.Inject(View.ListCardPanel);
        }

        private void OnPlaceSelect(WarriorPlace place)
        {
            if (!place.IsEmpty)
            {
                UnSelectCard(place.Hero);
            }
        }

        public void SelectCard(GameHero hero)
        {
            var result = AddHero(hero);
            if (result)
            {
                View.ListCardPanel.SelectCards(new List<GameHero> { hero });
            }
        }

        public void UnSelectCard(GameHero hero)
        {
            var result = RemoveHero(hero);
            if (result)
            {
                View.ListCardPanel.UnselectCards(new List<GameHero> { hero });
            }
        }

        private bool AddHero(GameHero hero)
        {
            var result = false;
            WarriorPlace selectedPlace = null;
            foreach (var place in View.LeftTeam)
            {
                if (place.IsEmpty)
                {
                    place.SetHero(hero);
                    UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
                    result = true;
                    selectedPlace = place;
                    break;
                }
            }

            if (result)
            {
                if (_playerTeam != null)
                {
                    _playerTeam.Heroes.Add(selectedPlace.Id, hero.HeroData.Id);
                    UnityEngine.Debug.Log($"AddHero: {hero.HeroData.Id} in team: {_playerTeam.Id}");
                    _onChangeTeam.Execute(_playerTeam);
                }

                CheckTeam(View.LeftTeam);
            }

            return result;
        }

        private bool RemoveHero(GameHero hero)
        {
            var result = false;
            var place = View.LeftTeam.Find(place => place.Hero == hero);

            if (place != null)
            {
                place.ClearPlace();

                if (_playerTeam != null)
                {
                    _playerTeam.Heroes.Remove(place.Id);
                    _onChangeTeam.Execute(_playerTeam);
                    UnityEngine.Debug.Log($"RemoveHero: {hero.HeroData.Id} in team: {_playerTeam.Id}");
                }

                CheckTeam(View.LeftTeam);
                UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
                result = true;

            }
            return result;
        }

        private void ClearPlaces(List<WarriorPlace> places)
        {
            foreach (var place in places)
            {
                if (!place.IsEmpty)
                    RemoveHero(place.Hero);
            }
        }

        private void UpdateStrengthTeam(List<WarriorPlace> team, TextMeshProUGUI textComponent)
        {
            float strengthTeam = 0f;
            for (int i = 0; i < team.Count; i++)
                if (team[i].Hero != null)
                    strengthTeam += team[i].Hero.Strength;
            textComponent.text = strengthTeam.ToString();
        }

        private void CheckTeam(List<WarriorPlace> team)
        {
            team = team.FindAll(x => x.Hero != null);
            //int racePeople = team.FindAll(x => x.Hero.General.Race == Race.People).Count;
            //int raceElf = team.FindAll(x => x.Hero.General.Race == Race.Elf).Count;
            //int raceUndead = team.FindAll(x => x.Hero.General.Race == Race.Undead).Count;
            //int raceDaemon = team.FindAll(x => x.Hero.General.Race == Race.Daemon).Count;
            //int raceGod = team.FindAll(x => x.Hero.General.Race == Race.God).Count;
            //int raceDarkGod = team.FindAll(x => x.Hero.General.Race == Race.Elemental).Count;
            //switch (team.Count)
            //{
            //    case 1:
            //        Debug.Log("one people");
            //        break;
            //    case 6:
            //        if (racePeople == 6)
            //        {
            //            Debug.Log("all people");
            //        }
            //        else if (raceElf == 6)
            //        {
            //            Debug.Log("all elf");
            //        }
            //        else if (raceUndead == 6)
            //        {
            //            Debug.Log("all undead");
            //        }
            //        else if (raceDaemon == 6)
            //        {
            //            Debug.Log("all daemon");
            //        }
            //        else if (raceGod == 6)
            //        {
            //            Debug.Log("all god");
            //        }
            //        else if (raceDarkGod == 6)
            //        {
            //            Debug.Log("all dark god");
            //        }
            //        else if ((raceElf == 3) && (racePeople == 3))
            //        {
            //            Debug.Log("3 elf and 3 people");
            //        }
            //        else if ((raceUndead == 3) && (raceDaemon == 3))
            //        {
            //            Debug.Log("3 undead and 3 Daemon");
            //        }
            //        else if ((raceGod == 3) && (raceDarkGod == 3))
            //        {
            //            Debug.Log("3 god and 3 darkgod");
            //        }
            //        else if ((racePeople == 2) && (raceElf == 2) && (raceGod == 2))
            //        {
            //            Debug.Log("this is Good");
            //        }
            //        else if ((raceUndead == 2) && (raceDaemon == 2) && (raceDarkGod == 2))
            //        {
            //            Debug.Log("this is Evil");
            //        }
            //        else if ((racePeople == 1) && (raceElf == 1) && (raceDaemon == 1) && (raceUndead == 1) && (raceGod == 1) && (raceDarkGod == 1))
            //        {
            //            Debug.Log("all race");
            //        }
            //        break;
            //}
            View.StartFightButton.interactable = View.LeftTeam.FindAll(place => place.Hero != null).Count > 0;
        }

        //API
        public void OpenTeamComposition(TeamContainer team, Action<TeamContainer> callback)
        {
            DisposeMainAction();
            _playerTeam = team;
            _callback = callback;
            _buttonAction = View.StartFightButton.OnClickAsObservable().Subscribe(_ => SaveTeam());
            MessagesPublisher.OpenWindowPublisher.OpenWindow<WarTableController>(openType: OpenType.Exclusive);
            ClearPlaces(View.LeftTeam);
            ClearPlaces(View.RightTeam);
            FillListHeroes(_heroesStorageController.ListHeroes);

            FillTeam(_playerTeam, View.LeftTeam);
            UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
            Open();
        }

        private void FillTeam(TeamContainer team, List<WarriorPlace> teamPlaces)
        {
            List<GameHero> heroes = new(team.Heroes.Count);

            foreach (var heroKeyValuePair in team.Heroes)
            {
                var suitablePlace = teamPlaces.Find(place => place.Id == heroKeyValuePair.Key);
                if (suitablePlace == null)
                    continue;

                var suitableHero = _heroesStorageController.ListHeroes.Find(hero => hero.HeroData.Id == heroKeyValuePair.Value);
                if(suitableHero == null)
                    continue;

                suitablePlace.SetHero(suitableHero);
                heroes.Add(suitableHero);
            }

            View.ListCardPanel.SelectCards(heroes);
        }

        private void SaveTeam()
        {
            _playerTeam.Heroes.Clear();
            foreach (var place in View.LeftTeam)
            {
                if (!place.IsEmpty)
                    _playerTeam.Heroes.Add(place.Id, place.Hero.HeroData.Id);
            }

            UnityEngine.Debug.Log($"_playerTeam.Heroes: {_playerTeam.Heroes.Count}");
            _callback.Invoke(_playerTeam);
            Close();
        }

        public void OpenMission(MissionModel mission, List<GameHero> listHeroes)
        {
            DisposeMainAction();
            _buttonAction = View.StartFightButton.OnClickAsObservable().Subscribe(_ => StartFight());
            MessagesPublisher.OpenWindowPublisher.OpenWindow<WarTableController>(openType: OpenType.Exclusive);
            ClearPlaces(View.LeftTeam);
            ClearPlaces(View.RightTeam);

            _mission = mission;

            for (var i = 0; i < mission.Units.Count; i++)
            {
                var enemyData = mission.Units[i];
                var model = _commonDictionaries.Heroes[enemyData.HeroId];
                var enemy = new GameHero(model, enemyData);
                View.RightTeam[i].SetHero(enemy);
            }

            UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
            UpdateStrengthTeam(View.RightTeam, View.StrengthRightTeam);

            CheckTeam(View.RightTeam);

            FillListHeroes(listHeroes);
            Open();
        }

        private void DisposeMainAction()
        {
            if(_buttonAction != null)
            {
                _buttonAction.Dispose();
                _buttonAction = null;
            }
        }

        public void OpenMission(MissionModel mission, TeamContainer teamContainer)
        {
            OpenMission(mission, _heroesStorageController.ListHeroes);
            _playerTeam = teamContainer;

            FillTeam(_playerTeam, View.LeftTeam);
            UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
        }

        public override void Close()
        {
            _playerTeam = null;
            OnClose.Execute();
            ClearPlaces(View.LeftTeam);
            ClearPlaces(View.RightTeam);
            base.Close();
        }

        private void FillListHeroes(List<GameHero> listHeroes)
        {
            View.ListCardPanel.ShowCards(listHeroes);
        }

        public async UniTaskVoid StartFight()
        {
            OnStartMission.Execute();
            await UniTask.Delay(200);
            base.Close();
            _fightController.SetMission(_mission, View.LeftTeam, View.RightTeam);
        }
    }
}