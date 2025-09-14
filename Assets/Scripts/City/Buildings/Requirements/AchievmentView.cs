using System;
using City.Achievements;
using City.Panels.SubjectPanels.Common;
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
using VContainer;

namespace City.Buildings.Requirement
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
        public bool IsComplete => Data != null & Data.IsComplete;
        public Button GetButton => Button;

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => GetReward().Forget()).AddTo(Disposable);
        }

        public override void SetData(GameAchievment data, ScrollRect scroll)
        {
            Data = data;
            Scroll = scroll;
            Name.StringReference = m_localizationSystem.GetLocalizedContainer($"{Data.ModelId}Description");
            //Description.StringReference = _localizationSystem.GetLocalizedContainer($"{Data.ModelId}Description");
            RewardController.SetScroll(scroll);
            UpdateUI();
            Data.OnChangeData.Subscribe(_ => UpdateUI()).AddTo(Disposable);
        }

        public async UniTaskVoid GetReward()
        {
            var message = new AchievmentGetRewardMessage
                { PlayerId = m_commonGameData.PlayerInfoData.Id, AchievmentId = Data.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                GiveReward();
                m_onGetReward.Execute(this);
                Data.NextStage();
                m_observerOnChange.Execute();
                UpdateUI();
            }
        }

        protected virtual void GiveReward()
        {
            m_isExistNews = false;
            Data.ShowAndGiveReward();
        }

        public void UpdateUI()
        {
            var reward = Data.GetReward();
            RewardController.ShowReward(reward);

            if (Data.CurrentStage < Data.CountStage)
            {
                if (!m_isExistNews)
                {
                    m_isExistNews = true;
                    m_observerComplete.Execute();
                }

                Button.interactable = Data.CheckCount();
                SliderAmount.SetAmount(Data.Progress, Data.GetRequireCount());
                Button.gameObject.SetActive(true);
                DonePanel.SetActive(false);
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
            return Data.CheckCount();
        }

        public void SetReward(SubjectDetailController subjectDetailController)
        {
            RewardController.SetDetailsController(subjectDetailController);
        }
    }
}