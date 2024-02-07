using ClientServices;
using Common.Resourses;
using System;
using TMPro;
using UIController.Observers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UIController.GameObservers
{
    public class ObserverResourceController : BaseObserverUi
    {
        [Inject] private ResourceStorageController _resourceStorageController;
        [Inject] private BuyResourcePanelController _buyResourcePanelController;

        [Header("General")]
        public ResourceType TypeResource;
        public int Cost;

        private GameResource _resource;
        private bool _isMyabeBuy;

        [Header("UI")]
        public GameObject btnAddResource;
        public Image imageResource;
        public TextMeshProUGUI countResource;
        public Button ButtonBuyResource;
        private IDisposable _disposable;

        protected override void Start()
        {
            _isMyabeBuy = _buyResourcePanelController.GetCanSellThisResource(TypeResource);
            _resource = new GameResource(TypeResource);
            imageResource.sprite = _resource.Image;
            btnAddResource.SetActive(_isMyabeBuy);
            ButtonBuyResource.OnClickAsObservable().Subscribe(_ => OpenPanelForBuyResource()).AddTo(Disposables);
        }

        [Inject]
        public override void Construct()
        {
            if (_disposable != null)
            {
                _disposable.Dispose();
                _disposable = null;
            }

            _disposable = _resourceStorageController.Subscribe(TypeResource, UpdateUI);
            UpdateUI(_resourceStorageController.GetResource(TypeResource));
        }

        private void UpdateUI(GameResource res)
        {
            _resource = res;
            countResource.text = _resource.ToString();
        }

        public void OpenPanelForBuyResource()
        {
            _buyResourcePanelController.Open(TypeResource);
        }

        public override void Dispose()
        {
            _disposable?.Dispose();
            base.Dispose();
        }
    }
}