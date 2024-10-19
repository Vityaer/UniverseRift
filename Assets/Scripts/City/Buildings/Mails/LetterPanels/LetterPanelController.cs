using Buildings.Mails.LetterPanels;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Misc.Json;
using Models.Misc;
using Network.DataServer;
using Network.DataServer.Messages.Mails;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.Mails.LetterPanels
{
    public class LetterPanelController : UiPanelController<LetterPanelView>
    {
        private readonly IJsonConverter _jsonConverter;
        private readonly ClientRewardService _clientRewardService;
        private readonly CommonDictionaries _commonDictionaries;

        private LetterData _letterData;

        public LetterPanelController(
            IJsonConverter jsonConverter,
            ClientRewardService clientRewardService,
            CommonDictionaries commonDictionaries)
        {
            _jsonConverter = jsonConverter;
            _clientRewardService = clientRewardService;
            _commonDictionaries = commonDictionaries;
        }

        public override void Start()
        {
            View.GetRewardButton.OnClickAsObservable().Subscribe(_ => GetReward().Forget()).AddTo(Disposables);
            base.Start();
        }

        public void ShowLetter(LetterView letterView)
        {
            _letterData = letterView.GetData;
            UpdateUi();
            MessagesPublisher.OpenWindowPublisher.OpenWindow<LetterPanelController>(openType: OpenType.Exclusive);
        }

        private void UpdateUi()
        {
            View.LetterTopic.text = _letterData.Topic;
            View.MainText.text = _letterData.Message;
            View.GetRewardButton.gameObject.SetActive(_letterData.RewardId >= 0);
        }

        private async UniTaskVoid GetReward()
        {
            View.GetRewardButton.interactable = false;
            var message = new GetRewardFromLetterMessage { PlayerId = CommonGameData.PlayerInfoData.Id, LetterId = _letterData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var rewardModel = _jsonConverter.Deserialize<RewardModel>(result);
                var gameReward = new GameReward(rewardModel, _commonDictionaries);
                _clientRewardService.ShowReward(gameReward);
            }

            View.GetRewardButton.interactable = true;
        }

    }
}
