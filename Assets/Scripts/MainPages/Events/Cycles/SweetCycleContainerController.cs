using System;
using City.Panels.Events.SweetCycles;
using Common.Resourses;
using Misc.Json;
using Models;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace MainPages.Events.Cycles
{
    public class SweetCycleContainerController : BaseCycleContainerController
    {
        [Inject] private readonly IJsonConverter _jsonConverter;
        
        [SerializeField] private Image _candyImage;
        
        private void Start()
        {
            MainPanelButton.OnClickAsObservable().Subscribe(_ => OpenMainPanel()).AddTo(Disposables);
        }

        public override void SetData(DateTime startDateTime, TimeSpan gameCycleTime)
        {
            var data = _jsonConverter.Deserialize<SweetEventData>(CommonGameData.CycleEventsData.CurrentCycle);
            var resource = new GameResource(data.ResourceType);
            _candyImage.sprite = resource.Image;
            base.SetData(startDateTime, gameCycleTime);
        }

        private void OpenMainPanel()
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<SweetCycleMainPanelController>(openType: OpenType.Exclusive);
        }
    }
}
