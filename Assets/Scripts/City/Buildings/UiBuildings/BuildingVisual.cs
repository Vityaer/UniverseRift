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
        public Button BuildingButton;
        public RectTransform TextBackground;
        public RectTransform TextRect;
        public Vector2 BackgroundOffset;

        private CompositeDisposable m_disposables = new();

        private void Start()
        {
            CheckTextBackgroundSize();
            LocalizationSettings.SelectedLocaleChanged += ChangeLocale;
            TextRect.OnRectTransformDimensionsChangeAsObservable()
                .Subscribe(_ => CheckTextBackgroundSize())
                .AddTo(m_disposables);
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

        public void SubscribeOnNews<TV>(TV buildingController) where TV : IPopUp
        {
            buildingController.OnNewsStatusChange.Subscribe(OnChangeNews).AddTo(m_disposables);
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
            m_disposables.Dispose();
        }
    }
}
