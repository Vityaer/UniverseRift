using UnityEngine;

namespace City.Buildings.PageCycleEvent.MonthlyEvents
{
    public class GameMonthlyTasks
    {
        public MonthlyEventsController monthlyEventsParent;
        public TypeMonthlyTasks type;
        //public GameObject MainPanel => building;

        //protected override void OnLoadGame() { }

        //protected override void SaveData()
        //{
        //    monthlyEventsParent.SaveData(type, listRequirement);
        //}

        //protected override void OnAfterLoadData()
        //{
        //    for (int i = 0; i < requirementControllers.Count - 1; i++)
        //    {
        //        if (requirementControllers[i].IsEmpty == false)
        //        {
        //            requirementControllers[i].RegisterOnComplete(OnCompleteTask);
        //        }
        //    }
        //    // OnCompleteTask();
        //}

        //private void OnCompleteTask()
        //{
        //    bool result = true;
        //    for (int i = 0; i < requirementControllers.Count - 1; i++)
        //    {
        //        if (requirementControllers[i].IsComplete == false)
        //        {
        //            result = false;
        //            break;
        //        }
        //    }

        //    if (result && requirementControllers[requirementControllers.Count - 1].IsComplete == false)
        //    {
        //        requirementControllers[requirementControllers.Count - 1].ChangeProgress(new BigDigit(1));
        //    }
        //}
    }
}