using System;
using City.TaskBoard;
using Cysharp.Threading.Tasks;
using Models.Data;
using UniRx;
using UnityEngine.UI;

namespace City.Buildings.TaskGiver
{
    public class GuildTaskController : BaseTaskController
    {
        public override void SetData(TaskData data, ScrollRect scrollRect)
        {
            base.SetData(data, scrollRect);
        }

        protected override void UpdateUi(DateTime startDateTime, TimeSpan requireTime)
        {
            switch (Data.Status)
            {
                case TaskStatusType.NotStart:
                    TaskControllerButton.gameObject.SetActive(true);
                    sliderTime.SetMaxValue(requireTime);
                    TaskControllerButton.SetLabel(_localizationSystem.GetString("StartButtonLabel"));
                    _disposable = TaskControllerButton.OnClick.Subscribe(_ => StartTask().Forget());
                    break;

                case TaskStatusType.InWork:
                    TaskControllerButton.gameObject.SetActive(false);
                    _timeSliderDisposable = sliderTime.OnTimerFinish.Subscribe(_ => FinishFromSlider());
                    sliderTime.SetData(startDateTime, requireTime);
                    break;

                case TaskStatusType.Done:
                    TaskControllerButton.gameObject.SetActive(true);
                    sliderTime.SetData(startDateTime, requireTime);

                    _timeSliderDisposable?.Dispose();
                    _timeSliderDisposable = null;
                    
                    TaskControllerButton.SetLabel(_localizationSystem.GetString("TaskCompleteButtonLabel"));
                    _disposable = TaskControllerButton.OnClick.Subscribe(_ => GetReward().Forget());
                    break;
            }
        }
    }
}