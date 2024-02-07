using Cysharp.Threading.Tasks;
using Misc.Json;
using Models;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;

namespace City.Buildings.Guild.NewGuildPanels
{
    public class NewGuildPanelController : UiPanelController<NewGuildPanelView>
    {
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly GuildController _guildController;

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
                var guildData =  _jsonConverter.FromJson<GuildData>(result);
                _guildController.LoadGuild(guildData);
                Close();
            }
        }
    }
}
