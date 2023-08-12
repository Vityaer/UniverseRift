using Models.Data.Inventories;
using Newtonsoft.Json;

namespace Models.City.Markets
{
    [System.Serializable]
    public class BaseProductModel : BaseModel
    {
        public int CountSell;
        public ResourceData Cost;
        public MarketProductType Type;
        
        [JsonIgnore] public InventoryBaseItem Subject { get; }
    }
}
