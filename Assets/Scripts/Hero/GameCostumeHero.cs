using Db.CommonDictionaries;
using Models;
using Models.Heroes.HeroCharacteristics;
using Models.Items;
using System.Collections.Generic;
using UIController.Inventory;

namespace Hero
{
    [System.Serializable]
    public class GameCostumeHero
    {
        public Dictionary<ItemType, GameItem> Items = new Dictionary<ItemType, GameItem>();

        public void TakeOn(GameItem newItem)
        {
            if (Items.ContainsKey(newItem.Type))
            {
                Items[newItem.Type] = newItem;
            }
            else
            {
                Items.Add(newItem.Type, newItem);
            }
        }

        public void TakeOff(GameItem item)
        {
            Items.Remove(item.Type);
        }

        public void TakeOffAll()
        {
            Items.Clear();
        }

        public float GetBonus(TypeCharacteristic type)
        {
            float result = 0f;
            foreach (var item in Items.Values)
                result += item.GetBonus(type);
            return result;
        }

        public GameCostumeHero Clone()
        {
            return new GameCostumeHero() { Items = this.Items };
        }

        public GameItem GetItem(ItemType cellType)
        {
            Items.TryGetValue(cellType, out var item);
            return item;
        }
    }
}