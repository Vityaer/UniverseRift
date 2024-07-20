using ClientServices;
using Common.Resourses;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController
{
    public partial class ButtonCostController : UiView, IDisposable
    {
        private ResourceStorageController _resourceStorageController;

        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _mainImage;
        [SerializeField] private TypeDefaultMessage typeDefaultMessage = TypeDefaultMessage.Word;

        private GameResource _cost;
        private int countBuy = 1;
        private bool disable = false;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _subscriberResource;

        private ReactiveCommand<GameResource> _onClick = new ReactiveCommand<GameResource>();

        public IObservable<GameResource> OnClick => _onClick;
        
        [Inject]
        public void Construct(ResourceStorageController resourceStorageController)
        {
            _resourceStorageController = resourceStorageController;

            if(_cost != null)
                _subscriberResource = _resourceStorageController.Subscribe(_cost.Type, OnChageStorageResource);
        }

        void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => Click()).AddTo(_disposables);
        }

        public void SetCost(GameResource res)
        {
            _cost = res;
            Enable();

            if (_resourceStorageController == null)
                Debug.LogError("You forgot inject this!", gameObject);

            OnChageStorageResource(_resourceStorageController.Resources[_cost.Type]);
            CheckResource(_cost);
        }

        public void SetCostWithoutInfo(GameResource res)
        {
            _cost = res;
            CheckResource(res);
            Enable();
        }

        public void SetLabel(string text)
        {
            _cost?.Clear();
            _subscriberResource?.Dispose();
            _costText.text = text;
            disable = false;
            _button.interactable = true;
        }

        private void OnChageStorageResource(GameResource storageResource)
        {
            if (disable == false)
            {
                if (_cost.Amount.Mantissa > 0)
                {
                    if (_mainImage != null)
                    {
                        _costText.text = _cost.ToString();
                        _mainImage.enabled = true;
                        _mainImage.sprite = _cost.Image;
                    }
                }
                else
                {
                    if (typeDefaultMessage != TypeDefaultMessage.Number)
                        if (_mainImage != null)
                        {
                            _mainImage.enabled = false;
                            _costText.text = DefaultEmpty();
                        }
                }
                CheckResource(_cost);
            }
        }

        public void Click()
        {
            _onClick.Execute(_cost);
        }

        public void CheckResource(GameResource res)
        {
            if (disable)
                return;

            _button.interactable = _resourceStorageController.CheckResource(res);
        }

        public void Disable()
        {
            disable = true;
            _button.interactable = false;
            _subscriberResource.Dispose();
            _subscriberResource = null;
        }


        public void Enable()
        {
            if (_subscriberResource != null)
            {
                //Debug.LogError("try double subscribe");
                return;
            }

            disable = false;
            if(_resourceStorageController != null)
                _subscriberResource = _resourceStorageController.Subscribe(_cost.Type, OnChageStorageResource);
        }

        public void Clear()
        {
            _button.interactable = true;
            if (_mainImage != null)
            {
                _mainImage.enabled = false;
            }
            disable = false;
        }

        private string DefaultEmpty()
        {
            string result = string.Empty;
            switch (typeDefaultMessage)
            {
                case TypeDefaultMessage.Emtpy:
                    result = string.Empty;
                    break;
                case TypeDefaultMessage.Number:
                    result = "0";
                    break;
                case TypeDefaultMessage.Word:
                    result = "Бесплатно";
                    break;
            }
            return result;
        }

        public new void OnDestroy()
        {
            base.OnDestroy();
            _subscriberResource?.Dispose();
            _disposables?.Dispose();
        }

        public new void Dispose()
        {
            base.Dispose();
            _subscriberResource?.Dispose();
            _disposables?.Dispose();
        }
    }
}