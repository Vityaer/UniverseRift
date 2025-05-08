using Db.CommonDictionaries;
using Models.Common.BigDigits;
using Sirenix.OdinInspector;
using System;
using Utils;

namespace Models.Guilds
{
    public class BossModel
    {
        [NonSerialized] public CommonDictionaries CommonDictionaries;

        [ValueDropdown(nameof(_allHeroesName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string HeroId;

        public int Level = 1;
        public int Rating = 1;
        public int Stage = 0;

        public BigDigit Attack;
        public BigDigit Health;
        
        private string[] _allHeroesName => DictionaryUtils.GetArrayIds(CommonDictionaries.Heroes);

        public BossModel Clone()
        {
            BossModel result = new BossModel();
            result.CommonDictionaries = CommonDictionaries;
            result.HeroId = HeroId;
            result.Level = Level;
            result.Rating = Rating;
            result.Stage = Stage;
            result.Attack = Attack.Clone();
            result.Health = Health.Clone();
            
            return result;
        }
    }
}
