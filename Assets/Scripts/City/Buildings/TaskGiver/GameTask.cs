using City.TaskBoard;
using Common;
using Common.Resourses;
using Models.Data;
using Models.Tasks;
using System;
using UnityEngine;

namespace City.Buildings.TaskGiver
{
    public class GameTask
    {
        private GameTaskModel _taskModel;
        private TimeSpan _requireTime;
        private DateTime _timeStartTask;
        private TaskData _taskData;

        public long strTimeStartTask;
        public TaskStatusType Status;
        private GameResource reward;

        public GameResource Reward { get => reward; set => reward = value; }

        public TimeSpan RequireTime
        {
            get
            {
                if (_requireTime.Hours == 0) { _requireTime = new TimeSpan(_taskModel.RequireHour, 0, 0); }
                return _requireTime;
            }
            set { _requireTime = value; }
        }

        public DateTime TimeStartTask
        {
            get
            {
                if (DateTime.Equals(_timeStartTask, new DateTime()))
                {
                    if (strTimeStartTask != 0)
                    {
                        _timeStartTask = DateTime.FromBinary(strTimeStartTask);
                    }
                }
                return _timeStartTask;
            }
            set
            {
                if (DateTime.Equals(_timeStartTask, new DateTime()))
                {
                    _timeStartTask = value;
                    strTimeStartTask = _timeStartTask.ToBinary();
                }
            }
        }

        public GameTask(GameTaskModel taskModel, TaskData taskData)
        {
            _taskModel = taskModel;
            _taskData = taskData;
        }

        public void Start()
        {
            Status = TaskStatusType.InWork;
            TimeStartTask = DateTime.UtcNow;
            _taskData.DateTimeStart = TimeStartTask.ToString();
        }

        public void Finish()
        {
            Status = TaskStatusType.Done;
        }

        public void GetReward()
        {
            //GameController.Instance.AddResource(reward);
        }
    }
}
