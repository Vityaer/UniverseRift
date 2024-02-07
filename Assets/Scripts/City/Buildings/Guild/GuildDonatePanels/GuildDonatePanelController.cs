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

namespace City.Buildings.Guild.GuildDonatePanels
{
    public class GuildDonatePanelController : UiPanelController<GuildDonatePanelView>
    {
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly ResourceStorageController _resourceStorageController;

        private GameResource _donate = new GameResource(ResourceType.Gold, 100, 3);
        private GameResource _bigDonate = new GameResource(ResourceType.Gold, 1, 6);

        public override void Start()
        {
            View.DonateButton.OnClickAsObservable().Subscribe(_ => Donate(_donate).Forget()).AddTo(Disposables);
            View.BigDonateButton.OnClickAsObservable().Subscribe(_ => Donate(_bigDonate).Forget()).AddTo(Disposables);
            base.Start();
        }

        private async UniTaskVoid Donate(GameResource donate)
        {
            var message = new GuildDonateForEvolveMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildId = CommonGameData.PlayerInfoData.GuildId,
                Donate = _jsonConverter.ToJson(donate.Amount)
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                _resourceStorageController.SubtractResource(donate);
            }
        }
    }
}
