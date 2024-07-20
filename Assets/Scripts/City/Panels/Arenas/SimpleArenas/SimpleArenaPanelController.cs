using City.Buildings.Abstractions;
using City.Buildings.Arena;
using Cysharp.Threading.Tasks;
using Fight;
using Misc.Json;
using Models;
using Models.Arenas;
using Network.DataServer;
using Network.DataServer.Messages.Arenas;
using System;
using System.Reflection;
using UiExtensions.Misc;
using UniRx;
using VContainer;

namespace City.Panels.Arenas.SimpleArenas
{
    public class SimpleArenaPanelController : BuildingWithFight<SimpleArenaPanelView>
    {
        [Inject] readonly private IJsonConverter _jsonConverter;

        private DynamicUiList<ArenaOpponentView, ArenaPlayerData> _opponnentPool;
        private ArenaBuildingModel _arenaSave;
        private ReactiveCommand<int> _onCompleteMission = new();
        private ArenaPlayerData _arenaOpponentData;
        private TeamContainer _teamContainer;

        public ReactiveCommand OnTryMission = new();
        public IObservable<int> OnCompleteMission => _onCompleteMission;

        protected override void OnStart()
        {
            _opponnentPool = new(View.OpponentPrefab, View.Content, View.ScrollRect, OnSelectOpponent);
            View.DefendersButton.OnClickAsObservable().Subscribe(_ => OpenWarTableDefenders()).AddTo(Disposables);
            base.OnStart();
        }

        protected override void OnLoadGame()
        {
            _arenaSave = CommonGameData.City.ArenaSave;
            _teamContainer = _arenaSave.MyData.Team;

            if (_teamContainer == null)
            {
                _teamContainer = new TeamContainer(GetType().Name);
                _arenaSave.MyData.Team = _teamContainer;
            }

            UpdateUi();
            base.OnLoadGame();
        }

        private void UpdateUi()
        {
            if (_arenaSave.Opponents.Count > 0)
            {
                _opponnentPool.ShowDatas(_arenaSave.Opponents);
                foreach (var view in _opponnentPool.Views)
                {
                    var data = view.GetData;
                    if(CommonGameData.CommunicationData.PlayersData.ContainsKey(data.PlayerId))
                        view.SetPlayer(CommonGameData.CommunicationData.PlayersData[data.PlayerId]);
                }
            }

            View.PlayerScore.text = $"{_arenaSave.MyData.Score}";
            View.PlayerAvatar.SetData(CommonGameData.PlayerInfoData);
        }

        private void OnSelectOpponent(ArenaOpponentView opponentView)
        {
            _arenaOpponentData = opponentView.GetData;
            OpenMission(_arenaOpponentData.Mission);
        }

        private void OpenWarTableDefenders()
        {
            WarTableController.OpenTeamComposition(_teamContainer, team =>  SetDefenders(team).Forget());
        }

        protected override void OnResultFight(FightResultType result)
        {
            var fightResult = (result == FightResultType.Win) ? 1 : 0;

            SendResultFightData(_arenaOpponentData.Id, fightResult).Forget();

            OnTryMission.Execute();
            if(fightResult > 0)
                _onCompleteMission.Execute(1);
        }

        private async UniTaskVoid SendResultFightData(int opponentId, int resultFight)
        {
            var message = new ArenaFinishFightMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                OpponentId = opponentId,
                Result = resultFight
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var newData = _jsonConverter.Deserialize<ArenaBuildingModel>(result);
                CommonGameData.City.ArenaSave = newData;
                _arenaSave = newData;
                UpdateUi();
            }
        }

        private async UniTaskVoid SetDefenders(TeamContainer heroesIdsContainer)
        {
            var message = new ArenaSetDefendersMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                HeroesIdsContainer = _jsonConverter.Serialize(heroesIdsContainer)
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
            }
        }
    }
}
