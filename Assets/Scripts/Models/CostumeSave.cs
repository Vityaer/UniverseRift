using Hero;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class CostumeSave
    {
        public List<string> ItemIds = new List<string>();
        public void NewData(CostumeHeroController costume)
        {
            ItemIds.Clear();
            foreach (var item in costume.items)
            {
                ItemIds.Add(item.Id);
            }
        }
    }
}
