using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Misc.Widgets.SwapToggle
{
    public class SwapSpriteToggle : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _diselectedSprite;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private ReactiveCommand<bool> _onSwitch = new ReactiveCommand<bool>();
        private bool _isOn;

        public IObservable<bool> OnSwitch => _onSwitch;

        private void Start()
        {
            //_button.OnClickAsObservable().Subscribe(_ => OnClick()).AddTo(_disposables);
        }

        private void OnClick()
        {
            _isOn = !_isOn;
            _image.sprite = _isOn ? _selectedSprite : _diselectedSprite;
            _onSwitch.Execute(_isOn);
        }

        public void On()
        {
            _isOn = true;
            _image.sprite = _selectedSprite;
        }

        public void Off()
        {
            _isOn = false;
            _image.sprite = _diselectedSprite;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}