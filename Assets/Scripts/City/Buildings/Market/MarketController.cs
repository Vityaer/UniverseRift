using City.Buildings.Abstractions;
using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Models;
using Models.City.Markets;
using Models.Common;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UIController.GameSystems;
using UIController.Inventory;
using UnityEngine;
using VContainer;

namespace City.Buildings.Market
{
    public class MarketController : BaseBuilding<MarketView>
    {
        [Inject] private readonly CommonGameData _сommonGameData;

        public MarketType typeMarket;
        public Transform showcase;
        [Header("Products")]
        [OdinSerialize] private List<BaseMarketProduct> productsForSale = new List<BaseMarketProduct>();
        [SerializeField] private List<MarketProductController> productControllers = new List<MarketProductController>();
        public CycleRecover cycle;
        private bool productFill = false;

        protected override void OnStart()
        {
            //if (productControllers.Count == 0) GetCells();
            //TimeControllerSystem.Instance.RegisterOnNewCycle(RecoveryAllProducts, cycle);
        }

        protected override void OnLoadGame()
        {
            //List<ProductModel> saveProducts = _сommonGameData.City.GetProductForMarket(typeMarket);
            //BaseMarketProduct currentProduct = null;
            //foreach (ProductModel product in saveProducts)
            //{
            //    currentProduct = productsForSale.Find(x => x.Id == product.Id);
            //    if (currentProduct != null) currentProduct.UpdateData(product.CountSell);
            //}
            //if (productFill)
            //    UpdateUIProducts();
        }

        private void OnBuyPoduct(string IDproduct, int coutSell)
        {
            //GameController.GetPlayerGame.NewDataAboutSellProduct(typeMarket, IDproduct, coutSell);
        }

        private void UpdateUIProducts()
        {
            foreach (MarketProductController product in productControllers)
            {
                product.UpdateUI();
            }
        }

        protected override void OpenPage()
        {
            for (int i = 0; i < productsForSale.Count; i++)
            {
                switch (productsForSale[i])
                {
                    case MarketProduct<GameResource> product:
                        productControllers[i].SetData(product, OnBuyPoduct);
                        break;
                    case MarketProduct<GameItem> product:
                        productControllers[i].SetData(product, OnBuyPoduct);
                        break;
                    case MarketProduct<GameSplinter> product:
                        productControllers[i].SetData(product, OnBuyPoduct);
                        break;

                }
            }
            for (int i = productsForSale.Count; i < productControllers.Count; i++)
            {
                productControllers[i].Hide();
            }
            productFill = true;
        }
        private void GetCells()
        {
            foreach (Transform child in showcase)
                productControllers.Add(child.GetComponent<MarketProductController>());
        }

        private void RecoveryAllProducts() { if (productFill) foreach (MarketProductController product in productControllers) { product.Recovery(); } }

        public void NewSellProduct(string IDproduct, int newCountSell)
        {
            //GameController.GetPlayerGame.NewDataAboutSellProduct(typeMarket, IDproduct, newCountSell);
        }

        [Button] public void AddResource() { productsForSale.Add(new MarketProduct<GameResource>()); }
        [Button] public void AddSplinter() { productsForSale.Add(new MarketProduct<GameSplinter>()); }
        [Button] public void AddItem() { productsForSale.Add(new MarketProduct<GameItem>()); }

        [ContextMenu("Check products")]
        private void CheckProducts()
        {
            for (int i = 0; i < productsForSale.Count - 1; i++)
            {
                for (int j = i + 1; j < productsForSale.Count; j++)
                {
                    if (productsForSale[i].Id == productsForSale[j].Id)
                    {
                        Debug.Log(string.Concat("product: ", i.ToString(), " and ", j.ToString(), " have equals ID"));
                    }
                }
            }
        }
    }
}