using Campaign;
using Db.CommonDictionaries;
using Editor.Common;
using Models.City.Misc;
using Models.Fights.Campaign;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;

namespace Pages.City.ChallengeTower
{
    [HideReferenceObjectPicker]
    public class StorageChallangeModelEditor : BaseModelEditor<StorageChallengeModel>
    {
        private CommonDictionaries _commonDictionaries;

        public StorageChallangeModelEditor(StorageChallengeModel model, CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
            _model = model;

            foreach (var mission in _model.Missions)
            {
                mission.Dictionaries = _commonDictionaries;
                if (mission.Units != null)
                    foreach (var unit in mission.Units)
                    {
                        unit.CommonDictionaries = _commonDictionaries;
                    }

                mission.WinReward._dictionaries = _commonDictionaries;
                if (mission.WinReward.Items != null)
                    foreach (var item in mission.WinReward.Items)
                        item.CommonDictionaries = _commonDictionaries;
            }
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(150)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Missions")]
        [LabelWidth(150)]
        [ListDrawerSettings(Expanded = false,
        NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveMission), CustomAddFunction = nameof(AddMission))]
        public List<MissionModel> Missions
        {
            get => _model.Missions;
            set => _model.Missions = value;
        }

        protected void AddMission()
        {
            var newMission = new MissionModel(_commonDictionaries);
            newMission.WinReward._dictionaries = _commonDictionaries;
            if (newMission.WinReward.Items != null)
                foreach (var item in newMission.WinReward.Items)
                    item.CommonDictionaries = _commonDictionaries;
            Missions.Add(newMission);
        }

        private void RemoveMission(MissionModel light, object b, List<MissionModel> lights)
        {
            Missions.Remove(light);
        }
    }
}
