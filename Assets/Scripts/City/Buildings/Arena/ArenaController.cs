using System;
using Assets.Scripts.City.Panels.Arenas;
using City.Buildings.Abstractions;
using City.Panels.Arenas;
using City.Panels.Arenas.SimpleArenas;
using Fight;
using Models;
using Models.Arenas;
using Models.City.Arena;
using Models.Common;
using Services.TimeLocalizeServices;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Model;
using VContainerUi.Messages;

namespace City.Buildings.Arena
{
    public class ArenaController : BuildingWithFight<ArenaView>, IInitializable
    {
        [Inject] private readonly CommonGameData _сommonGameData;
        [Inject] private readonly TimeLocalizeService m_timeLocalizeService;

        private ArenaData _arenaBuildingSave;

        public void Initialize()
        {
            View.SimpleArenaButton.OnClickAsObservable().Subscribe(_ => OpenSimpleAreana()).AddTo(Disposables);
            View.RatingArenaButton.OnClickAsObservable().Subscribe(_ => OpenRatingAreana()).AddTo(Disposables);
            View.TournamentButton.OnClickAsObservable().Subscribe(_ => OpenTournament()).AddTo(Disposables);
        }

        private void OpenSimpleAreana()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<SimpleArenaPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenRatingAreana()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<RatingArenaPanelController>(openType: OpenType.Exclusive);

        }

        private void OpenTournament()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<TournamentPanelController>(openType: OpenType.Exclusive);
        }

        protected override void OnLoadGame()
        {
            _arenaBuildingSave = _сommonGameData.City.ArenaSave;
            var arenaBuildingModel = CommonDictionaries.Buildings[nameof(ArenaBuildingModel)] as ArenaBuildingModel;

            var workHours = arenaBuildingModel.ArenaContainers[ArenaType.Simple].WorkHours;
            Debug.Log(_arenaBuildingSave.ArenaGeneralData.SimpleArenaDateTimeStartText);
            var startDateTime = TimeUtils.ParseTime(_arenaBuildingSave.ArenaGeneralData.SimpleArenaDateTimeStartText);
            
            View.SimpleArenaTimer.SetData(startDateTime, TimeSpan.FromHours(workHours));
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                _onWinFight.Execute(1);
                TextUtils.Save(_сommonGameData);
            }
            _onTryFight.Execute(1);
        }
    }
}