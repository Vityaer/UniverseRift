using Models.Items;
using System.Collections.Generic;
using UIController.Inventory;

namespace Models
{
    [System.Serializable]
    public class ItemModel : BaseModel
    {
        public ItemType Type;
        public string SetName;
        public List<Bonus> Bonuses;
        public string Rating;
    }
}
