﻿using Common;
using Db.CommonDictionaries;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UIController.Inventory;

namespace Models.Data.Inventories
{
    [System.Serializable]
    public class ItemData : InventoryBaseItem
    {
        [NonSerialized] public CommonDictionaries CommonDictionaries;
        private string[] _allItemName => CommonDictionaries.Items.Values.Select(r => r.Id).ToArray();
        [ValueDropdown(nameof(_allItemName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Id;

        public int Amount;

        public ItemData() { }

        public ItemData(GameItem item)
        {
            Id = item.Id;
            Amount = item.Amount;
        }

        public ItemData(CommonDictionaries dictionaries)
        {
            CommonDictionaries = dictionaries;
        }

        public override BaseObject CreateGameObject()
        {
            return new GameItem(Id, Amount);
        }
    }
}