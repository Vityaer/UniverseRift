using System;
using System.Collections.Generic;
using City.Buildings.CityButtons.EventAgent;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using LocalizationSystems;
using TMPro;
using UI.Utils.Localizations.Extensions;
using UIController;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataCheckUtils;
using VContainer;

namespace City.Panels.DailyRewards
{
    public class DailyRewardUI : ScrollableUiView<GameReward>
    {
        private ILocalizationSystem m_localizationSystem;

        public GameObject blockPanel;
        public GameObject readyForGet;
        public TMP_Text Label;
        public RewardUIController RewardController;

        private IDisposable m_disposable;
        private ScrollableViewStatus m_statusReward = ScrollableViewStatus.Close;
        private int m_id;

        [Inject]
        private void Construct(ILocalizationSystem localizationSystem)
        {
            m_localizationSystem = localizationSystem;
            m_disposable = m_localizationSystem.OnChangeLanguage.Subscribe(_ => ChangeUi());
        }

        private void ChangeUi()
        {
            m_id = transform.GetSiblingIndex();
            Label.text = m_localizationSystem.GetLocalizedContainer("DailyRewardName")
                .WithArguments(new List<object> { m_id + 1 }).GetLocalizedString();
        }

        public override void SetData(GameReward data, ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            Data = data.ThrowIfArgumentNull();
            Scroll = scrollRect.ThrowIfArgumentNull();
            CommonDataDictionaries = dictionaries.ThrowIfArgumentNull();

            RewardController.Init(Scroll, CommonDataDictionaries);
            RewardController.ShowReward(data, CommonDataDictionaries);
            ChangeUi();
        }

        public override void SetStatus(ScrollableViewStatus newStatusReward)
        {
            m_statusReward = newStatusReward;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (m_statusReward)
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
            switch (m_statusReward)
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
            m_disposable?.Dispose();
            m_disposable = null;
            base.Dispose();
        }
    }
}