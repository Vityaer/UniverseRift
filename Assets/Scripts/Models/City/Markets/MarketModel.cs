using City.Buildings.Market;
using System.Collections.Generic;

namespace Models.City.Markets
{
    [System.Serializable]
    public class MarketModel : BaseModel
    {
        public TypeMarket TypeMarket;
        public List<ProductModel> Products = new List<ProductModel>();

        public MarketModel() { }
    }
}
