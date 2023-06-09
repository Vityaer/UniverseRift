using City.TaskBoard;
using Common;
using Common.Resourses;
using Models;
using Models.Heroes;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskModel : BaseModel, ICloneable
{
    public string Name;
    public int ID;
    public int Rating;
    public List<HeroModel> heroes = new List<HeroModel>();
    public int RequireHour;
    private TimeSpan _requireTime = new TimeSpan();
    public TimeSpan requireTime
    {
        get
        {
            if (_requireTime.Hours == 0) { _requireTime = new TimeSpan(RequireHour, 0, 0); }
            return _requireTime;
        }
        set { _requireTime = value; }
    }

    private DateTime _timeStartTask = new DateTime();
    public DateTime timeStartTask
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

    public long strTimeStartTask;
    public TaskStatusType status = TaskStatusType.NotStart;
    [HideInInspector][SerializeField] private Resource reward;
    public Resource Reward { get => reward; set => reward = value; }
    public void Start()
    {
        status = TaskStatusType.InWork;
        timeStartTask = DateTime.Now;
    }
    public void Finish()
    {
        status = TaskStatusType.Done;
    }
    //API
    public void GetReward()
    {
        GameController.Instance.AddResource(reward);
    }

    public object Clone()
    {
        return new TaskModel
        {
            ID = this.ID,
            Name = this.Name,
            Rating = this.Rating,
            heroes = this.heroes,
            RequireHour = this.RequireHour,
            strTimeStartTask = this.strTimeStartTask,
            reward = this.reward,
            status = this.status
        };
    }
}


