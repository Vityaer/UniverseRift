using Common;
using Cysharp.Threading.Tasks;
using Models.Common;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.Observers
{
    public abstract class OtherObserver : UiView
    {
        private bool _isMyabeBuy = false;

        [Header("UI")]
        public GameObject btnAddResource;
        public Image imageObserver;
        public TextMeshProUGUI textObserver;

        protected CommonGameData CommonGameData;
        private IDisposable _disposable;

        [Inject]
        public void Construct(CommonGameData commonGameData)
        {
            CommonGameData = commonGameData;
        }

        protected override void Start()
        {
            btnAddResource.gameObject.SetActive(_isMyabeBuy);
            RegisterOnChange();
            UpdateUI();
        }

        protected abstract void RegisterOnChange();
        protected abstract void UpdateUI();

        public void OpenPanelForBuyResource()
        {
            // MarketProduct<Resource> product = null;
            // product = MarketResourceScript.Instance.GetProductFromTypeResource(resource.Name);
            // if(product != null)
            // 	PanelBuyResourceScript.StandartPanelBuyResource.Open(
            // 		product.subject, product.cost
            // 		);
        }

        protected override void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}