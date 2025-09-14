using System;
using System.Collections.Generic;
using System.Linq;
using City.Buildings.Mails.LetterPanels;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using LocalizationSystems;
using Misc.Json;
using Models.Misc;
using Network.DataServer;
using Network.DataServer.Messages.Mails;
using UIController.Rewards;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;

namespace City.Buildings.Mails
{
    public class MailPanelController : UiPanelController<MailPanelView>
    {
        private readonly LetterPanelController _letterPanelController;
        private readonly IJsonConverter _jsonConverter;
        private readonly ClientRewardService _clientRewardService;
        private readonly CommonDictionaries _commonDictionaries;
        private readonly ILocalizationSystem _localizationSystem;

        private DynamicUiList<LetterView, LetterData> _lettersWrapper;

        private List<LetterData> _letterDatas;
        
        public MailPanelController(LetterPanelController letterPanelController,
            IJsonConverter jsonConverter,
            ClientRewardService clientRewardService,
            CommonDictionaries commonDictionaries,
            ILocalizationSystem localizationSystem)
        {
            _letterPanelController = letterPanelController;
            _jsonConverter = jsonConverter;
            _clientRewardService = clientRewardService;
            _commonDictionaries = commonDictionaries;
            _localizationSystem = localizationSystem;
        }

        public override void Start()
        {
            _lettersWrapper = new(View.LetterPrefab, View.Content, View.Scroll, OnSelectLetterView, OnCreateLetterView);
            base.Start();
            View.GetAllButton.OnClickAsObservable().Subscribe(_ => GetAllRewards().Forget()).AddTo(Disposables);
            View.DeleteAllButton.OnClickAsObservable().Subscribe(_ => DeleteAllLetters()).AddTo(Disposables);
            View.AdminFilterButton.interactable = false;
            
            View.AdminFilterButton.OnClickAsObservable().Subscribe(_=> FilterAdminLetter()).AddTo(Disposables);
            View.PlayerFilterButton.OnClickAsObservable().Subscribe(_ => FilterPlayersLetter()).AddTo(Disposables);
        }

        public void OnCreateLetterView(LetterView letterView)
        {
            letterView.SetLocalizationSystem(_localizationSystem);
        }

        private void FilterPlayersLetter()
        {
            View.AdminFilterButton.interactable = true;
            View.PlayerFilterButton.interactable = false;
            var adminLetters = _letterDatas.FindAll(letter => !letter.IsAdmin);
            _lettersWrapper.ShowDatas(adminLetters);
        }

        private void FilterAdminLetter()
        {
            View.AdminFilterButton.interactable = false;
            View.PlayerFilterButton.interactable = true;
            var adminLetters = _letterDatas.FindAll(letter => letter.IsAdmin);
            _lettersWrapper.ShowDatas(adminLetters);
        }

        private void OnSelectLetterView(LetterView letterView)
        {
            _letterPanelController.ShowLetter(letterView);
            CheckNews();
        }

        protected override void OnLoadGame()
        {
            var receiveLetters = CommonGameData.CommunicationData.LetterDatas
                .FindAll(letter => letter.ReceiverPlayerId == CommonGameData.PlayerInfoData.Id);
            
            _letterDatas = receiveLetters;

            CheckNews();
            FilterAdminLetter();
        }

        private void CheckNews()
        { 
            OnNewsStatusChangeInternal.Execute(_letterDatas.Any(letter => !letter.IsOpened));
        }

        private async UniTask GetAllRewards()
        {
            RewardModel rewardModel = new RewardModel();
            rewardModel.CommonDictionaries = _commonDictionaries;

            bool showReward = false;
            foreach (var letter in _letterDatas)
            {
                if (letter.IsRewardReceived)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(letter.RewardJSON))
                {
                    continue;
                }

                var message = new GetRewardFromLetterMessage { PlayerId = CommonGameData.PlayerInfoData.Id,
                    LetterId = letter.Id };
                var result = await DataServer.PostData(message);

                if (string.IsNullOrEmpty(result))
                {
                    continue;
                }

                try
                {
                    var reward = _jsonConverter.Deserialize<RewardModel>(result);

                    rewardModel.Resources.AddRange(reward.Resources);
                    rewardModel.Items.AddRange(reward.Items);
                    rewardModel.Splinters.AddRange(reward.Splinters);

                    letter.IsRewardReceived = true;
                    showReward = true;
                }
                catch
                {
                }
            }

            if (!showReward)
            {
                return;
            }

            var gameReward = new GameReward(rewardModel, _commonDictionaries);
            _clientRewardService.ShowReward(gameReward);
        }

        private void DeleteAllLetters()
        {
        }
    }
}