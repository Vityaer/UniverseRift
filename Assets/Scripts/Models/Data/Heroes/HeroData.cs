using Db.CommonDictionaries;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class HeroData
    {
        [NonSerialized] public CommonDictionaries _commonDictionaries;

        private string[] _allHeroesName => _commonDictionaries.Heroes.Values.Select(r => r.Id).ToArray();
        [ValueDropdown(nameof(_allHeroesName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public int Id;
        public string HeroId;

        public int Level = 1;
        public int Rating = 1;
        [HideInInspector] public int CurrentBreakthrough = 0;
        [HideInInspector] public CostumeData Costume = new CostumeData();

        public HeroData() { }

        public HeroData(CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
        }
    }
}
