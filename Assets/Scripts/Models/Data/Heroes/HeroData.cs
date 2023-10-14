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
        [NonSerialized] public CommonDictionaries CommonDictionaries;

        private string[] _allHeroesName
        {
            get
            {
                var result = CommonDictionaries.Heroes.Values.Select(r => r.Id).ToArray();
                Debug.Log(result);
                return result;
            }
        }
        [HideInInspector] public int Id;
        [ValueDropdown(nameof(_allHeroesName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string HeroId;

        public int Level = 1;
        public int Rating = 1;
        [HideInInspector] public int CurrentBreakthrough = 0;
        [HideInInspector] public CostumeData Costume = new CostumeData();

        public HeroData() { }

        public HeroData(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }
    }
}
