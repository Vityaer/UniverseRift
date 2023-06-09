using Campaign;
using Fight;
using Fight.WarTable;
using MainScripts;
using System;
using System.Collections.Generic;
using UIController.Rewards;

namespace Models.Fights.Campaign
{
    [System.Serializable]
    public class MissionModel : BaseModel, ICloneable
    {
        public string Name;
        public string Location;
        public List<Unit> ListEnemy;
        public Reward WinReward;

        public virtual void OnFinishFight(FightResultType fightResult)
        {
            if (fightResult == FightResultType.Win)
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
            return new MissionModel
            {
                Name = Name,
                ListEnemy = ListEnemy,
                Location = Location,
                WinReward = WinReward.Clone()
            };
        }
    }
}