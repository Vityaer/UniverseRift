using Common.Resourses;
using Models.City.Markets;
using System;
using Common.Inventories.Resourses;
using UnityEngine;

namespace City.Buildings.Market
{
    [System.Serializable]
    public abstract class BaseMarketProduct
    {
        protected GameResource cost;
        protected string id;
        protected int countMaxProduct = 1;
        private int countLeftProduct = 0;

        public string Id { get => id; }
        public int CountMaxProduct => countMaxProduct;
        public int CountLeftProduct => countLeftProduct;
        public GameResource Cost => cost;

        protected void AddCountLeftProduct(int count = 1)
        {
            countLeftProduct += count;
        }

        public void UpdateData(int newCountLeftProduct = 1)
        {
            countLeftProduct = newCountLeftProduct;
        }

        public void Recovery()
        {
            countLeftProduct = 0;
        }

        public virtual void GetProduct(int count) { }

        public void SetPurchaseCount(int purchaseCount)
        {
            countLeftProduct = purchaseCount;
        }
    }
}