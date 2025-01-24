using Db.CommonDictionaries;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.City.Markets
{
    [System.Serializable]
    public class MarketModel : BaseModel
    {
        [NonSerialized] public CommonDictionaries CommonDictionaries;

        private string[] _allProductId => CommonDictionaries.Products.Values.Select(r => r.Id).ToArray();
        [ValueDropdown(nameof(_allProductId), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public List<string> Products = new();

        public RecoveryType RecoveryType;

        [ValueDropdown(nameof(_allProductId), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public List<string> Promotions = new();

        public MarketModel(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }
    }
}
