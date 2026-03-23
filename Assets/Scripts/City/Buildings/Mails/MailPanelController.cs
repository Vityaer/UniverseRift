using System.Collections.Generic;
using System.Linq;
using City.Buildings.Mails.LetterPanels;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using LocalizationSystems;
using Misc.Json;
using Models.Misc;
using Network.DataServer;
using Network.DataServer.Messages.Mails;
using UIController.Rewards;
using UiExtensions.Misc;
using UiExtensions.Panels;
using UniRx;
using Utils.DataCheckUtils;

namespace City.Buildings.Mails
{
    public class MailPanelController : UiPanelController<MailPanelView>
    {
        private readonly LetterPanelController m_letterPanelController;
        private readonly IJsonConverter m_jsonConverter;
        private readonly ClientRewardService m_clientRewardService;
        private readonly CommonDictionaries m_commonDictionaries;
        private readonly ILocalizationSystem m_localizationSystem;

        private DynamicUiList<LetterView, LetterData> m_lettersWrapper;

        private List<LetterData> m_letterDatas;

        public MailPanelController(LetterPanelController letterPanelController,
            IJsonConverter jsonConverter,
            ClientRewardService clientRewardService,
            CommonDictionaries commonDictionaries,
            ILocalizationSystem localizationSystem)
        {
            m_letterPanelController = letterPanelController.ThrowIfArgumentNull();
            m_jsonConverter = jsonConverter.ThrowIfArgumentNull();
            m_clientRewardService = clientRewardService.ThrowIfArgumentNull();
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_localizationSystem = localizationSystem.ThrowIfArgumentNull();
        }

        public override void Start()
        {
            m_lettersWrapper = new(View.LetterPrefab, View.Content, View.Scroll, m_commonDictionaries,
                OnSelectLetterView, OnCreateLetterView);
            base.Start();
            View.GetAllButton.OnClickAsObservable().Subscribe(_ => GetAllRewards().Forget()).AddTo(Disposables);
            View.DeleteAllButton.OnClickAsObservable().Subscribe(_ => DeleteAllLetters()).AddTo(Disposables);
            View.AdminFilterButton.interactable = false;

            View.AdminFilterButton.OnClickAsObservable().Subscribe(_ => FilterAdminLetter()).AddTo(Disposables);
            View.PlayerFilterButton.OnClickAsObservable().Subscribe(_ => FilterPlayersLetter()).AddTo(Disposables);
        }

        private void OnCreateLetterView(LetterView letterView)
        {
            letterView.SetLocalizationSystem(m_localizationSystem);
        }

        private void FilterPlayersLetter()
        {
            View.AdminFilterButton.interactable = true;
            View.PlayerFilterButton.interactable = false;
            List<LetterData> adminLetters = m_letterDatas.FindAll(letter => !letter.IsAdmin);
            m_lettersWrapper.ShowDatas(adminLetters);
        }

        private void FilterAdminLetter()
        {
            View.AdminFilterButton.interactable = false;
            View.PlayerFilterButton.interactable = true;
            List<LetterData> adminLetters = m_letterDatas.FindAll(letter => letter.IsAdmin);
            m_lettersWrapper.ShowDatas(adminLetters);
        }

        private void OnSelectLetterView(LetterView letterView)
        {
            m_letterPanelController.ShowLetter(letterView);
            CheckNews();
        }

        protected override void OnLoadGame()
        {
            List<LetterData> receiveLetters = CommonGameData.CommunicationData.LetterDatas
                .FindAll(letter => letter.ReceiverPlayerId == CommonGameData.PlayerInfoData.Id);

            m_letterDatas = receiveLetters;

            CheckNews();
            FilterAdminLetter();
        }

        private void CheckNews()
        {
            OnNewsStatusChangeInternal.Execute(m_letterDatas.Any(letter => !letter.IsOpened));
        }

        private async UniTask GetAllRewards()
        {
            RewardModel rewardModel = new RewardModel();
            rewardModel.CommonDictionaries = m_commonDictionaries;

            bool showReward = false;
            foreach (LetterData letter in m_letterDatas)
            {
                if (letter.IsRewardReceived) continue;

                if (string.IsNullOrWhiteSpace(letter.RewardJSON)) continue;

                GetRewardFromLetterMessage message = new GetRewardFromLetterMessage
                {
                    PlayerId = CommonGameData.PlayerInfoData.Id,
                    LetterId = letter.Id
                };
                string result = await DataServer.PostData(message);

                if (string.IsNullOrEmpty(result)) continue;

                try
                {
                    RewardModel reward = m_jsonConverter.Deserialize<RewardModel>(result);

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

            if (!showReward) return;

            GameReward gameReward = new GameReward(rewardModel, m_commonDictionaries);
            m_clientRewardService.ShowReward(gameReward);
        }

        private void DeleteAllLetters()
        {
        }
    }
}