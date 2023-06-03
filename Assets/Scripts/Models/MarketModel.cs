using City.Buildings.Market;
using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class MarketModel : BaseModel
    {
        public TypeMarket typeMarket;
        public List<MarketProductModel> products = new List<MarketProductModel>();
    }
}
