using Assets.Scripts.GeneralObject;
using Common;
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
                case Resource product:
                    GameController.Instance.AddResource(product * count);
                    break;
                case Item product:
                    InventoryController.Instance.AddItem(product);
                    break;
                case SplinterModel product:
                    Debug.Log("write add splinter here");
                    // InventoryControllerScript.Instance.AddSplinter(product);
                    break;
            }
        }
    }
}
