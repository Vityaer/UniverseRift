using System;
using City.Buildings.Abstractions;
using City.Buildings.Arena;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Fight.Common;
using Misc.Json;
using Models;
using Models.Arenas;
using Models.City.Arena;
using Network.DataServer;
using Network.DataServer.Messages.Arenas;
using UiExtensions.Misc;
using UniRx;
using Utils;
using VContainer;

namespace City.Panels.Arenas.SimpleArenas
{
    public class SimpleArenaPanelController : BuildingWithFight<SimpleArenaPanelView>
    {
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;

        private DynamicUiList<ArenaOpponentView, ArenaPlayerData> m_opponnentPool;
        private ArenaData m_arenaSave;
        private readonly ReactiveCommand<int> m_onCompleteMission = new();
        private ArenaPlayerData m_arenaOpponentData;
        private TeamContainer m_teamContainer;

        public ReactiveCommand OnTryMission = new();
        public IObservable<int> OnCompleteMission => m_onCompleteMission;

        protected override void OnStart()
        {
            m_opponnentPool = new(View.OpponentPrefab, View.Content,
                View.ScrollRect,
                m_commonDictionaries,
                OnSelectOpponent,
                OnCreateOpponentPanel);
            View.DefendersButton.OnClickAsObservable().Subscribe(_ => OpenWarTableDefenders()).AddTo(Disposables);
            base.OnStart();
        }

        private void OnCreateOpponentPanel(ArenaOpponentView view)
        {
            Resolver.Inject(view);
        }

        protected override void OnLoadGame()
        {
            m_arenaSave = CommonGameData.City.ArenaSave;
            m_teamContainer = m_arenaSave.MyData.Team;

            if (m_teamContainer == null)
            {
                m_teamContainer = new TeamContainer(GetType().Name);
                m_arenaSave.MyData.Team = m_teamContainer;
            }

            ArenaBuildingModel arenaBuildingModel =
                CommonDictionaries.Buildings[nameof(ArenaBuildingModel)] as ArenaBuildingModel;

            int workHours = arenaBuildingModel.ArenaContainers[ArenaType.Simple].WorkHours;
            DateTime startDateTime = TimeUtils.ParseTime(m_arenaSave.ArenaGeneralData.SimpleArenaDateTimeStartText);

            View.LeftTime.SetData(startDateTime, TimeSpan.FromHours(workHours));

            UpdateUi();
            base.OnLoadGame();
        }

        private void UpdateUi()
        {
            if (m_arenaSave.Opponents.Count > 0)
                m_opponnentPool.ShowDatas(m_arenaSave.Opponents);

            View.PlayerScore.text = $"{m_arenaSave.MyData.Score}";
            View.PlayerAvatar.SetData(CommonGameData.PlayerInfoData);
        }

        private void OnSelectOpponent(ArenaOpponentView opponentView)
        {
            m_arenaOpponentData = opponentView.GetData;
            OpenMission(m_arenaOpponentData.Mission);
        }

        private void OpenWarTableDefenders()
        {
            WarTableController.OpenTeamComposition(m_teamContainer, team => SetDefenders(team).Forget());
        }

        protected override void OnResultFight(FightResultType result)
        {
            int fightResult = (result == FightResultType.Win) ? 1 : 0;

            SendResultFightData(m_arenaOpponentData.Id, fightResult).Forget();

            OnTryMission.Execute();
            if (fightResult > 0)
                m_onCompleteMission.Execute(1);
        }

        private async UniTaskVoid SendResultFightData(int opponentId, int resultFight)
        {
            ArenaFinishFightMessage message = new ArenaFinishFightMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                OpponentId = opponentId,
                Result = resultFight
            };

            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                ArenaData newData = m_jsonConverter.Deserialize<ArenaData>(result);
                CommonGameData.City.ArenaSave = newData;
                m_arenaSave = newData;
                CommonGameData.CommunicationData.AddPlayers(m_arenaSave.PlayersData);
                UpdateUi();
            }
        }

        private async UniTaskVoid SetDefenders(TeamContainer heroesIdsContainer)
        {
            ArenaSetDefendersMessage message = new ArenaSetDefendersMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                HeroesIdsContainer = m_jsonConverter.Serialize(heroesIdsContainer)
            };

            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
            }
        }
    }
}