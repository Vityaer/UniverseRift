using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission : ICloneable
{
    [SerializeField] protected string name;
    public TypeLocation Location;

    [Header("Enemy")]
    [SerializeField] protected List<MissionEnemy> ListEnemy = new List<MissionEnemy>();
    [SerializeField] public Reward WinReward;

    public string Name { get => name; set => name = value; }
    public List<MissionEnemy> listEnemy => ListEnemy;

    public virtual void OnFinishFight(FightResult fightResult)
    {
        if (fightResult == FightResult.Win)
        {
            MessageController.Instance.OpenWin(WinReward, WarTableController.Instance.FinishMission);
        }
        else
        {
            MessageController.Instance.OpenDefeat(null, WarTableController.Instance.FinishMission);
        }
    }

    public object Clone()
    {
        return new Mission
        {
            Name = this.Name,
            ListEnemy = this.listEnemy,
            Location = this.Location,
            WinReward = (Reward)this.WinReward.Clone()
        };
    }
}