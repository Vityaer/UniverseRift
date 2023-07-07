using Hero;
using Models.Data;
using Models.Items;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class CostumeData : BaseDataModel
    {
        public Dictionary<ItemType, string> Items;

        public void NewData(GameCostumeHero costume)
        {
            Items = new Dictionary<ItemType, string>(costume.Items.Count);

            foreach (var item in costume.Items)
            {
                Items.Add(item.Value.Type, item.Value.Id);
            }
        }
    }
}
