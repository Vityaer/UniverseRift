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
        public List<string> Products = new List<string>();

        public RecoveryType RecoveryType;

        public MarketModel(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }
    }
}
