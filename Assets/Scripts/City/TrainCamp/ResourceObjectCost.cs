using Assets.Scripts.ClientServices;
using Common.Resourses;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.TrainCamp
{
    public class ResourceObjectCost : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image Image;
        [SerializeField] private TextMeshProUGUI TextAmount;

        private ResourceStorageController _resourceStorageController;
        private GameResource _costResource;
        private IDisposable _disposable;
        private ReactiveCommand<bool> _observerCanBuy = new ReactiveCommand<bool>();

        public IObservable<bool> ObserverCanBuy => _observerCanBuy;

        [Inject]
        public void Construct(ResourceStorageController resourceStorageController)
        {
            _resourceStorageController = resourceStorageController;
        }

        public void SetData(GameResource res)
        {
            _disposable?.Dispose();
            _disposable = _resourceStorageController.Subscribe(res.Type, CheckResource);

            _costResource = res;
            CheckResource();
            Image.sprite = _costResource.Image;
            gameObject.SetActive(true);
        }

        public void CheckResource(GameResource res)
        {
            CheckResource();
        }

        public bool CheckResource()
        {
            var storeResource = _resourceStorageController.GetResource(_costResource.Type);
            bool flag = storeResource.CheckCount(_costResource);
            string color = flag ? "<color=green>" : "<color=red>";

            string result = $"{color}{_costResource}</color>/{storeResource}";
            TextAmount.text = result;
            OnCheckResource(flag);
            return flag;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnCheckResource(bool check)
        {
            _observerCanBuy?.Execute(check);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}