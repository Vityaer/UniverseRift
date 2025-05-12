using System;
using City.TaskBoard;
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
                    _disposable = TaskControllerButton.OnClick.Subscribe(_ => StartTask().Forget());
                    break;

                case TaskStatusType.InWork:
                    _timeSliderDisposable = sliderTime.OnTimerFinish.Subscribe(_ => FinishFromSlider());
                    var costFastFinish = new GameResource(ResourceType.Diamond, _model.Rating * 10, 0);
                    TaskControllerButton.SetCost(costFastFinish);
                    sliderTime.SetData(startDateTime, requireTime);
                    _disposable = TaskControllerButton.OnClick.Subscribe(_ => BuyProgress().Forget());
                    break;

                case TaskStatusType.Done:
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