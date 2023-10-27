using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace UIController.ControllerPanels.CountControllers
{
    public class CountPanelController : MonoBehaviour, IDisposable
    {
        public TMP_InputField CountInputField;
        public Button IncereaseCountButton;
        public Button DeacreaseCountButton;

        private int _count = 1;
        private int _minCount = 1;
        private int _maxCount = 10;
        private int _delta = 1;

        private ReactiveCommand<int> _onChangeCount = new ReactiveCommand<int>();
        private CompositeDisposable _disposables = new CompositeDisposable();

        public int Count { get => _count; }
        public int MinCount { get => _minCount; }
        public int MaxCount { get => _maxCount; }
        public IObservable<int> OnChangeCount => _onChangeCount;

        public void Start()
        {
            CountInputField.onEndEdit.AddListener(OnEndEditCount);
            DeacreaseCountButton.OnClickAsObservable().Subscribe(_ => DecreaseCount()).AddTo(_disposables);
            IncereaseCountButton.OnClickAsObservable().Subscribe(_ => IncreaseCount()).AddTo(_disposables);
        }

        private void OnEndEditCount(string text)
        {
            if (int.TryParse(text, out var count))
            {
                var check = (count >= _minCount) && (count <= _maxCount);
            }
        }

        private void IncreaseCount()
        {
            if (_count < _maxCount) _count += _delta;
            if (_count > _maxCount) _count = _maxCount;
            ChangeCount();
        }

        private void DecreaseCount()
        {
            if (_count > _minCount) _count -= _delta;
            if (_count < _minCount) _count = _minCount;
            ChangeCount();
        }

        private void ChangeCount()
        {
            IncereaseCountButton.interactable = (_count != _maxCount);
            DeacreaseCountButton.interactable = (_count != _minCount);

            _onChangeCount.Execute(_count);
            UpdateUI();
        }

        private void UpdateUI()
        {
            CountInputField.text = $"{_count}";
        }

        public void SetMax(int newMax)
        {
            _maxCount = newMax;
            _count = _maxCount;
            ChangeCount();
        }

        public void SetMin(int newMin)
        {
            _minCount = newMin;
            if (_count < newMin) _count = newMin;
            ChangeCount();
        }

        public void Dispose()
        {
            CountInputField.onEndEdit.RemoveAllListeners();
            _disposables.Dispose();
        }
    }
}