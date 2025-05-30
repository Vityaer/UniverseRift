using System;
using City.Panels.Arenas.Teams;
using ClientServices;
using Common.Rewards;
using Db.CommonDictionaries;
using Fight;
using Fight.WarTable;
using Models.Arenas;
using Models.Fights.Campaign;
using Ui.Misc.Widgets;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;

namespace UiExtensions.Panels
{
    public class PanelWithFight<T> : UiPanelController<T>
        where T : BasePanel
    {
        [Inject] protected readonly FightController FightController;
        [Inject] protected readonly WarTableController WarTableController;
        [Inject] protected readonly CommonDictionaries CommonDictionaries;
        [Inject] protected readonly ClientRewardService ClientRewardService;
        
        protected ReactiveCommand<int> _onTryFight = new ReactiveCommand<int>();
        protected ReactiveCommand<int> _onWinFight = new ReactiveCommand<int>();
        private IDisposable _closeDisposable;
        private IDisposable _tempDisposable;
        private IDisposable _fightResultDisposable;
        private IDisposable _teamChangeDisposable;

        private string FastFightStatusArgName => $"{this.GetType().Name}.FastFightStatus";
        protected WarTableLimiter WarTableLimiter = null;
        protected TeamContainer TeamContainer;

        public IObservable<int> OnTryFight => _onTryFight;
        public IObservable<int> OnWinFight => _onWinFight;

        public override void Start()
        {
            TeamContainer = TeamUtils.LoadTeam(GetType().Name);
            base.Start();
        }

        public void OpenMission(MissionModel mission)
        {
            bool isFastFight = false;
            TeamContainer = TeamUtils.LoadTeam(GetType().Name);
            if (PlayerPrefs.HasKey(FastFightStatusArgName))
            {
                isFastFight = bool.Parse(PlayerPrefs.GetString(FastFightStatusArgName));
            }

            DisposeAll();
            
            WarTableController.OpenMission(mission, TeamContainer, isFastFight, limiter: WarTableLimiter);
            _teamChangeDisposable = WarTableController.OnChangeTeam.Subscribe(OnChangeTeam);
            _tempDisposable = WarTableController.OnStartMission.Subscribe(_ => OnStartMission());
            _closeDisposable = WarTableController.OnClose.Subscribe(_ => OnCloseWarTable());
        }

        private void DisposeAll()
        {
            _fightResultDisposable?.Dispose();
            _teamChangeDisposable?.Dispose();
            _tempDisposable?.Dispose();
            _closeDisposable?.Dispose();

            _fightResultDisposable = null;
            _teamChangeDisposable = null;
            _tempDisposable = null;
            _closeDisposable = null;
        }

        protected virtual void OnCloseWarTable()
        {
            PlayerPrefs.SetString(FastFightStatusArgName, $"{WarTableController.IsFastFight}");
            _teamChangeDisposable?.Dispose();
            _teamChangeDisposable = null;
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
            _teamChangeDisposable?.Dispose();
            _teamChangeDisposable = null;
            PlayerPrefs.SetString(FastFightStatusArgName, $"{WarTableController.IsFastFight}");
            _fightResultDisposable?.Dispose();
            _fightResultDisposable = FightController.OnFightResult.Subscribe(OnResultFight);
        }

        protected virtual void OnResultFight(FightResultType result)
        {
            _onTryFight.Execute(1);

            _tempDisposable?.Dispose();
            _fightResultDisposable?.Dispose();

            _tempDisposable = null;
            _fightResultDisposable = null;
            
            if (result == FightResultType.Win)
            {
                _onWinFight.Execute(1);
            }
            else
            {
                OnDefeatFight();
            }

            UnregisterFight();
        }

        protected virtual void OnDefeatFight()
        {
            ClientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
        }

        private void UnregisterFight()
        {
            DisposeAll();
        }
    }
}