using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Models.City.Markets;
using Sirenix.Serialization;
using UIController.Inventory;
using UnityEngine;

namespace City.Buildings.Market
{
    [System.Serializable]
    public class MarketProduct<T> : BaseMarketProduct where T : BaseObject
    {
        [OdinSerialize] public T Subject;

        public MarketProduct(BaseProductModel productModel, T subject)
        {
            cost = new GameResource(productModel.Cost);
            countMaxProduct = productModel.CountSell;
            Subject = subject;
        } 

        public override void GetProduct(int count)
        {
            AddCountLeftProduct(count);
            switch (Subject)
            {
                case GameResource product:
                    //GameController.Instance.AddResource(product * count);
                    break;
                case GameItem product:
                    //InventoryController.Instance.AddItem(product);
                    break;
                case GameSplinter product:
                    Debug.Log("write add splinter here");
                    // InventoryControllerScript.Instance.AddSplinter(product);
                    break;
            }
        }
    }
}
