﻿using Db.CommonDictionaries;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UIController.Rewards;

namespace Models.Fights.Campaign
{
    [Serializable]
    public class MissionModel
    {
        [NonSerialized] public CommonDictionaries Dictionaries;

        public string Name;
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

    }
}