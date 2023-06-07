using Models.City.Markets;
using System;
using System.Collections.Generic;

namespace Models
{
    //Markets
    [Serializable]
    public class ShopModel : BaseModel
    {
        public List<MarketModel> markets = new List<MarketModel>();
    }
}
