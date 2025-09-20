using Sirenix.OdinInspector;
using System;
using System.Linq;
using Common.Db.CommonDictionaries;
using Models.Guilds;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class HeroData
    {
        [NonSerialized] public CommonDictionaries CommonDictionaries;

        [HideInInspector] public int Id;
        [ValueDropdown(nameof(_allHeroesName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string HeroId;

        public int Level = 1;
        public int Rating = 1;
        public int Stage = 0;

        [HideInInspector] public int CurrentBreakthrough = 0;
        [HideInInspector] public CostumeData Costume = new CostumeData();

        public HeroData() { }

        public HeroData(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }

        public HeroData(BossModel bossModel)
        {
            HeroId = bossModel.HeroId;
            Level = bossModel.Level;
            Rating = bossModel.Rating;
            Stage = bossModel.Stage;
        }

        private string[] _allHeroesName
        {
            get
            {
                var result = CommonDictionaries.Heroes.Values.Select(r => r.Id).ToArray();
                return result;
            }
        }
    }
}
