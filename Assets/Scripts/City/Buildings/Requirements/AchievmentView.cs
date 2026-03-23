using System;
using City.Achievements;
using City.Panels.SubjectPanels.Common;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using LocalizationSystems;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.Achievments;
using UIController;
using UIController.ItemVisual;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Utils.DataCheckUtils;
using VContainer;

namespace City.Buildings.Requirements
{
    public class AchievmentView : ScrollableUiView<GameAchievment>
    {
        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;

        [SerializeField] private ItemSliderController SliderAmount;
        [SerializeField] private RewardUIController RewardController;
        [SerializeField] private LocalizeStringEvent Name;
        [SerializeField] private LocalizeStringEvent Description;
        [SerializeField] private GameObject DonePanel;

        private ReactiveCommand<AchievmentView> m_onGetReward = new();
        private ReactiveCommand m_observerOnChange = new();
        private ReactiveCommand m_observerComplete = new();
        private bool m_isExistNews = false;

        public IObservable<AchievmentView> OnGetReward => m_onGetReward;
        public ReactiveCommand ObserverOnChange => m_observerOnChange;
        public ReactiveCommand ObserverComplete => m_observerComplete;
        public bool IsComplete => Data is { IsComplete: true };
        public Button GetButton => Button;

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => GetReward().Forget()).AddTo(Disposable);
        }

        public override void SetData(GameAchievment data, ScrollRect scroll, CommonDictionaries commonDictionaries)
        {
            Data = data;
            Scroll = scroll.ThrowIfArgumentNull();
            CommonDataDictionaries = commonDictionaries.ThrowIfArgumentNull();
            
            Name.StringReference = m_localizationSystem.GetLocalizedContainer($"{Data.ModelId}Description");
            // Description.StringReference = m_localizationSystem.GetLocalizedContainer($"{Data.ModelId}Description");
            RewardController.Init(scroll, commonDictionaries);
            UpdateUI();
            Data.OnChangeData.Subscribe(_ => UpdateUI()).AddTo(Disposable);
        }

        public async UniTaskVoid GetReward()
        {
            AchievmentGetRewardMessage message = new AchievmentGetRewardMessage
                { PlayerId = m_commonGameData.PlayerInfoData.Id, AchievmentId = Data.Id };
            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                GiveReward();
                m_isExistNews = false;
                m_onGetReward.Execute(this);
                Data.NextStage();
                m_observerOnChange.Execute();
                UpdateUI();
            }
        }

        protected virtual void GiveReward()
        {
            Data.ShowAndGiveReward();
        }

        private void UpdateUI()
        {
            GameReward reward = Data.GetReward();
            RewardController.ShowReward(reward, CommonDataDictionaries);

            if (Data.CurrentStage < Data.CountStage)
            {
                bool checkCount = Data.CheckCount();
                Button.interactable = checkCount;
                SliderAmount.SetAmount(Data.Progress, Data.GetRequireCount());
                Button.gameObject.SetActive(checkCount);
                DonePanel.SetActive(false);
                
                if (!m_isExistNews && checkCount)
                {
                    m_isExistNews = true;
                    m_observerComplete.Execute();
                }
            }
            else
            {
                m_isExistNews = false;
                DonePanel.SetActive(true);
                Button.gameObject.SetActive(false);
                SliderAmount.Hide();
            }
        }

        public bool CheckDoneForReward()
        {
            return m_isExistNews;
        }

        public void SetReward(SubjectDetailController subjectDetailController)
        {
            RewardController.SetDetailsController(subjectDetailController);
        }
    }
}