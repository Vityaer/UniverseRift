using Common;
using Common.Inventories.Splinters;
using Db.CommonDictionaries;
using Sirenix.OdinInspector;
using System.Linq;

namespace Models.Data.Inventories
{
    public class SplinterData : InventoryBaseItem
    {
        private string[] _allSplinterName => CommonDictionaries.Splinters.Values.Select(r => r.Id).ToArray();
        [ValueDropdown(nameof(_allSplinterName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Id;

        public int Amount;

        public SplinterData() { }

        public SplinterData(GameSplinter splinter)
        {
            Id = splinter.Id;
            Amount = splinter.Amount;
        }

        public SplinterData(CommonDictionaries dictionaries)
        {
            CommonDictionaries = dictionaries;
        }

        public override BaseObject CreateGameObject()
        {
            var result = new GameSplinter(Id, Amount);
            result.SetCommonDictionaries(CommonDictionaries);
            return result;
        }
    }
}
