using Fight;
using Fight.WarTable;
using Models.Common.BigDigits;
using Models.Fights.Campaign;
using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainerUi.Interfaces;

namespace City.Buildings.Abstractions
{
    public abstract class BuildingWithFight<T> : BaseBuilding<T> where T : BaseBuildingView, IDisposable
    {
        [Inject] protected readonly FightController FightController;
        [Inject] protected readonly WarTableController WarTableController;

        protected ReactiveCommand<int> _onTryFight = new ReactiveCommand<int>();
        protected ReactiveCommand<int> _onWinFight = new ReactiveCommand<int>();
        private CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _disposable;

        public IObservable<int> OnTryFight => _onTryFight;
        public IObservable<int> OnWinFight => _onWinFight;

        public void OpenMission(MissionModel mission)
        {
            WarTableController.OpenMission(mission);
            _disposable = WarTableController.OnStartMission.Subscribe(_ => OnStartMission());
        }

        private void OnStartMission()
        {
            _disposable.Dispose();
            _disposable = FightController.OnFigthResult.Subscribe(OnResultFight);
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
            _disposable?.Dispose();
            if (result == FightResultType.Win)
            {
                _onWinFight.Execute(1);
            }
        }

        private void UnregisterFight()
        {
            _disposables.Dispose();
        }
    }
}