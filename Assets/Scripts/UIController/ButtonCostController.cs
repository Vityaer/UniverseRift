using ClientServices;
using Common.Resourses;
using LocalizationSystems;
using System;
using Common.Inventories.Resourses;
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
        private ResourceStorageController m_resourceStorageController;
        private ILocalizationSystem m_localizationSystem;

        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _mainImage;
        [SerializeField] private TypeDefaultMessage typeDefaultMessage = TypeDefaultMessage.Word;

        private GameResource m_cost;
        private bool m_disable = false;
        private CompositeDisposable m_disposables = new();
        private IDisposable m_subscriberResource;
        private ReactiveCommand<GameResource> m_onClick = new();

        public IObservable<GameResource> OnClick => m_onClick;
        public Button Button => _button;
        
        [Inject]
        public void Construct(ResourceStorageController resourceStorageController, ILocalizationSystem localizationSystem)
        {
            m_resourceStorageController = resourceStorageController;
            m_localizationSystem = localizationSystem;
            TrySubscribe();
        }

        private void TrySubscribe()
        {
            m_subscriberResource?.Dispose();
            
            if (m_cost != null)
                m_subscriberResource = m_resourceStorageController.Subscribe(m_cost.Type, OnChangeStorageResource);
        }

        protected override void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => Click()).AddTo(m_disposables);
        }

        public void SetCost(GameResource res)
        {
            m_cost = res;
            Enable();

            if (m_resourceStorageController == null)
            {
                Debug.LogError("You forgot inject this!", gameObject);
            }

            OnChangeStorageResource(m_resourceStorageController.Resources[m_cost.Type]);
            CheckResource(m_cost);
            TrySubscribe();
        }

        public void SetCostWithoutInfo(GameResource res)
        {
            m_cost = res;
            CheckResource(res);
            Enable();
            TrySubscribe();
        }

        public void SetLabel(string text)
        {
            m_cost?.Clear();
            m_subscriberResource?.Dispose();
            m_subscriberResource = null;
            _costText.text = text;
            m_disable = false;
            _button.interactable = true;
        }

        private void OnChangeStorageResource(GameResource storageResource)
        {
            if (m_disable == false)
            {
                if (m_cost.Amount.Mantissa > 0)
                {
                    if (_mainImage != null)
                    {
                        _costText.text = m_cost.ToString();
                        _mainImage.enabled = true;
                        _mainImage.sprite = m_cost.Image;
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
                CheckResource(m_cost);
            }
        }

        private void Click()
        {
            m_onClick.Execute(m_cost);
        }

        private void CheckResource(GameResource res)
        {
            if (m_disable)
                return;

            var result = m_resourceStorageController.CheckResource(res);
            _button.interactable = m_resourceStorageController.CheckResource(res);
        }

        public void Disable()
        {
            m_disable = true;
            _button.interactable = false;
            m_subscriberResource?.Dispose();
            m_subscriberResource = null;
        }


        public void Enable()
        {
            if (m_subscriberResource != null)
            {
                //Debug.LogError("try double subscribe");
                return;
            }

            m_disable = false;
            TrySubscribe();
        }

        public void Clear()
        {
            _button.interactable = true;
            if (_mainImage != null)
            {
                _mainImage.enabled = false;
            }
            m_disable = false;
            
            m_subscriberResource?.Dispose();
            m_subscriberResource = null;
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
                    result = m_localizationSystem.GetString("ButtonCostFreeLabel");
                    break;
            }
            return result;
        }

        public new void OnDestroy()
        {
            base.OnDestroy();
            m_subscriberResource?.Dispose();
            m_disposables?.Dispose();
        }
    }
}