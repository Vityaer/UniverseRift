using Common.Resourses;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.Market
{
    public class MarketResourceController : MonoBehaviour
    {
        [Header("Resource")]
        public List<MarketProduct<Resource>> resources = new List<MarketProduct<Resource>>();
        private static MarketResourceController instance;
        public static MarketResourceController Instance { get => instance; }

        public MarketProduct<Resource> GetProductFromTypeResource(TypeResource name)
        {
            return resources.Find(x => (x.subject as Resource).Name == name);
        }

        public bool GetCanSellThisResource(TypeResource name)
        {
            return resources.Find(x => (x.subject as Resource).Name == name) != null;
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.Log("MarketResourceScript twice");
            }
        }
    }
}