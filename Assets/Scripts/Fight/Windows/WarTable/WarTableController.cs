using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using City.Buildings.Abstractions;
using Common.Db.CommonDictionaries;
using Common.Heroes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fight.Common.WarTable;
using Hero;
using Models.Arenas;
using Models.Fights.Campaign;
using TMPro;
using UniRx;
using UnityEngine;
using Utils.AsyncUtils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace Fight.Windows.WarTable
{
    public class WarTableController : BaseBuilding<WarTableView>, IInitializable
    {
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly HeroesStorageController m_heroesStorageController;
        [Inject] private readonly Common.FightController m_fightController;

        private readonly CompositeDisposable m_disposables = new();
        private readonly ReactiveCommand<TeamContainer> m_onChangeTeam = new();

        private MissionModel m_mission;
        private IDisposable m_buttonAction;
        private Action<TeamContainer> m_callback;
        private TeamContainer m_playerTeam;

        private bool m_canStartBattle;
        
        private bool m_isDragging;
        private WarriorPlace m_startDragPlace;
        private CancellationTokenSource m_cancellationTokenSource;
        private Tween m_dragTween;

        public readonly ReactiveCommand OnStartMission = new();
        public readonly ReactiveCommand OnPlayerStartFight = new();
        public readonly ReactiveCommand OnClose = new();
        public IObservable<TeamContainer> OnChangeTeam => m_onChangeTeam;
        public bool IsFastFight => View.FastFightToggle.IsOn;
        
        public void Initialize()
        {
            View.ListCardPanel.OnSelect.Subscribe(SelectCard).AddTo(m_disposables);
            View.ListCardPanel.OnDiselect.Subscribe(UnSelectCard).AddTo(m_disposables);

            View.StrengthLeftTeam.text = string.Empty;
            View.StrengthRightTeam.text = string.Empty;

            foreach (var place in View.LeftTeam)
            {
                place.OnClick.Subscribe(OnPlaceSelect).AddTo(m_disposables);
                place.OnStartDrag.Subscribe(OnStartDrag).AddTo(m_disposables);
                place.OnDrop.Subscribe(OnDrop).AddTo(m_disposables);
            }

            Resolver.Inject(View.ListCardPanel);

            View.DragableItem.gameObject.SetActive(false);
            m_fightController.OnFightResult.Subscribe(_ => Close()).AddTo(m_disposables);
        }

        private void OnStartDrag(WarriorPlace place)
        {
            if (m_isDragging)
                return;

            if (place.Hero == null)
                return;

            View.DragableItem.DOMove(Input.mousePosition, 0f);
            View.DragableItem.gameObject.SetActive(true);

            if (place.IsEmpty)
                return;

            m_startDragPlace = place;
            m_startDragPlace.SetDragingStatus(true);
            m_isDragging = true;
            View.DragableItemImage.sprite = place.Hero.Avatar;

            m_cancellationTokenSource.TryCancel();
            m_cancellationTokenSource = new();
            DragingItem(m_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid DragingItem(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (Input.GetMouseButton(0))
                {
                    MoveDragableItem();
                }
                else
                {
                    var selectPlace = View.LeftTeam.Find(place => place.MouseInnerFlag);
                    OnDrop(selectPlace);
                }

                await UniTask.Yield(token);
            }
        }

        private void MoveDragableItem()
        {
            m_dragTween.Kill();
            m_dragTween = View.DragableItem
                .DOMove(Input.mousePosition, View.DragSpeed)
                .SetSpeedBased(true);
        }

        private void OnDrop(WarriorPlace place)
        {
            View.DragableItem.gameObject.SetActive(false);
            m_cancellationTokenSource.TryCancel();
            m_isDragging = false;

            if (m_startDragPlace == null)
                return;

            if (place == null)
            {
                m_startDragPlace.SetDragingStatus(false);
                return;
            }

            if (place == m_startDragPlace || place.Hero == m_startDragPlace.Hero)
            {
                m_startDragPlace.SetDragingStatus(false);
                return;
            }

            var oldIndexPlace = View.LeftTeam.FindIndex(warPlace => warPlace == m_startDragPlace);
            var newIndexPlace = View.LeftTeam.FindIndex(warPlace => warPlace == place);
            m_playerTeam.Heroes.Remove(oldIndexPlace);

            if (place.IsEmpty)
            {
                place.SetHero(m_startDragPlace.Hero);
                m_startDragPlace.Clear();
            }
            else
            {
                var tempHero = place.Hero;
                place.SetHero(m_startDragPlace.Hero);
                m_startDragPlace.SetHero(tempHero);
                ChangeTeamData(m_playerTeam, oldIndexPlace, tempHero.HeroData.Id);
            }

            ChangeTeamData(m_playerTeam, newIndexPlace, place.Hero.HeroData.Id);
            m_onChangeTeam.Execute(m_playerTeam);
            m_startDragPlace.SetDragingStatus(false);
        }

        private static void ChangeTeamData(TeamContainer playerTeam, int indexPlace, int id)
        {
            playerTeam.Heroes[indexPlace] = id;
        }

        private void OnPlaceSelect(WarriorPlace place)
        {
            if (m_isDragging)
                return;

            if (place == m_startDragPlace)
                m_startDragPlace = null;

            if (!place.IsEmpty) UnSelectCard(place.Hero);
        }

        private void SelectCard(GameHero hero)
        {
            var result = AddHero(hero);
            if (result) View.ListCardPanel.SelectCards(new List<GameHero> { hero });
        }

        private void UnSelectCard(GameHero hero)
        {
            var result = RemoveHero(hero);
            if (result) View.ListCardPanel.UnselectCards(new List<GameHero> { hero });
        }

        private bool AddHero(GameHero hero)
        {
            var result = false;
            WarriorPlace selectedPlace = null;
            var existHero = View.LeftTeam.Find(place => place.Hero == hero);

            if (existHero != null)
            {
                Debug.LogError("Герой уже есть в команде");
                return false;
            }

            foreach (var place in View.LeftTeam.Where(place => place.IsEmpty))
            {
                place.SetHero(hero);
                UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
                result = true;
                selectedPlace = place;
                break;
            }

            if (!result)
            {
                return false;
            }

            
            if (m_playerTeam != null)
            {
                if (m_playerTeam.Heroes.ContainsKey(selectedPlace.Id))
                {
                    Debug.LogError("Это место в команде уже занято");
                    return false;
                }

                m_playerTeam.Heroes.Add(selectedPlace.Id, hero.HeroData.Id);
                m_onChangeTeam.Execute(m_playerTeam);
            }

            CheckTeam(View.LeftTeam);

            return true;
        }

        private bool RemoveHero(GameHero hero)
        {
            var place = View.LeftTeam.Find(place => place.Hero == hero);

            if (place == null)
            {
                return false;
            }
            
            place.Clear();

            if (m_playerTeam != null)
            {
                m_playerTeam.Heroes.Remove(place.Id);
                m_onChangeTeam.Execute(m_playerTeam);
            }

            CheckTeam(View.LeftTeam);
            UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);

            return true;
        }

        private void ClearPlaces(List<WarriorPlace> places)
        {
            foreach (var place in places.Where(place => !place.IsEmpty))
            {
                RemoveHero(place.Hero);
            }

        }

        private void UpdateStrengthTeam(List<WarriorPlace> team, TMP_Text textComponent)
        {
            float strengthTeam = team.Where(t => t.Hero != null)
                .Aggregate(0f, (current, t)
                    => current + t.Hero.CalculateStrength(m_commonDictionaries));

            textComponent.text = $"{strengthTeam}";
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
            View.StartFightButton.interactable = View.LeftTeam.FindAll(place => !place.IsEmpty).Count > 0;
        }

        //API
        public void OpenTeamComposition(TeamContainer team, Action<TeamContainer> callback)
        {
            View.FastFightToggle.gameObject.SetActive(false);
            CheckTeamContainer(team, View.LeftTeam);
            DisposeMainAction();
            m_playerTeam = team;
            m_callback = callback;
            m_buttonAction = View.StartFightButton.OnClickAsObservable().Subscribe(_ => SaveTeam());
            MessagesPublisher.OpenWindowPublisher.OpenWindow<WarTableController>(openType: OpenType.Exclusive);
            ClearPlaces(View.LeftTeam);
            ClearPlaces(View.RightTeam);
            FillListHeroes(m_heroesStorageController.ListHeroes);

            FillTeam(m_playerTeam, View.LeftTeam);
            UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
            Open();
            CheckTeam(View.LeftTeam);
        }

        private void CheckTeamContainer(TeamContainer team, List<WarriorPlace> teamPlaces)
        {
            var heroesRemove = new List<int>();
            foreach (var heroKeyValuePair in team.Heroes)
            {
                var suitablePlace = teamPlaces.Find(place => place.Id == heroKeyValuePair.Key);
                if (suitablePlace == null)
                {
                    heroesRemove.Add(heroKeyValuePair.Key);
                    continue;
                }

                var suitableHero =
                    m_heroesStorageController.ListHeroes.Find(hero => hero.HeroData.Id == heroKeyValuePair.Value);
                if (suitableHero != null)
                {
                    continue;
                }

                heroesRemove.Add(heroKeyValuePair.Key);
            }

            foreach (var heroId in heroesRemove) team.Heroes.Remove(heroId);
        }

        private void FillTeam(TeamContainer team, List<WarriorPlace> teamPlaces)
        {
            List<GameHero> heroes = new(team.Heroes.Count);

            foreach (var heroKeyValuePair in team.Heroes)
            {
                var suitablePlace = teamPlaces.Find(place => place.Id == heroKeyValuePair.Key);
                if (suitablePlace == null)
                    continue;

                var suitableHero =
                    m_heroesStorageController.ListHeroes.Find(hero => hero.HeroData.Id == heroKeyValuePair.Value);
                if (suitableHero == null) continue;

                suitablePlace.SetHero(suitableHero);
                heroes.Add(suitableHero);
            }

            View.ListCardPanel.SelectCards(heroes);
        }

        private void SaveTeam()
        {
            m_playerTeam.Heroes.Clear();
            foreach (var place in View.LeftTeam)
                if (!place.IsEmpty)
                    m_playerTeam.Heroes.Add(place.Id, place.Hero.HeroData.Id);

            m_callback.Invoke(m_playerTeam);
            Close();
        }

        private void OpenMission(MissionModel mission, List<GameHero> listHeroes)
        {
            DisposeMainAction();
            m_buttonAction = View.StartFightButton.OnClickAsObservable().Subscribe(_ => StartFight().Forget());
            MessagesPublisher.OpenWindowPublisher.OpenWindow<WarTableController>(openType: OpenType.Additive);
            ClearPlaces(View.LeftTeam);
            ClearPlaces(View.RightTeam);

            m_mission = mission;

            for (var i = 0; i < mission.Units.Count; i++)
            {
                var enemyData = mission.Units[i];
                var model = m_commonDictionaries.Heroes[enemyData.HeroId];
                var enemy = new GameHero(model, enemyData);
                View.RightTeam[i].SetHero(enemy);
            }

            UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
            UpdateStrengthTeam(View.RightTeam, View.StrengthRightTeam);

            CheckTeam(View.RightTeam);
            FillListHeroes(listHeroes);
            View.FastFightToggle.gameObject.SetActive(true);
            Open();
        }

        private void DisposeMainAction()
        {
            if (m_buttonAction != null)
            {
                m_buttonAction.Dispose();
                m_buttonAction = null;
            }
        }

        public void OpenMission(MissionModel mission, TeamContainer teamContainer, bool isFastFight = false, WarTableLimiter limiter = null)
        {
            m_canStartBattle = true;
            SetFastFightStatus(isFastFight);
            GetHeroesByLimiter(out var selectedHeroes, limiter);
            OpenMission(mission, selectedHeroes);
            m_playerTeam = teamContainer;

            FillTeam(m_playerTeam, View.LeftTeam);
            UpdateStrengthTeam(View.LeftTeam, View.StrengthLeftTeam);
            CheckTeam(View.LeftTeam);
        }

        private void SetFastFightStatus(bool isFastFight)
        {
            if (isFastFight)
            {
                View.FastFightToggle.On();
            }
            else
            {
                View.FastFightToggle.Off();
            }
        }

        private void GetHeroesByLimiter(out List<GameHero> selectedHeroes, WarTableLimiter limiter)
        {
            if (limiter == null)
            {
                selectedHeroes = m_heroesStorageController.ListHeroes;
                return;
            }

            selectedHeroes = new List<GameHero>();

            foreach (GameHero hero in m_heroesStorageController.ListHeroes)
            {
                if (limiter.CheckHero(hero))
                {
                    selectedHeroes.Add(hero);
                }
            }
        }

        public override void Close()
        {
            m_playerTeam = null;
            m_mission = null;
            OnClose.Execute();
            ClearPlaces(View.LeftTeam);
            ClearPlaces(View.RightTeam);
            base.Close();
        }

        private void FillListHeroes(List<GameHero> listHeroes)
        {
            View.ListCardPanel.ShowCards(listHeroes, m_mission?.HeroRestrictions);
        }

        private async UniTask StartFight()
        {
            if (!m_canStartBattle)
                return;
            
            Debug.Log("war table start fight");
            OnStartMission.Execute();
            if (View.FastFightToggle.IsOn)
            {
                base.Close();
                m_fightController.FastFight(m_mission, View.LeftTeam, View.RightTeam);
                m_playerTeam = null;
                m_mission = null;
                OnClose.Execute();
                ClearPlaces(View.LeftTeam);
                ClearPlaces(View.RightTeam);
            }
            else
            {
                OnPlayerStartFight.Execute();
                await UniTask.Delay(200);
                base.Close();
                m_fightController.SetMission(m_mission, View.LeftTeam, View.RightTeam);
            }

            DisposeMainAction();
        }

        public override void Dispose()
        {
            m_cancellationTokenSource.TryCancel();
            m_dragTween.Kill();
            base.Dispose();
        }
    }
}