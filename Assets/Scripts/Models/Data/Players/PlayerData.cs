using Models.Data.Heroes;
using System;
using System.Collections.Generic;

namespace Models.Data.Players
{
    [Serializable]
    public class PlayerData : BaseDataModel
    {
        public PlayerInfoData PlayerInfoData = new PlayerInfoData();
        public List<ResourceData> Resources;
        public List<TaskData> ListTasks;
        public AchievmentStorageData Requirements;
        public HeroesStorage HeroesStorage;
    }
}
