using Common.Resourses;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.Market
{
    public class MarketResourceController : MonoBehaviour
    {
        [Header("Resource")]
        public List<MarketProduct<GameResource>> resources = new List<MarketProduct<GameResource>>();
        private static MarketResourceController instance;
        public static MarketResourceController Instance { get => instance; }

        public MarketProduct<GameResource> GetProductFromTypeResource(ResourceType name)
        {
            return resources.Find(x => (x.subject as GameResource).Type == name);
        }

        public bool GetCanSellThisResource(ResourceType name)
        {
            return resources.Find(x => (x.subject as GameResource).Type == name) != null;
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