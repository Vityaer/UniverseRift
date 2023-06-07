using Common;
using Common.Resourses;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace City.TrainCamp
{
    public class ResourceObjectCost : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI textAmount;
        private Resource costResource = null;
        private Resource storeResource;

        public void SetData(Resource res)
        {
            if (costResource != null)
                GameController.Instance.UnregisterOnChangeResource(CheckResource, costResource.Name);

            costResource = res;
            CheckResource();
            image.sprite = costResource.Image;
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
            string result = flag ? "<color=green>" : "<color=red>";
            result = string.Concat(result, costResource.ToString(), "</color>/", storeResource.ToString());
            textAmount.text = result;
            OnCheckResource(flag);
            return flag;
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        private Action<bool> observerCanBuy;
        public void RegisterOnCanBuy(Action<bool> d) { observerCanBuy += d; }
        public void UnregisterOnCanBuy(Action<bool> d) { observerCanBuy -= d; }
        private void OnCheckResource(bool check)
        {
            if (observerCanBuy != null)
                observerCanBuy(check);
        }
    }
}