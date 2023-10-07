using Models.City.Markets;

namespace UIController.ControllerPanels.MarketResources
{
    public class ResourceProductContainer
    {
        public ResourceProductModel ResourceProduct;
        public int PurchaseCount;
        public int MaxCount;

        public ResourceProductContainer(ResourceProductModel resourceProduct, int purchaseCount, int maxCount)
        {
            ResourceProduct = resourceProduct;
            PurchaseCount = purchaseCount;
            MaxCount = maxCount;
        }
    }
}
