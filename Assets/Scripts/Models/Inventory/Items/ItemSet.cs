using System.Collections.Generic;
using UIController.Inventory;

namespace Models
{
    public class ItemSet : BaseModel
    {
        public string Name;
        public int Rating;
        public List<Bonus> ExtraBonuses = new List<Bonus>();
    }
}
