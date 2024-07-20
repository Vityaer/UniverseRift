using Cysharp.Threading.Tasks;
using Misc.Json;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models.Guilds;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Services;

namespace City.Buildings.Guild.NewGuildPanels
{
    public class NewGuildPanelController : UiPanelController<NewGuildPanelView>
    {
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly GuildController _guildController;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;

        public ReactiveCommand OnCreateGuild = new();

        public override void Start()
        {
            View.CreateNewGuildButton.OnClickAsObservable().Subscribe(_ => CreateNewGuild().Forget()).AddTo(Disposables);
            base.Start();
        }

        private async UniTaskVoid CreateNewGuild()
        {
            var message = new CreateNewGuildMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildName = View.NameNewGuildInputField.text,
                IconPath = string.Empty
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var guildPlayerSaveContainer = _jsonConverter.Deserialize<GuildPlayerSaveContainer>(result);
                CommonGameData.PlayerInfoData.GuildId = guildPlayerSaveContainer.GuildData.Id;
                CommonGameData.City.GuildPlayerSaveContainer = guildPlayerSaveContainer;
                OnCreateGuild.Execute();
                _messagesPublisher.MessageCloseWindowPublisher.CloseWindow<NewGuildPanelController>();

                _guildController.CreateGuild(guildPlayerSaveContainer);

                _guildController.Open();
            }
        }
    }
}
