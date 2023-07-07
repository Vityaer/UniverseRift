using System.Collections.Generic;
using UIController.Inventory;

namespace Models
{
    public class ItemSet : BaseModel
    {
        public string Name;
        public List<Bonus> ExtraBonuses;
    }
}
