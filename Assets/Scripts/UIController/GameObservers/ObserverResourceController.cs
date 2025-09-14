using ClientServices;
using Common.Resourses;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Models.Common.BigDigits;
using TMPro;
using UIController.Observers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
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
        public float CountDuration;
        
        private GameResource _resource;
        private bool _isMyabeBuy;

        [Header("UI")]
        public GameObject btnAddResource;
        public Image imageResource;
        public TextMeshProUGUI countResource;
        public Button ButtonBuyResource;
        private IDisposable _disposable;

        private BigDigit? _previousBigDigit;
        private Tween _counterTween;
        
        protected override void Start()
        {
            if (_buyResourcePanelController == null)
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

                Debug.LogError($"Object: {path} {nameof(_buyResourcePanelController)} is null");
                return;
            }
            
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
            
            if (_previousBigDigit == null)
            {
                _previousBigDigit = res.Amount;
                countResource.text = _resource.ToString();
                return;
            }

            if (gameObject.activeInHierarchy || (_previousBigDigit.E10 == res.Amount.E10))
            {
                _counterTween.Kill();
                int startValue = Mathf.FloorToInt(_previousBigDigit.Mantissa);
                int targetValue = Mathf.FloorToInt(_resource.Amount.Mantissa);

                int value = startValue;
                _counterTween = DOTween.To(() => startValue, x => value = x, targetValue, CountDuration)
                    .OnUpdate(() =>
                    {
                        countResource.text = new BigDigit(value, _previousBigDigit.E10).ToString();
                    });
            }
            else
            {
                countResource.text = _resource.ToString();
            }
        }

        public void OpenPanelForBuyResource()
        {
            _buyResourcePanelController.Open(TypeResource);
        }

        public override void Dispose()
        {
            _counterTween.Kill();
            _disposable?.Dispose();
            base.Dispose();
        }
    }
}