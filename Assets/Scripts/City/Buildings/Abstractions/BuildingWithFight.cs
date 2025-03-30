using City.Panels.Arenas.Teams;
using Db.CommonDictionaries;
using Fight;
using Fight.WarTable;
using Models.Arenas;
using Models.Fights.Campaign;
using System;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Buildings.Abstractions
{
    public abstract class BuildingWithFight<T> : BaseBuilding<T> where T : BaseBuildingView, IDisposable
    {
        [Inject] protected readonly FightController FightController;
        [Inject] protected readonly WarTableController WarTableController;
        [Inject] protected readonly CommonDictionaries CommonDictionaries;

        protected ReactiveCommand<int> _onTryFight = new ReactiveCommand<int>();
        protected ReactiveCommand<int> _onWinFight = new ReactiveCommand<int>();
        private CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _tempDisposable;
        private IDisposable _teamChangeDisposable;

        protected WarTableLimiter WarTableLimiter = null;
        protected TeamContainer TeamContainer;

        public IObservable<int> OnTryFight => _onTryFight;
        public IObservable<int> OnWinFight => _onWinFight;

        protected override void OnStart()
        {
            TeamContainer = TeamUtils.LoadTeam(GetType().Name);
            base.OnStart();
        }

        public void OpenMission(MissionModel mission)
        {
            WarTableController.OpenMission(mission, TeamContainer, WarTableLimiter);
            _teamChangeDisposable = WarTableController.OnChangeTeam.Subscribe(OnChangeTeam);
            _tempDisposable = WarTableController.OnStartMission.Subscribe(_ => OnStartMission());
            WarTableController.OnClose.Subscribe(_ => OnCloseWarTable()).AddTo(_disposables);
        }

        private void OnCloseWarTable()
        {
            _teamChangeDisposable?.Dispose();
        }

        private void OnChangeTeam(TeamContainer container)
        {
            if (!TeamContainer.Id.Equals(container.Id))
            {
                Debug.LogError("Try change not my team!");
            }

            TeamUtils.SaveTeam(TeamContainer);
        }

        protected virtual void OnStartMission()
        {
            _tempDisposable.Dispose();
            _tempDisposable = FightController.OnFigthResult.Subscribe(OnResultFight);
        }

        public void OnAfterFight(bool isOpen)
        {
            _onTryFight.Execute(1);
            if (isOpen == false)
            {
                Open();
                UnregisterFight();
            }
            else
            {
                Close();
            }
        }

        protected virtual void OnResultFight(FightResultType result)
        {
            _tempDisposable?.Dispose();
            if (result == FightResultType.Win)
            {
                _onWinFight.Execute(1);
            }
        }

        private void UnregisterFight()
        {
            _teamChangeDisposable?.Dispose();
            _disposables.Dispose();
        }
    }
}