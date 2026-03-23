using System;
using City.Buildings.TaskGiver.TaskControllers;
using City.TaskBoard;
using Common.Inventories.Resourses;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using UniRx;
namespace City.Buildings.TaskGiver
{
    public class TaskController : BaseTaskController
    {
        protected override void UpdateUi(DateTime startDateTime, TimeSpan requireTime)
        {
            switch (Data.Status)
            {
                case TaskStatusType.NotStart:
                    sliderTime.SetMaxValue(requireTime);
                    TaskControllerButton.SetLabel(_localizationSystem.GetString("StartButtonLabel"));
                    m_disposable = TaskControllerButton.OnClick.Subscribe(_ => StartTask().Forget());
                    break;

                case TaskStatusType.InWork:
                    TimeSliderDisposable = sliderTime.OnTimerFinish.Subscribe(_ => FinishFromSlider());
                    var costFastFinish = new GameResource(ResourceType.Diamond, _model.Rating * 10, 0);
                    TaskControllerButton.SetCost(costFastFinish);
                    sliderTime.SetData(startDateTime, requireTime);
                    m_disposable = TaskControllerButton.OnClick.Subscribe(_ => BuyProgress().Forget());
                    break;

                case TaskStatusType.Done:
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