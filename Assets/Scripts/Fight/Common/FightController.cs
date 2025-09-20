using System;
using System.Collections.Generic;
using Assets.Scripts.Fight.Common;
using City.TrainCamp.HeroInstances;
using Cysharp.Threading.Tasks;
using Fight.Common.Comparers;
using Fight.Common.Factories;
using Fight.Common.FightInterface;
using Fight.Common.Grid;
using Fight.Common.HeroControllers.Generals;
using Fight.Common.Misc;
using Fight.Common.UI;
using Fight.Common.WarTable;
using LocalizationSystems;
using Models.Fights.Campaign;
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

namespace Fight.Common
{
    public partial class FightController : UiController<FightView>
    {
        private const string START_FIGHT_LOCALIZATION_KEY = "StartFightLabel";
        private const string FINISH_FIGHT_LOCALIZATION_KEY = "FinishFightLabel";
        private const string START_TIME_LOCALIZATION_KEY = "CurrentTimeStartFightLabel";
        private const string ROUND_LOCALIZATION_KEY = "CurrentRoundLabel";
        private const int MAX_ROUND_COUNT = 15;
        private const int TICK_DELAY = 750;

        [Inject] private readonly HeroFactory m_heroFactory;
        [Inject] private readonly GridController m_gridController;
        [Inject] private readonly IUiMessagesPublisherService m_messagesPublisher;
        [Inject] private readonly FightDirectionController m_fightDirectionController;
        [Inject] private readonly HeroInstancesController m_heroInstancesController;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        private FightPanelController m_fightPanelController;

        public List<Warrior> m_leftTeam = new();
        public List<Warrior> m_rightTeam = new();
        private readonly List<HeroController> m_listInitiative = new();
        private readonly List<HeroController> m_listHeroesWithAction = new();

        public ReactiveCommand<LocalizedString> OnChangeFightUiText = new();
        public ReactiveCommand OnEndRound = new();
        public ReactiveCommand OnStartFight = new();
        public ReactiveCommand OnFinishFight = new();
        public ReactiveCommand OnPlayerFinishFight = new();
        public ReactiveCommand AfterCreateFight = new();

        private bool m_isFightFinish = false;
        private int m_currentHeroIndex = -1;
        private int m_round = 1;
        private readonly ReactiveCommand<FightResultType> m_onFightResult = new();

        public List<Warrior> GetLeftTeam => m_leftTeam;
        public List<Warrior> GetRightTeam => m_rightTeam;
        public IObservable<FightResultType> OnFightResult => m_onFightResult;
        public bool IsFastFight { get; private set; }

        public HeroController GetCurrentHero() => m_listInitiative[m_currentHeroIndex];

        public void SetControllerUi(FightPanelController fightPanelController)
        {
            m_fightPanelController = fightPanelController;
        }

        public void SetMission(MissionModel mission, List<WarriorPlace> leftWarriorPlace,
            List<WarriorPlace> rightWarriorPlace)
        {
            IsFastFight = false;
            View.gameObject.SetActive(true);

            m_messagesPublisher.OpenWindowPublisher.OpenWindow<FightWindow>(openType: OpenType.Exclusive);
            m_isFightFinish = false;
            //View.LocationController.OpenLocation(mission.Location);
            m_heroInstancesController.HideLight();
            m_gridController.OpenGrid();
            CreateTeams(leftWarriorPlace, rightWarriorPlace);
        }

        private void CreateTeams(List<WarriorPlace> leftWarriorPlace, List<WarriorPlace> rightWarriorPlace)
        {
            CreateTeam(m_gridController.GetLeftTeamPos, leftWarriorPlace, Side.Left, m_leftTeam);
            CreateTeam(m_gridController.GetRightTeamPos, rightWarriorPlace, Side.Right, m_rightTeam);
            WaitDelayBeforeStartFight().Forget();
        }

        private async UniTaskVoid WaitDelayBeforeStartFight()
        {
            if (!IsFastFight)
                await UniTask.Delay(1000);

            AfterCreateFight.Execute();

            if (!IsFastFight)
                for (var i = 3; i > 0; i--)
                {
                    var localizeTime = m_localizationSystem.LocalizationUiContainer
                        .GetLocalizedContainer(START_TIME_LOCALIZATION_KEY)
                        .WithArguments(new List<object> { i });

                    OnChangeFightUiText.Execute(localizeTime);
                    await UniTask.Delay(TICK_DELAY);
                }

            var localize =
                m_localizationSystem.LocalizationUiContainer.GetLocalizedContainer(START_FIGHT_LOCALIZATION_KEY);
            OnChangeFightUiText.Execute(localize);

            if (!IsFastFight)
                await UniTask.Delay(500);

            ShowRoundInfo();
            OnStartFight.Execute();
            m_listInitiative.Sort(new HeroInitiativeComparer());
            StartFight();
        }

        private void ShowRoundInfo()
        {
            var localize = m_localizationSystem.LocalizationUiContainer.GetLocalizedContainer(ROUND_LOCALIZATION_KEY)
                .WithArguments(new List<object> { m_round });
            OnChangeFightUiText.Execute(localize);
        }

        private void CreateTeam(List<HexagonCell> teamPos, List<WarriorPlace> team, Side side,
            List<Warrior> warriorTeam)
        {
            for (var i = 0; i < team.Count; i++)
                if (team[i].Hero != null)
                {
                    var hero = m_heroFactory.Create(team[i].Hero, teamPos[i], side, m_gridController.RootTemplateObjects);

                    if (hero == null) continue;
                    warriorTeam.Add(new Warrior(hero));
                    hero.SetData(team[i].Hero, teamPos[i], side, IsFastFight);
                    m_listInitiative.Add(hero);
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
            m_listInitiative.Remove(hero);
            m_currentHeroIndex -= 1;
            m_listInitiative.Add(hero);
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
            m_listHeroesWithAction.Add(newHero);
        }

        public void RemoveHeroWithAction(HeroController removeHero)
        {
            m_listHeroesWithAction.Remove(removeHero);
            if (m_listHeroesWithAction.Count == 0)
                NextHero();
        }

        public void RemoveHeroWithActionAll(HeroController removeHero)
        {
            m_listHeroesWithAction.RemoveAll(x => x == removeHero);

            if (m_listHeroesWithAction.Count == 0)
                NextHero();
        }

        private void NextHero()
        {
            if (m_isFightFinish)
                return;

            if ((m_currentHeroIndex + 1) < m_listInitiative.Count)
                m_currentHeroIndex++;
            else
                NewRound();

            if (!m_isFightFinish && m_listInitiative.Count > 0)
            {
                m_fightPanelController.SetHeroStatus(m_listInitiative[m_currentHeroIndex]);
                m_listInitiative[m_currentHeroIndex].DoTurn();
            }
        }

        private void NewRound()
        {
            if (m_isFightFinish)
                return;

            UpdateListInitiative();

            foreach (var hero in m_listInitiative)
                hero.Refresh();

            m_currentHeroIndex = 0;
            m_round++;

            ShowRoundInfo();

            OnEndRound.Execute();
            m_fightDirectionController.ClearData();
            if (m_round == MAX_ROUND_COUNT) Win(Side.Right);
        }

        private void UpdateListInitiative()
        {
            m_listInitiative.Clear();

            for (var i = 0; i < m_leftTeam.Count; i++)
                if (m_leftTeam[i].heroController != null)
                    m_listInitiative.Add(m_leftTeam[i].heroController);
                else
                    Debug.Log("left team hero null");

            for (var i = 0; i < m_rightTeam.Count; i++)
                if (m_rightTeam[i].heroController != null)
                    m_listInitiative.Add(m_rightTeam[i].heroController);
                else
                    Debug.Log("Right team hero null");

            m_listInitiative.Sort(new HeroInitiativeComparer());
        }

        //Result fight
        private void CheckFinishFight()
        {
            if ((m_leftTeam.Count == 0) || (m_rightTeam.Count == 0))
                Win(m_leftTeam.Count > 0 ? Side.Left : Side.Right);
        }

        private void Win(Side side)
        {
            if (m_isFightFinish) return;

            m_isFightFinish = true;
            m_fightDirectionController.CloseControllers();
            FinishFightCountdown(side).Forget();
        }

        private async UniTaskVoid FinishFightCountdown(Side side)
        {
            var localize =
                m_localizationSystem.LocalizationUiContainer.GetLocalizedContainer(FINISH_FIGHT_LOCALIZATION_KEY);
            OnChangeFightUiText.Execute(localize);

            if (side == Side.Right) CheckSaveResult();
            if (!IsFastFight)
                await UniTask.Delay(500);

            OnFinishFight.Execute();
            m_messagesPublisher.MessageCloseWindowPublisher.CloseWindow<FightWindow>();

            if (!IsFastFight)
            {
                OnPlayerFinishFight.Execute();
                await UniTask.Delay(2000);
            }

            var fightResult = (side == Side.Left) ? FightResultType.Win : FightResultType.Defeat;
            ClearAll();

            m_onFightResult.Execute(fightResult);
        }

        private void ClearAll()
        {
            if (!IsFastFight) m_gridController.FinishFight();

            m_heroInstancesController.OpenLight();
            DeleteTeam(m_rightTeam);
            DeleteTeam(m_leftTeam);
            m_listInitiative.Clear();
            m_listHeroesWithAction.Clear();
            m_currentHeroIndex = -1;
            m_round = 1;
        }

        private void DeleteTeam(List<Warrior> team)
        {
            for (var i = 0; i < team.Count; i++) team[i].heroController.DeleteHero();
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
            m_listInitiative[m_currentHeroIndex].MessageDamageAfterStrike(damage);
        }

        public void DeleteHero(HeroController heroForDelete)
        {
            RemoveHeroWithActionAll(heroForDelete);
            var warrior = m_leftTeam.Find(x => x.heroController == heroForDelete);
            if (warrior != null)
            {
                m_leftTeam.Remove(warrior);
            }
            else
            {
                warrior = m_rightTeam.Find(x => x.heroController == heroForDelete);
                m_rightTeam.Remove(warrior);
            }

            m_listInitiative.Remove(heroForDelete);
            CheckFinishFight();
        }


        private void KillRightTeam()
        {
            DeleteTeam(m_rightTeam);
            CheckFinishFight();
        }

        public void FastFight(MissionModel mission, List<WarriorPlace> leftWarriorPlace,
            List<WarriorPlace> rightWarriorPlace)
        {
            IsFastFight = true;
            View.gameObject.SetActive(false);
            m_isFightFinish = false;

            m_heroInstancesController.HideLight();
            CreateTeam(m_gridController.GetLeftTeamPos, leftWarriorPlace, Side.Left, m_leftTeam);
            CreateTeam(m_gridController.GetRightTeamPos, rightWarriorPlace, Side.Right, m_rightTeam);
            AfterCreateFight.Execute();
            OnStartFight.Execute();
            m_listInitiative.Sort(new HeroInitiativeComparer());
            StartFight();
        }
    }
}