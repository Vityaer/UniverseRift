using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using VContainerUi.Interfaces;

namespace City.Buildings.UiBuildings
{
    public class BuildingVisual : MonoBehaviour
    {
        public GameObject News;
        public SpriteRenderer ViewRenderer;
        public Button BuildingButton;
        public RectTransform TextBackground;
        public RectTransform TextRect;
        public Vector2 BackgroundOffset;
        public LocalizeStringEvent BuildingName;

        private CompositeDisposable _disposables = new();

        private void Start()
        {
            CheckTextBackgroundSize();
            LocalizationSettings.SelectedLocaleChanged += ChangeLocale;
            TextRect.OnRectTransformDimensionsChangeAsObservable()
                .Subscribe(_ => CheckTextBackgroundSize())
                .AddTo(_disposables);
        }

        private void ChangeLocale(Locale locale)
        {
            CheckTextBackgroundSize();
        }

        [ContextMenu("CheckTextBackgroundSize")]
        private void CheckTextBackgroundSize()
        {
            TextBackground.sizeDelta = TextRect.rect.size + BackgroundOffset;
        }

        private void ShowNews()
        {
            News.SetActive(true);
        }

        private void HideNews()
        {
            News.SetActive(false);
        }

        public void SubscribeOnNews<V>(V buildingController) where V : IPopUp
        {
            buildingController.OnNewsStatusChange.Subscribe(OnChangeNews).AddTo(_disposables);
        }

        private void OnChangeNews(bool flag)
        {
            if (flag)
            {
                ShowNews();
            }
            else
            {
                HideNews();
            }
        }

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= ChangeLocale;
            _disposables.Dispose();
        }
    }
}
