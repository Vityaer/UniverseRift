using Cysharp.Threading.Tasks;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models;
using Network.DataServer;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using Models.Common.BigDigits;
using Misc.Json;
using VContainer;
using Common.Resourses;
using ClientServices;
using System;
using Db.CommonDictionaries;
using UnityEngine;

namespace City.Buildings.Guild.GuildDonatePanels
{
    public class GuildDonatePanelController : UiPanelController<GuildDonatePanelView>
    {
        private const string DONATE_EVOLUTION_COST_CONTAINER_KEY = "GuildDonateEvolution";

        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private GameResource _donate = new GameResource(ResourceType.Gold, 100, 3);
        private GameResource _bigDonate = new GameResource(ResourceType.Gold, 1, 6);
        private GuildData _guildData;

        public override void Start()
        {
            View.DonateButton.OnClickAsObservable().Subscribe(_ => Donate(_donate).Forget()).AddTo(Disposables);
            View.BigDonateButton.OnClickAsObservable().Subscribe(_ => Donate(_bigDonate).Forget()).AddTo(Disposables);
            base.Start();
        }

        protected override void OnLoadGame()
        {
            _guildData = CommonGameData.City.GuildPlayerSaveContainer.GuildData;
            UpdateUi();
            base.OnLoadGame();
        }

        public override void OnShow()
        {
            _guildData = CommonGameData.City.GuildPlayerSaveContainer.GuildData;
            UpdateUi();
            base.OnShow();
        }

        private void UpdateUi()
        {
            if (_guildData == null)
                return;

            View.GuildMineLevel.text = $"{_guildData.DonateEvoleLevel}";
            var currentDonatLevel = new BigDigit(_guildData.StorageMantissa, _guildData.StorageE10);
            var targetDonate = _commonDictionaries.CostContainers[DONATE_EVOLUTION_COST_CONTAINER_KEY]
                .GetCostForLevelUp(_guildData.DonateEvoleLevel);

            var delta = currentDonatLevel / targetDonate[0].Amount;
            var currentProgress = delta.Mantissa * Mathf.Pow(10, delta.E10);
            currentProgress = Mathf.Clamp01(currentProgress);

            View.DonateCurrentProgress.text = $"{currentDonatLevel} / {targetDonate[0].Amount}"; 
            View.DonateSlider.value = currentProgress;
        }

        private async UniTaskVoid Donate(GameResource donate)
        {
            var message = new GuildDonateForEvolveMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildId = CommonGameData.PlayerInfoData.GuildId,
                Donate = _jsonConverter.Serialize(donate.Amount)
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                _resourceStorageController.SubtractResource(donate);
                var newGuildData = _jsonConverter.Deserialize<GuildData>(result);
                if (newGuildData != null)
                {
                    _guildData = newGuildData;
                    UpdateUi();
                }
            }
        }
    }
}
