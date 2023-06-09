using System.Collections.Generic;

namespace Models.City.Markets
{
    [System.Serializable]
    public class MarketModel : BaseModel
    {
        public MarketType Type;
        public List<ProductModel> Products = new List<ProductModel>();

        public MarketModel() { }
    }
}
