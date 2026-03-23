using System.Collections.Generic;

namespace Models.Inventory.Splinters
{
    [System.Serializable]
    public class SplinterModel : BaseModel
    {
        public string ImageName;
        public SplinterType SplinterType;
        public List<RandomSplinterModel> ModelIds = new();
        public int RequireCount;
    }
}