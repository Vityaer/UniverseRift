using City.Buildings.Market;
using Common;
using Common.Resourses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UniRx;
using Assets.Scripts.ClientServices;

namespace UIController.Observers
{
    public class ObserverResourceController : BaseObserverUi
    {
        [Inject] private ResourceStorageController _resourceStorageController;
        [Inject] private BuyResourcePanelController _buyResourcePanelController;

        [Header("General")]
        public ResourceType typeResource;
        private bool isMyabeBuy;
        public int cost;
        private GameResource resource;

        [Header("UI")]
        public GameObject btnAddResource;
        public Image imageResource;
        public TextMeshProUGUI countResource;

        protected override void Start()
        {
            //isMyabeBuy = MarketResourceController.Instance.GetCanSellThisResource(typeResource);
            resource = new GameResource(typeResource);
            imageResource.sprite = resource.Image;
            btnAddResource.SetActive(isMyabeBuy);
        }

        [Inject]
        public override void Construct()
        {
            _resourceStorageController.Subscribe(typeResource, UpdateUI);
            UpdateUI(_resourceStorageController.GetResource(typeResource));
        }

        public void UpdateUI(GameResource res)
        {
            resource = res;
            countResource.text = resource.ToString();
        }

        public void OpenPanelForBuyResource()
        {
            //MarketProduct<GameResource> product = null;
            //product = MarketResourceController.Instance.GetProductFromTypeResource(resource.Type);
            //if (product != null)
            //    _buyResourcePanelController.Open(
            //        product.Subject, product.Cost
            //        );
        }

    }
}