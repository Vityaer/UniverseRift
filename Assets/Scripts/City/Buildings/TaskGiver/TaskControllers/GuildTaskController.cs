using System;
using City.TaskBoard;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Models.Data;
using UniRx;
using UnityEngine.UI;

namespace City.Buildings.TaskGiver.TaskControllers
{
    public class GuildTaskController : BaseTaskController
    {
        public override void SetData(TaskData data, ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            base.SetData(data, scrollRect, dictionaries);
        }

        protected override void UpdateUi(DateTime startDateTime, TimeSpan requireTime)
        {
            switch (Data.Status)
            {
                case TaskStatusType.NotStart:
                    TaskControllerButton.gameObject.SetActive(true);
                    sliderTime.SetMaxValue(requireTime);
                    TaskControllerButton.SetLabel(_localizationSystem.GetString("StartButtonLabel"));
                    m_disposable = TaskControllerButton.OnClick.Subscribe(_ => StartTask().Forget());
                    break;

                case TaskStatusType.InWork:
                    TaskControllerButton.gameObject.SetActive(false);
                    TimeSliderDisposable = sliderTime.OnTimerFinish.Subscribe(_ => FinishFromSlider());
                    sliderTime.SetData(startDateTime, requireTime);
                    break;

                case TaskStatusType.Done:
                    TaskControllerButton.gameObject.SetActive(true);
                    sliderTime.SetData(startDateTime, requireTime);

                    TimeSliderDisposable?.Dispose();
                    TimeSliderDisposable = null;
                    
                    TaskControllerButton.SetLabel(_localizationSystem.GetString("TaskCompleteButtonLabel"));
                    m_disposable = TaskControllerButton.OnClick.Subscribe(_ => GetReward().Forget());
                    break;
            }
        }
    }
}