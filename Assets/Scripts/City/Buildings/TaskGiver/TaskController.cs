using TMPro;
using UnityEngine;
public class TaskController : MonoBehaviour
{
    [SerializeField] private Task task;
    [Header("UI")]
    public TextMeshProUGUI name;
    public GameObject objectCurrentTime;
    public SliderTimeScript sliderTime;
    public ButtonCostController buttonCostScript;
    public RatingHero ratingController;
    public SubjectCellControllerScript rewardUIController;

    public Task GetTask => task;

    public void SetData(Task task)
    {
        this.task = task;
        name.text = task.Name;
        ratingController.ShowRating(task.Rating);
        rewardUIController.SetItem(task.Reward);
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        buttonCostScript.Clear();
        switch (task.status)
        {
            case StatusTask.NotStart:
                sliderTime.SetMaxValue(task.requireTime);
                buttonCostScript.UpdateLabel(StartTask, "Начать");
                break;
            case StatusTask.InWork:
                sliderTime.RegisterOnFinish(FinishFromSlider);
                buttonCostScript.UpdateCost(new Resource(TypeResource.Diamond, task.Rating * 10, 0), BuyProgress);
                sliderTime.SetData(task.timeStartTask, task.requireTime);
                break;
            case StatusTask.Done:
                sliderTime.SetData(task.timeStartTask, task.requireTime);
                sliderTime.UnregisterOnFinish(FinishFromSlider);
                buttonCostScript.UpdateLabel(GetReward, "Завершить");
                break;
        }
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
        task.Finish();
        UpdateStatus();
    }

    //Action button
    public void StartTask(int count)
    {
        task.Start();
        objectCurrentTime.SetActive(true);
        sliderTime?.SetData(task.timeStartTask, task.requireTime);
        UpdateStatus();
        TaskGiver.Instance.UpdateSave();
    }

    public void BuyProgress(int count = 1)
    {
        buttonCostScript.Clear();
        sliderTime.SetFinish();
    }

    public void GetReward(int count)
    {
        TaskGiver.Instance.FinishTask(this);
    }

}
