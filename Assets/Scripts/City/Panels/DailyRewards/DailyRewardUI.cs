using City.Buildings.CityButtons.EventAgent;
using Common.Rewards;
using LocalizationSystems;
using System;
using System.Collections.Generic;
using TMPro;
using UI.Utils.Localizations.Extensions;
using UIController;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UniRx;

namespace City.Buildings.CityButtons.DailyReward
{
    public class DailyRewardUI : ScrollableUiView<GameReward>
    {
        private ILocalizationSystem _localizationSystem;

        public GameObject blockPanel;
        public GameObject readyForGet;
        public TMP_Text Label;
        public RewardUIController RewardController;

        private IDisposable _disposable;
        private ScrollableViewStatus statusReward = ScrollableViewStatus.Close;
        private int _id;

        [Inject]
        private void Construct(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _disposable = _localizationSystem.OnChangeLanguage.Subscribe(_ => ChangeUi());
        }

        private void ChangeUi()
        {
            _id = transform.GetSiblingIndex();
            Label.text = _localizationSystem.GetLocalizedContainer("DailyRewardName")
                .WithArguments(new List<object> { _id + 1 }).GetLocalizedString();
        }

        public override void SetData(GameReward data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            RewardController.ShowReward(data);
            ChangeUi();
        }

        public override void SetStatus(ScrollableViewStatus newStatusReward)
        {
            statusReward = newStatusReward;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (statusReward)
            {
                case ScrollableViewStatus.Completed:
                    blockPanel.SetActive(true);
                    //readyForGet.SetActive(false);
                    break;
                case ScrollableViewStatus.Close:
                    blockPanel.SetActive(false);
                    //readyForGet.SetActive(false);
                    break;
                case ScrollableViewStatus.Open:
                    blockPanel.SetActive(false);
                    //readyForGet.SetActive(true);
                    break;
            }
        }

        public void GetReward()
        {
            switch (statusReward)
            {
                case ScrollableViewStatus.Close:
                    //MessageController.Instance.AddMessage("Награда не открыта, приходите позже");
                    break;
                case ScrollableViewStatus.Completed:
                    //MessageController.Instance.AddMessage("Вы уже получали эту награду");
                    break;
                case ScrollableViewStatus.Open:
                    //DailyRewardPanelController.Instance.OnGetReward(transform.GetSiblingIndex());
                    SetStatus(ScrollableViewStatus.Completed);
                    break;
            }
        }

        public override void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            base.Dispose();
        }
    }
}