using Db.CommonDictionaries;
using Models.Data;
using System.Linq;
using UnityEngine;
using VContainer;

namespace City.Buildings.TaskGiver
{
    public class GameTaskProvider
    {
        private const int MAX_SIMPLE_TASK_RATING = 4;

        [Inject] private readonly CommonDictionaries _commonDictionaries;

        public GameTask GetSimpleTask()
        {
            var workTasks = _commonDictionaries.PatternTasks.ToList();
            var model = workTasks[Random.Range(0, workTasks.Count)].Value;
            var result = new GameTask(model, new TaskData());
            return result;
        }

        public GameTask GetSpecialTask()
        {
            var workTasks = _commonDictionaries.PatternTasks.Where(task => task.Value.Rating > MAX_SIMPLE_TASK_RATING).ToList();
            var model = workTasks[Random.Range(0, workTasks.Count)].Value;
            var result = new GameTask(model, new TaskData());
            return result;
        }
    }
}