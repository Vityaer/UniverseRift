using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Hero;
using Models;
using UIController.Inventory;

namespace City.Panels.SubjectPanels.Items
{
    public static class ItemCostumeExtraBonusesUtils
    {
        public static void GetExtraBonusesForItemOnHero(GameHero hero,
            GameItem gameItem,
            CommonDictionaries dictionaries,
            out List<Bonus> extraBonuses,
            out int bonusAvailableCount)
        {
            var targetItemSetName = gameItem.Model.SetName;
            var currentItems = hero.HeroData.Costume.Items;

            extraBonuses = dictionaries.ItemSets.TryGetValue(targetItemSetName, out ItemSet setModel)
                ? setModel.ExtraBonuses
                : new List<Bonus>();

            bonusAvailableCount = -1;
            
            foreach (var costumeItem in currentItems)
            {
                if(!dictionaries.ItemSets.TryGetValue(costumeItem.Value, out var costumeItemModel))
                {
                    continue;
                }

                if (costumeItemModel.Name.Equals(targetItemSetName))
                {
                    bonusAvailableCount += 1;
                }
            }
        }
        
        public static void GetExtraBonusesForHeroes(GameHero hero,
            CommonDictionaries dictionaries,
            out List<Bonus> extraBonuses)
        {
            extraBonuses = new();
            
            var currentItems = hero.HeroData.Costume.Items;

            Dictionary<string, int> setCounts = new();
            
            foreach (var costumeItem in currentItems)
            {
                if(!dictionaries.ItemSets.TryGetValue(costumeItem.Value, out var costumeItemModel))
                {
                    continue;
                }

                if (!setCounts.TryAdd(costumeItemModel.Name, 1))
                {
                    setCounts[costumeItemModel.Name] += 1;
                }
            }

            foreach (var set in setCounts.Where(set => set.Value > 1))
            {
                if(!dictionaries.ItemSets.TryGetValue(set.Key, out var costumeItemModel))
                {
                    continue;
                }
                
                for (var i = 1; i < set.Value; i++)
                {
                    
                    extraBonuses.Add(costumeItemModel.ExtraBonuses[i]);
                }
            }
        }

        public static void GetExtraBonusesForItem(GameItem item,
            CommonDictionaries commonDictionaries,
            out List<Bonus> extraBonuses,
            out int availableExtraBonusCount)
        {
            var targetItemSetName = item.Model.SetName;

            extraBonuses = commonDictionaries.ItemSets.TryGetValue(targetItemSetName, out ItemSet setModel)
                ? setModel.ExtraBonuses
                : new List<Bonus>();

            availableExtraBonusCount = extraBonuses.Count;
        }
    }
}