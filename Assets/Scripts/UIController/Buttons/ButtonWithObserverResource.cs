using City.TrainCamp;
using Common.Resourses;
using System;
using Common.Inventories.Resourses;
using UniRx;
using UnityEngine;

namespace UIController.Buttons
{
    public class ButtonWithObserverResource : MonoBehaviour
    {
        public ResourceObjectCost resourceObserver;
        public ButtonCostController buttonCostComponent;
        public GameResource cost;
        private CompositeDisposable _disposable = new CompositeDisposable();

        public event Action OnClick;

        private void Awake()
        {
            buttonCostComponent.OnClick.Subscribe(_ => OnClick?.Invoke()).AddTo(_disposable);
        }

        public void ChangeCost(GameResource cost)
        {
            resourceObserver.SetData(cost);
            buttonCostComponent.SetCost(cost);
        }

        public void ChangeCost(GameResource cost, Action value)
        {
            resourceObserver.SetData(cost);
            buttonCostComponent.SetCost(cost);
            OnClick = value;
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }


    }
}