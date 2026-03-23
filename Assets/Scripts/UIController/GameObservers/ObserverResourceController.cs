using ClientServices;
using Common.Resourses;
using System;
using System.Collections.Generic;
using Common.Inventories.Resourses;
using DG.Tweening;
using Models.Common.BigDigits;
using TMPro;
using UIController.ControllerPanels.MarketResources;
using UIController.Observers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UIController.GameObservers
{
    public class ObserverResourceController : BaseObserverUi
    {
        [Inject] private ResourceStorageController m_resourceStorageController;
        [Inject] private BuyResourcePanelController m_buyResourcePanelController;

        [Header("General")]
        public ResourceType TypeResource;
        public float CountDuration;
        
        private GameResource _resource;
        private bool _isMyabeBuy;

        [Header("UI")]
        public GameObject btnAddResource;
        public Image imageResource;
        public TextMeshProUGUI countResource;
        public Button ButtonBuyResource;
        private IDisposable m_disposable;

        private BigDigit? m_previousBigDigit;
        private Tween m_counterTween;
        
        protected override void Start()
        {
            if (m_buyResourcePanelController == null)
            {
                string path = string.Empty;

                List<Transform> pathObjects = new();
                Transform current = transform;
                while (current != null)
                {
                    pathObjects.Add(current);
                    current = current.parent;
                    
                }

                for (var i = pathObjects.Count - 1; i > 0; i--)
                {
                    path = string.Concat(path, pathObjects[i].name, "/");
                }
                
                path = string.Concat(path, pathObjects[0].name);

                Debug.LogError($"Object: {path} {nameof(m_buyResourcePanelController)} is null");
                return;
            }
            
            _isMyabeBuy = m_buyResourcePanelController.GetCanSellThisResource(TypeResource);
            _resource = new GameResource(TypeResource);
            imageResource.sprite = _resource.Image;
            btnAddResource.SetActive(_isMyabeBuy);
            ButtonBuyResource.OnClickAsObservable().Subscribe(_ => OpenPanelForBuyResource()).AddTo(Disposables);
        }

        [Inject]
        public override void Construct()
        {
            if (m_disposable != null)
            {
                m_disposable.Dispose();
                m_disposable = null;
            }

            m_disposable = m_resourceStorageController.Subscribe(TypeResource, UpdateUI);
            UpdateUI(m_resourceStorageController.GetResource(TypeResource));
        }

        private void UpdateUI(GameResource res)
        {
            _resource = res;
            
            if (m_previousBigDigit == null)
            {
                m_previousBigDigit = res.Amount;
                countResource.text = _resource.ToString();
                return;
            }

            if (gameObject.activeInHierarchy || (m_previousBigDigit.E10 != res.Amount.E10))
            {
                countResource.text = _resource.ToString();
                return;
            }

            m_counterTween?.Kill();
            int startValue = Mathf.FloorToInt(m_previousBigDigit.Mantissa);
            int targetValue = Mathf.FloorToInt(_resource.Amount.Mantissa);

            int value = startValue;
            m_counterTween = DOTween.To(() => startValue, x => value = x, targetValue, CountDuration)
                .OnUpdate(() =>
                {
                    countResource.text = new BigDigit(value, m_previousBigDigit.E10).ToString();
                });

        }

        public void OpenPanelForBuyResource()
        {
            m_buyResourcePanelController.Open(TypeResource);
        }

        public override void Dispose()
        {
            m_counterTween?.Kill();
            m_disposable?.Dispose();
            base.Dispose();
        }
    }
}