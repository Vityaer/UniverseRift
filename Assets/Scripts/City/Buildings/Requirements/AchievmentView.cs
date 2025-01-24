using City.Achievements;
using City.Panels.SubjectPanels.Common;
using Cysharp.Threading.Tasks;
using LocalizationSystems;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.Achievments;
using System;
using TMPro;
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
    public class AchievmentView : ScrollableUiView<GameAchievment>, IDisposable
    {
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly ILocalizationSystem _localizationSystem;

        [SerializeField] private ItemSliderController SliderAmount;
        [SerializeField] private RewardUIController RewardController;
        [SerializeField] private LocalizeStringEvent Name;
        [SerializeField] private LocalizeStringEvent Description;
        [SerializeField] private GameObject DonePanel;

        private ReactiveCommand _observerOnChange = new();
        private ReactiveCommand _observerComplete = new();

        public ReactiveCommand ObserverOnChange => _observerOnChange;
        public ReactiveCommand ObserverComplete => _observerComplete;
        public bool IsEmpty { get => Data == null; }
        public bool IsComplete { get => !IsEmpty & Data.IsComplete; }

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => GetReward().Forget()).AddTo(Disposable);
        }

        public override void SetData(GameAchievment data, ScrollRect scroll)
        {
            Data = data;
            Scroll = scroll;
            Name.StringReference = _localizationSystem.GetLocalizedContainer($"{Data.ModelId}Description");
            //Description.StringReference = _localizationSystem.GetLocalizedContainer($"{Data.ModelId}Description");
            UpdateUI();
            Data.OnChangeData.Subscribe(_ => UpdateUI()).AddTo(Disposable);
        }

        public async UniTaskVoid GetReward()
        {
            var message = new AchievmentGetRewardMessage { PlayerId = _commonGameData.PlayerInfoData.Id, AchievmentId = Data.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                Data.GiveReward();
                Data.NextStage();
                _observerOnChange.Execute();
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            var reward = Data.GetReward();
            RewardController.ShowReward(reward);

            if (Data.CurrentStage < Data.CountStage)
            {
                Button.interactable = Data.CheckCount();
                SliderAmount.SetAmount(Data.Progress, Data.GetRequireCount());
                Button.gameObject.SetActive(true);
                DonePanel.SetActive(false);
            }
            else
            {
                DonePanel.SetActive(true);
                Button.gameObject.SetActive(false);
                SliderAmount.Hide();
            }
        }

        public void SetReward(SubjectDetailController subjectDetailController)
        {
            RewardController.SetDetailsController(subjectDetailController);
        }
    }
}