using Common.Inventories.Splinters;
using System;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer.Unity;

namespace UIController.ControllerPanels.SelectCount
{
    public class SplinterSelectCountPanelController : UiPanelController<SplinterSelectCountPanelView>, IStartable
    {
        private int _requireCount = 0;
        private int _storeCount = 0;
        private int _selectedCount = 0;
        private GameSplinter _splinter;
        private ReactiveCommand<int> _actionOnSelectedCount = new ReactiveCommand<int>();

        public ReactiveCommand ActionAfterUse = new ReactiveCommand();
        public IObservable<int> ActionOnSelectedCount => _actionOnSelectedCount;

        public override void Start()
        {
            View.CountController.OnChangeCount.Subscribe(ChangeCount).AddTo(Disposables);
            //View.ActionButton.OnClickAsObservable().Subscribe(_ => SelectedCountDone()).AddTo(Disposables);
        }

        public void ChangeCount(int count)
        {
            _selectedCount = count;
            View.CountSlider.SetAmount(count * _requireCount, _requireCount);
        }

        public void Open(GameSplinter splinter)
        {
            _splinter = splinter;
            _requireCount = splinter.RequireAmount;
            _storeCount = splinter.Amount;
            View.MainImage.SetData(splinter);
            View.CountController.SetMax(_storeCount / _requireCount);
            ChangeCount(count: _storeCount / _requireCount);
        }

        void SelectedCountDone()
        {
            _splinter.GetReward(_selectedCount);
            _actionOnSelectedCount.Execute(_selectedCount);
            ActionAfterUse.Execute();
            Close();
        }
    }
}