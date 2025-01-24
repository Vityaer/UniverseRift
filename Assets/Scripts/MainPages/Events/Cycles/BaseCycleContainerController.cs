using Db.CommonDictionaries;
using LocalizationSystems;
using Models.Common;
using Services.TimeLocalizeServices;
using System;
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
        [Inject] private readonly ILocalizationSystem _localizationSystem;
        [Inject] private readonly TimeLocalizeService _timeLocalizeService;

        protected CompositeDisposable Disposables = new();

        [SerializeField] protected SliderTime CycleGameEventSliderTime;
        [SerializeField] protected Button MainPanelButton;

        public void SetData(DateTime startDateTime, TimeSpan gameCycleTime)
        {
            CycleGameEventSliderTime.Init(_localizationSystem, _timeLocalizeService);
            CycleGameEventSliderTime.SetData(startDateTime, gameCycleTime);
        }

        private void OnDestroy()
        {
            Disposables.Dispose();
        }
    }
}
