using City.TaskBoard;
using Common.Resourses;
using Models.Data;
using TMPro;
using UIController;
using UIController.ItemVisual;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.TaskGiver
{
    public class TaskController : ScrollableUiView<TaskData>
    {
        private TaskData _data;
        private TaskModel _model;

        [Header("UI")]
        public TextMeshProUGUI Name;
        public GameObject objectCurrentTime;
        public SliderTime sliderTime;
        public ButtonCostController buttonCostScript;
        public RatingHero ratingController;
        public SubjectCell RewardUIController;

        public TaskData GetTask => _data;

        public override void SetData(TaskData data, ScrollRect scrollRect)
        {
            throw new System.NotImplementedException();
        }

        public void SetData(TaskData data, TaskModel model)
        {
            _data = data;
            _model = model;

            Name.text = _model.Name;
            ratingController.ShowRating(_model.Rating);
            //RewardUIController.SetItem(task.Reward);
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            buttonCostScript.Clear();
            //switch (task.status)
            //{
            //    case TaskStatusType.NotStart:
            //        sliderTime.SetMaxValue(task.requireTime);
            //        buttonCostScript.UpdateLabel(StartTask, "Начать");
            //        break;
            //    case TaskStatusType.InWork:
            //        sliderTime.RegisterOnFinish(FinishFromSlider);
            //        buttonCostScript.UpdateCost(new GameResource(ResourceType.Diamond, task.Rating * 10, 0), BuyProgress);
            //        sliderTime.SetData(task.timeStartTask, task.requireTime);
            //        break;
            //    case TaskStatusType.Done:
            //        sliderTime.SetData(task.timeStartTask, task.requireTime);
            //        sliderTime.UnregisterOnFinish(FinishFromSlider);
            //        buttonCostScript.UpdateLabel(GetReward, "Завершить");
            //        break;
            //}
        }
        public void StopTimer()
        {
            sliderTime.StopTimer();
        }
        private void FinishFromSlider()
        {
            FinishTask();
        }

        private void FinishTask()
        {
            sliderTime.UnregisterOnFinish(FinishFromSlider);
            //task.Finish();
            UpdateStatus();
        }

        //Action button
        public void StartTask(int count)
        {
            //task.Start();
            objectCurrentTime.SetActive(true);
            //sliderTime?.SetData(task.timeStartTask, task.requireTime);
            UpdateStatus();
            //TaskboardController.Instance.UpdateSave();
        }

        public void BuyProgress(int count = 1)
        {
            buttonCostScript.Clear();
            sliderTime.SetFinish();
        }

        public void GetReward(int count)
        {
            //TaskboardController.Instance.FinishTask(this);
        }


    }
}