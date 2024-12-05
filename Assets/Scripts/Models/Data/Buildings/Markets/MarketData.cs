using System.Collections.Generic;

namespace Models.Data.Buildings.Markets
{
    public class MarketData
    {
        public List<PurchaseData> PurchaseDatas = new List<PurchaseData>();
        public List<Promotion> ShopPromotions = new();
    }
}
