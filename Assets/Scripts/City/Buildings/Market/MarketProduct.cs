using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Sirenix.Serialization;
using UIController.Inventory;
using UnityEngine;

namespace City.Buildings.Market
{
    [System.Serializable]
    public class MarketProduct<T> : BaseMarketProduct where T : BaseObject
    {
        [OdinSerialize] public T subject;
        public override void GetProduct(int count)
        {
            AddCountLeftProduct(count);
            switch (subject)
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
