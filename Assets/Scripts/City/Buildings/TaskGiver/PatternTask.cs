using Common.Resourses;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.TaskGiver
{
    [CreateAssetMenu(fileName = "PatternTasks", menuName = "Custom ScriptableObject/PatternTasks", order = 55)]
    public class PatternTask : ScriptableObject
    {
        public List<TaskModel> tasks = new List<TaskModel>();
        public List<RewardForTaskFromRating> rewardForTaskFromRating = new List<RewardForTaskFromRating>();

        public TaskModel GetSimpleTask()
        {
            List<TaskModel> workTasks = tasks.FindAll(x => x.Rating <= 4);
            TaskModel result = (TaskModel)workTasks[Random.Range(1, workTasks.Count)].Clone();
            result.Reward = GetRandomReward(result.Rating);
            return result;
        }

        public TaskModel GetSpecialTask()
        {
            List<TaskModel> workTasks = tasks.FindAll(x => x.Rating > 4);
            TaskModel result = (TaskModel)workTasks[Random.Range(1, workTasks.Count)].Clone();
            result.Reward = GetRandomReward(result.Rating);
            return result;
        }

        public TaskModel GetRandomTask()
        {
            TaskModel result = (TaskModel)tasks[Random.Range(0, tasks.Count)].Clone();
            result.Reward = GetRandomReward(result.Rating);
            return result;
        }

        [ContextMenu("Check equals ID")]
        private void CheckEquealsID()
        {
            for (int i = 0; i < tasks.Count - 1; i++)
            {
                for (int j = i + 1; j < tasks.Count; j++)
                {
                    if (tasks[i].ID == tasks[j].ID)
                        Debug.Log("Tasks have equals ID = " + tasks[i].ID.ToString() + ", name task = '" + tasks[i].Name + "' and task = '" + tasks[j].Name + "'");
                }
            }
        }

        public Resource GetRandomReward(int rating)
        {
            List<RewardForTaskFromRating> listReward = rewardForTaskFromRating.FindAll(x => x.rating == rating);
            return listReward[Random.Range(0, listReward.Count)].GetReward();
        }
    }

    [System.Serializable]
    public class RewardForTaskFromRating
    {
        public int rating;
        public Resource res;
        public float delta = 0.25f;
        public Resource GetReward()
        {
            return new Resource(res.Name, res.Amount) * Random.Range(1 - delta, 1 + delta);
        }
    }
}