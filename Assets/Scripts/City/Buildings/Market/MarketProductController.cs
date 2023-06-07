using Assets.Scripts.GeneralObject;
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
        public SubjectCellController cellProduct;
        private BaseObject _subject;

        private Action<string, int> callback = null;

        public void SetData(MarketProduct<Resource> product, Action<string, int> callback)
        {
            this.callback = callback;
            marketProduct = product;
            buttonCost.UpdateCost(product.Cost, Buy);
            cellProduct.SetItem(product.subject);
            UpdateUI();
            _subject = product.subject;
        }

        public void SetData(MarketProduct<Item> product, Action<string, int> callback)
        {
            this.callback = callback;
            marketProduct = product;
            buttonCost.UpdateCost(product.Cost, Buy);
            cellProduct.SetItem(product.subject);
            UpdateUI();
            _subject = product.subject;
        }

        public void SetData(MarketProduct<SplinterModel> product, Action<string, int> callback)
        {
            this.callback = callback;
            marketProduct = product;
            buttonCost.UpdateCost(product.Cost, Buy);
            cellProduct.SetItem(product.subject);
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
                buttonCost.EnableButton();
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