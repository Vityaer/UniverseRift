using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Interfaces;

namespace City.Buildings.UiBuildings
{
    public class BuildingVisual : MonoBehaviour, IDisposable
    {
        public GameObject News;
        public Button BuildingButton;

        private CompositeDisposable _disposables = new CompositeDisposable();

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
            buildingController.OnNews.Subscribe(OnChangeNews).AddTo(_disposables);
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

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
