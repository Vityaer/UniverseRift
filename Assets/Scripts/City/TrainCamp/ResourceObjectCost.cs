using Common;
using Common.Resourses;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.TrainCamp
{
    public class ResourceObjectCost : MonoBehaviour
    {
        [SerializeField] private Image Image;
        [SerializeField] private TextMeshProUGUI TextAmount;

        private Resource costResource = null;
        private Resource storeResource;

        private ReactiveCommand<bool> _observerCanBuy = new ReactiveCommand<bool>();

        public IObservable<bool> ObserverCanBuy => _observerCanBuy;

        public void SetData(Resource res)
        {
            if (costResource != null)
                GameController.Instance.UnregisterOnChangeResource(CheckResource, costResource.Name);

            costResource = res;
            CheckResource();
            Image.sprite = costResource.Image;
            gameObject.SetActive(true);
            GameController.Instance.RegisterOnChangeResource(CheckResource, costResource.Name);
        }

        public void CheckResource(Resource res)
        {
            CheckResource();
        }

        public bool CheckResource()
        {
            storeResource = GameController.Instance.GetResource(costResource.Name);
            bool flag = GameController.Instance.CheckResource(costResource);
            string color = flag ? "<color=green>" : "<color=red>";

            string result = $"{color}{costResource}</color>/{storeResource}";
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
    }
}