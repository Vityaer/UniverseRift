using ClientServices;
using Common.Resourses;
using LocalizationSystems;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController
{
    public class ButtonCostController : UiView
    {
        private ResourceStorageController _resourceStorageController;
        private ILocalizationSystem _localizationSystem;

        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _mainImage;
        [SerializeField] private TypeDefaultMessage typeDefaultMessage = TypeDefaultMessage.Word;

        private GameResource _cost;
        private bool _disable = false;
        private CompositeDisposable _disposables = new();
        private IDisposable _subscriberResource;
        private ReactiveCommand<GameResource> _onClick = new();

        public IObservable<GameResource> OnClick => _onClick;
        public Button Button => _button;
        
        [Inject]
        public void Construct(ResourceStorageController resourceStorageController, ILocalizationSystem localizationSystem)
        {
            _resourceStorageController = resourceStorageController;
            _localizationSystem = localizationSystem;
            TrySubscribe();
        }

        private void TrySubscribe()
        {
            _subscriberResource?.Dispose();
            
            if (_cost != null)
                _subscriberResource = _resourceStorageController.Subscribe(_cost.Type, OnChangeStorageResource);
        }

        protected override void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => Click()).AddTo(_disposables);
        }

        public void SetCost(GameResource res)
        {
            _cost = res;
            Enable();

            if (_resourceStorageController == null)
            {
                Debug.LogError("You forgot inject this!", gameObject);
            }

            OnChangeStorageResource(_resourceStorageController.Resources[_cost.Type]);
            CheckResource(_cost);
            TrySubscribe();
        }

        public void SetCostWithoutInfo(GameResource res)
        {
            _cost = res;
            CheckResource(res);
            Enable();
            TrySubscribe();
        }

        public void SetLabel(string text)
        {
            _cost?.Clear();
            _subscriberResource?.Dispose();
            _subscriberResource = null;
            _costText.text = text;
            _disable = false;
            _button.interactable = true;
        }

        private void OnChangeStorageResource(GameResource storageResource)
        {
            if (_disable == false)
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

        private void Click()
        {
            _onClick.Execute(_cost);
        }

        private void CheckResource(GameResource res)
        {
            if (_disable)
                return;

            var result = _resourceStorageController.CheckResource(res);
            _button.interactable = _resourceStorageController.CheckResource(res);
        }

        public void Disable()
        {
            _disable = true;
            _button.interactable = false;
            _subscriberResource?.Dispose();
            _subscriberResource = null;
        }


        public void Enable()
        {
            if (_subscriberResource != null)
            {
                //Debug.LogError("try double subscribe");
                return;
            }

            _disable = false;
            TrySubscribe();
        }

        public void Clear()
        {
            _button.interactable = true;
            if (_mainImage != null)
            {
                _mainImage.enabled = false;
            }
            _disable = false;
            
            _subscriberResource?.Dispose();
            _subscriberResource = null;
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
                    result = _localizationSystem.GetString("ButtonCostFreeLabel");
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
    }
}