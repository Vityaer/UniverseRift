using LocalizationSystems;
using Models.Common;
using Services.TimeLocalizeServices;
using System;
using Common.Db.CommonDictionaries;
using UIController;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainerUi.Services;

namespace MainPages.Events.Cycles
{
    public abstract class BaseCycleContainerController : MonoBehaviour
    {
        [Inject] protected IUiMessagesPublisherService UiMessagesPublisher;
        [Inject] protected CommonDictionaries CommonDictionaries;
        [Inject] protected CommonGameData CommonGameData;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        [Inject] private readonly TimeLocalizeService m_timeLocalizeService;

        protected readonly CompositeDisposable Disposables = new();

        [SerializeField] protected SliderTime CycleGameEventSliderTime;
        [SerializeField] protected Button MainPanelButton;

        public virtual void SetData(DateTime startDateTime, TimeSpan gameCycleTime)
        {
            CycleGameEventSliderTime.Init(m_localizationSystem, m_timeLocalizeService);
            CycleGameEventSliderTime.SetData(startDateTime, gameCycleTime);
        }

        private void OnDestroy()
        {
            Disposables.Dispose();
        }
    }
}
