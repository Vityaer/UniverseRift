using Common;
using Models;
using Models.Heroes.HeroCharacteristics;
using Models.Items;
using System;
using UnityEngine;

namespace UIController.Inventory
{
    [Serializable]
    public class GameItem : BaseObject
    {
        public ItemModel Model;

        public ItemType Type => Model.Type;

        private static Sprite[] m_spriteAtlas;
        public override Sprite Image
        {
            get
            {
                if (sprite == null || !Id.Equals(sprite.name))
                {
                    if (m_spriteAtlas == null) m_spriteAtlas = Resources.LoadAll<Sprite>("Items/Items");
                    LoadData();
                    for (int i = 0; i < m_spriteAtlas.Length; i++)
                    {
                        if (Id.Equals(m_spriteAtlas[i].name))
                        {
                            sprite = m_spriteAtlas[i];
                            break;
                        }
                    }
                }
                return sprite;
            }
        }

        public GameItem(ItemModel itemModel, int amount = 1)
        {
            Model = itemModel;
            Id = itemModel.Id;
            Amount = amount;
        }

        public GameItem(string ID, int amount = 1)
        {
            Amount = amount;
            Id = ID;
            LoadData();
        }

        public void LoadData()
        {
            if (Id == string.Empty)
            {
                Debug.Log("ID item empty");
            }
            sprite = null;
            //GameItem loadedItem = GetItem(Id);
            //Name = loadedItem.Id;
            //Type = loadedItem.Type;
            //Set = loadedItem.Set;
            //ListBonuses = loadedItem.ListBonuses;
        }

        public string GetTextBonuses()
        {
            var result = string.Empty;
            foreach (Bonus bonus in Model.Bonuses)
                result = string.Concat(result, GetText(bonus.Value, $"{Id}"));
            return result;
        }

        private string GetText(float bonus, string who)
        {
            var result = string.Empty;
            result = string.Concat(result, bonus > 0 ? "<color=green>+" : "<color=red>", Math.Round(bonus, 1).ToString(), "</color> ", who, "\n");
            return result;
        }

        public float GetBonus(TypeCharacteristic typeBonus)
        {
            var result = 0f;
            var bonus = Model.Bonuses.Find(x => x.Name == typeBonus);
            if (bonus != null)
                result = bonus.Value;

            return result;
        }

        public static GameItem operator *(GameItem item, int k)
        {
            var result = new GameItem(item.Id, k);
            result.Model = item.Model;
            return result;
        }
    }
}