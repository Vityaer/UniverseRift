using Models;

namespace Models.Inventory.Splinters
{
    [System.Serializable]
    public class SplinterModel : BaseModel
    {
        public SplinterType SplinterType;
        public string ModelId;
        public int RequireCount;
    }
}
