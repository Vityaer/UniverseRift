using Sirenix.OdinInspector;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;

namespace Models.Guilds
{
    public class GuildBossContainer : BaseModel
    {
        private CommonDictionaries _commonDictionaries;

        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
        NumberOfItemsPerPage = 20,
        CustomRemoveElementFunction = nameof(RemoveMission), CustomAddFunction = nameof(AddMission))]
        public List<GuildBossMission> Missions = new();

        public void SetDictionary(CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
            foreach (var mission in Missions)
            {
                foreach (var boss in mission.BossModels)
                    boss.CommonDictionaries = _commonDictionaries;

                foreach (var reward in mission.RewardModels)
                    reward.Reward.CommonDictionaries = _commonDictionaries;
            }
        }

        protected void AddMission()
        {
            var mission = new GuildBossMission();
            mission.SetCommonDictionary(_commonDictionaries);
            Missions.Add(mission);
        }

        private void RemoveMission(GuildBossMission light, object b, List<GuildBossMission> lights)
        {
            Missions.Remove(light);
        }

    }
}
