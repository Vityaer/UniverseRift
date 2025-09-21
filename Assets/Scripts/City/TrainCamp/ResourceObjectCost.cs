using ClientServices;
using Common.Resourses;
using System;
using Common.Inventories.Resourses;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainerUi.Abstraction;

namespace City.TrainCamp
{
    public class ResourceObjectCost : UiView
    {
        [SerializeField] private Image Image;
        [SerializeField] private TextMeshProUGUI TextAmount;

        private ResourceStorageController m_resourceStorageController;
        private GameResource m_costResource;
        private IDisposable m_disposable;
        private ReactiveCommand<bool> m_observerCanBuy = new ReactiveCommand<bool>();

        public IObservable<bool> ObserverCanBuy => m_observerCanBuy;

        [Inject]
        public void Construct(ResourceStorageController resourceStorageController)
        {
            m_resourceStorageController = resourceStorageController;
        }

        public void SetData(GameResource res)
        {
            if (m_resourceStorageController == null)
                Debug.LogError($"You forgot inject ResourceStorageController, {gameObject.name}", gameObject);

            m_disposable?.Dispose();
            m_disposable = m_resourceStorageController.Subscribe(res.Type, CheckResource);

            m_costResource = res;
            CheckResource();
            Image.sprite = m_costResource.Image;
            gameObject.SetActive(true);
        }

        public void CheckResource(GameResource res)
        {
            CheckResource();
        }

        public bool CheckResource()
        {
            var storeResource = m_resourceStorageController.GetResource(m_costResource.Type);
            bool enoughResource = storeResource.CheckCount(m_costResource);
            string color = enoughResource ? "<color=green>" : "<color=red>";

            string result = $"{color}{m_costResource}</color>/{storeResource}";
            TextAmount.text = result;
            OnCheckResource(enoughResource);
            return enoughResource;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnCheckResource(bool check)
        {
            m_observerCanBuy?.Execute(check);
        }

        protected override void OnDestroy()
        {
            m_disposable?.Dispose();
            base.OnDestroy();
        }
    }
}