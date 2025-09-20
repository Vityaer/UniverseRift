using City.Buildings.TravelCircle;
using Editor.Common;
using Models.City.TravelCircle;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;

namespace Editor.Pages.Buildings.TravelRaceCampaigns
{
    public class TravelRaceModelEditor : BaseModelEditor<TravelRaceModel>
    {
        private CommonDictionaries _commonDictionaries;
        private string[] _allRaces => _commonDictionaries.Races.Select(c => c.Value).Select(r => r.Id).ToArray();

        public TravelRaceModelEditor(TravelRaceModel model, CommonDictionaries dictionaries)
        {
            _commonDictionaries = dictionaries;
            _model = model;

            foreach (var mission in _model.Missions)
            {
                mission.Dictionaries = _commonDictionaries;
                if (mission.Units != null)
                    foreach (var unit in mission.Units)
                    {
                        unit.CommonDictionaries = _commonDictionaries;
                    }

                mission.WinReward.CommonDictionaries = _commonDictionaries;
                if (mission.WinReward.Items != null)
                    foreach (var item in mission.WinReward.Items)
                        item.CommonDictionaries = _commonDictionaries;

                mission.SmashReward.CommonDictionaries = _commonDictionaries;
                if (mission.SmashReward.Items != null)
                    foreach (var item in mission.SmashReward.Items)
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
        [LabelText("Race")]
        [LabelWidth(150)]
        [ValueDropdown(nameof(_allRaces), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Race
        {
            get => _model.Race;
            set => _model.Race = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("Missions")]
        [LabelWidth(150)]
        [ListDrawerSettings(Expanded = false,
        NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveMission), CustomAddFunction = nameof(AddMission))]
        public List<MissionWithSmashReward> Missions
        {
            get => _model.Missions;
            set => _model.Missions = value;
        }

        protected void AddMission()
        {
            Missions.Add(new MissionWithSmashReward(_commonDictionaries));
        }

        private void RemoveMission(MissionWithSmashReward light, object b, List<MissionWithSmashReward> lights)
        {
            Missions.Remove(light);
        }
    }
}
