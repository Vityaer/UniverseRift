using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using UIController.Rewards;

namespace Models.Fights.Campaign
{
    [Serializable]
    public class MissionModel
    {
        [NonSerialized] public CommonDictionaries Dictionaries;

        public string Name;
        
        [ValueDropdown(nameof(_allLocationName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Location;
        
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
        NumberOfItemsPerPage = 20,
        CustomRemoveElementFunction = nameof(RemoveHero), CustomAddFunction = nameof(AddHero))]
        public List<HeroData> Units = new();
        public RewardModel WinReward = new();

        public List<AbstractHeroRestriction> HeroRestrictions;
        public MissionModel() { }

        public MissionModel(CommonDictionaries dictionaries)
        {
            Dictionaries = dictionaries;
            WinReward.CommonDictionaries = dictionaries;
        }

        protected void AddHero()
        {
            Units.Add(new HeroData(Dictionaries));
        }

        private void RemoveHero(HeroData light, object b, List<HeroData> lights)
        {
            Units.Remove(light);
        }

        private string[] _allLocationName
        {
            get
            {
                var result = Dictionaries.LocationModels.Values.Select(r => r.Id).ToArray();
                return result;
            }
        }
    }
}