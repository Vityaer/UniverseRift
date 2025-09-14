using System;
using System.Collections.Generic;
using City.TaskBoard;
using Db.CommonDictionaries;
using Models.Data;
using Models.Tasks;

namespace City.Buildings.TaskGiver
{
    public class TaskDataComparer : IComparer<TaskData>
    {
        private readonly CommonDictionaries m_dictionaries;
        
        public TaskDataComparer(CommonDictionaries dictionaries)
        {
            m_dictionaries = dictionaries;
        }

        public int Compare(TaskData x, TaskData y)
        {
            if (!m_dictionaries.GameTaskModels.TryGetValue(x.TaskModelId, out var xTaskModel))
            {
                return 0;
            }
            
            if (!m_dictionaries.GameTaskModels.TryGetValue(y.TaskModelId, out var yTaskModel))
            {
                return 0;
            }
            
            if (x.Status == TaskStatusType.Done && y.Status == TaskStatusType.Done)
            {
                return CheckRating(xTaskModel.Rating, yTaskModel.Rating);
            }

            if (x.Status == TaskStatusType.Done)
            {
                return -1;
            }
            
            if (y.Status == TaskStatusType.Done)
            {
                return 1;
            }

            return CheckRating(xTaskModel.Rating, yTaskModel.Rating);
        }

        private int CheckRating(int x, int y)
        {
            if (y > x)
            {
                return 1;
            }
            
            if (y < x)
            {
                return -1;
            }
            return 0;
        }
    }
}