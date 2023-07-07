using Common;
using Models.Heroes.HeroCharacteristics;
using Models.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace UIController.Inventory
{
    [Serializable]
    public class GameItem : BaseObject
    {
        [SerializeField] public ItemType Type;
        [SerializeField] public string Set;
        [SerializeField] public List<Bonus> ListBonuses;
        [SerializeField] public string Rarity;

        private static Sprite[] spriteAtlas;
        public override Sprite Image
        {
            get
            {
                if (sprite == null || !Id.Equals(sprite.name))
                {
                    if (spriteAtlas == null) spriteAtlas = Resources.LoadAll<Sprite>("Items/Items");
                    //LoadData();
                    for (int i = 0; i < spriteAtlas.Length; i++)
                    {
                        if (Id.Equals(spriteAtlas[i].name))
                        {
                            sprite = spriteAtlas[i];
                            break;
                        }
                    }
                }
                return sprite;
            }
        }

        public GameItem()
        {
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
            string result = string.Empty;
            foreach (Bonus bonus in ListBonuses)
                result = string.Concat(result, GetText(bonus.Count, $"{Id}"));
            return result;
        }

        private string GetText(float bonus, string who)
        {
            string result = string.Empty;
            result = string.Concat(result, bonus > 0 ? "<color=green>+" : "<color=red>", Math.Round(bonus, 1).ToString(), "</color> ", who, "\n");
            return result;
        }

        public float GetBonus(TypeCharacteristic typeBonus)
        {
            float result = 0;
            Bonus bonus = ListBonuses.Find(x => x.Name == typeBonus);
            if (bonus != null) result = bonus.Count;
            return result;
        }

        public static GameItem operator *(GameItem item, int k)
        {
            return new GameItem(item.Id, k);
        }
    }
}