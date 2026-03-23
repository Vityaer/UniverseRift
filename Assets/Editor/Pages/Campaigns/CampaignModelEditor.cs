using Campaign;
using Editor.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Common.Db.CommonDictionaries;
using UIController.Rewards;

namespace Editor.Pages.Campaigns
{
    [HideReferenceObjectPicker]
    public class CampaignModelEditor : BaseModelEditor<CampaignChapterModel>
    {
        private CommonDictionaries _commonDictionaries;

        public CampaignModelEditor(CampaignChapterModel model, CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
            m_model = model;
            foreach (var mission in m_model.Missions)
            {
                mission.Dictionaries = _commonDictionaries;
                if(mission.Units != null)
                    foreach (var unit in mission.Units)
                    {
                        unit.CommonDictionaries = _commonDictionaries;
                    }

                mission.WinReward.CommonDictionaries = _commonDictionaries;
                if (mission.WinReward.Items != null)
                    foreach (var item in mission.WinReward.Items)
                        item.CommonDictionaries = _commonDictionaries;

                mission.AutoFightReward.SetCommonDictionaries(_commonDictionaries);
            }
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(250)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Name")]
        [LabelWidth(250)]
        public string Name
        {
            get => m_model.Name;
            set => m_model.Name = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("Mission")]
        [LabelWidth(250)]
        [ListDrawerSettings(Expanded = false,
        NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveMission), CustomAddFunction = nameof(AddMission))]
        public List<CampaignMissionModel> Missions
        {
            get => m_model.Missions;
            set => m_model.Missions = value;
        }

        protected void AddMission()
        {
            Missions.Add(new CampaignMissionModel(_commonDictionaries));
        }

        private void RemoveMission(CampaignMissionModel light, object b, List<CampaignMissionModel> lights)
        {
            Missions.Remove(light);
        }
    }
}
