using Common;
using Models.Heroes.Characteristics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIController.Inventory
{
    [Serializable]
    public class Item : PlayerModel, VisualAPI
    {
        private static Sprite[] spriteAtlas;
        private static ItemsList itemsList;
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

        [SerializeField] public string Type;
        [SerializeField] public string Set;
        [SerializeField] public List<Bonus> ListBonuses;
        [SerializeField] public string Rarity;


        public void LoadData()
        {
            if (Id == string.Empty)
            {
                Debug.Log("ID item empty");
            }
            sprite = null;
            Item loadedItem = GetItem(Id);
            name = loadedItem.Id;
            Type = loadedItem.Type;
            Set = loadedItem.Set;
            ListBonuses = loadedItem.ListBonuses;
        }

        public static Item GetItem(string ID)
        {
            if (itemsList == null) itemsList = Resources.Load<ItemsList>("Items/ListItems");
            return itemsList.GetItem(ID);
        }
        //API

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

        //Visial API
        public override void ClickOnItem()
        {
            InventoryController.Instance.OpenInfoItem(this);
        }

        public override void SetUI(ThingUI UI)
        {
            this.UI = UI;
            UpdateUI();
        }

        public override void UpdateUI()
        {
            UI?.UpdateUI(Image, Amount);
        }

        //Constructors
        public Item(string ID, int amount = 1)
        {
            Amount = amount;
            Id = ID;
            LoadData();
        }

        public Item()
        {
        }

        public override BaseObject Clone()
        {
            return new Item(Id, Amount);
        }

        //Operators
        public static Item operator *(Item item, int k)
        {
            return new Item(item.Id, k);
        }
    }
}