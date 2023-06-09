using Common.Resourses;
using UnityEngine;

namespace City.Buildings.Market
{
    [System.Serializable]
    public abstract class BaseMarketProduct
    {
        [SerializeField] private Resource cost;
        [SerializeField] private string id;
        [Min(1)][SerializeField] private int countMaxProduct = 1;
        private int countLeftProduct = 0;

        public string Id { get => id; }
        public int CountMaxProduct => countMaxProduct;
        public int CountLeftProduct => countLeftProduct;
        public Resource Cost => cost;

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
    }
}