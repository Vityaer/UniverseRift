using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Sirenix.Serialization;
using System;
using UIController;
using UIController.Inventory;
using UIController.ItemVisual;
using UnityEngine;

namespace City.Buildings.Market
{
    public class MarketProductController : MonoBehaviour
    {
        [OdinSerialize] private BaseMarketProduct marketProduct;
        [SerializeField] private GameObject soldOutPanel;

        public ButtonCostController buttonCost;
        public SubjectCell CellProduct;
        private BaseObject _subject;

        private Action<string, int> callback = null;

        public void SetData(MarketProduct<GameResource> product, Action<string, int> callback)
        {
            this.callback = callback;
            marketProduct = product;
            //buttonCost.UpdateCost(product.Cost, Buy);
            //CellProduct.SetItem(product.subject);
            UpdateUI();
            _subject = product.subject;
        }

        public void SetData(MarketProduct<GameItem> product, Action<string, int> callback)
        {
            this.callback = callback;
            marketProduct = product;
            //buttonCost.UpdateCost(product.Cost, Buy);
            //CellProduct.SetItem(product.subject);
            UpdateUI();
            _subject = product.subject;
        }

        public void SetData(MarketProduct<GameSplinter> product, Action<string, int> callback)
        {
            this.callback = callback;
            marketProduct = product;
            //buttonCost.UpdateCost(product.Cost, Buy);
            //CellProduct.SetItem(product.subject);
            UpdateUI();
            _subject = product.subject;
        }

        public void UpdateUI()
        {
            if (marketProduct.CountLeftProduct == marketProduct.CountMaxProduct)
            {
                buttonCost.Disable();
                soldOutPanel.SetActive(true);
            }
            else
            {
                soldOutPanel.SetActive(false);
                buttonCost.Enable();
            }
        }

        public void Recovery()
        {
            marketProduct.Recovery();
            UpdateUI();
        }

        public void Buy(int count = 1)
        {
            if (count + marketProduct.CountLeftProduct > marketProduct.CountMaxProduct)
                count = marketProduct.CountMaxProduct - marketProduct.CountLeftProduct;
            marketProduct.GetProduct(count);

            UpdateUI();
            if (callback != null)
                callback(marketProduct.Id, marketProduct.CountLeftProduct);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}