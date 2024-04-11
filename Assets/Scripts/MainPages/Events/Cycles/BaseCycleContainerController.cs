using Db.CommonDictionaries;
using Models.Common;
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

        protected CompositeDisposable Disposables = new();

        [SerializeField] protected SliderTime CycleGameEventSliderTime;
        [SerializeField] protected Button MainPanelButton;

        public void SetData(DateTime startDateTime, TimeSpan gameCycleTime)
        {
            CycleGameEventSliderTime.SetData(startDateTime, gameCycleTime);
        }

        private void OnDestroy()
        {
            Disposables.Dispose();
        }
    }
}
