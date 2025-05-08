using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Misc.Widgets.SwapToggle
{
    public class SwapSpriteToggle : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _diselectedSprite;
        [SerializeField] private bool _isOn;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private ReactiveCommand<bool> _onSwitch = new ReactiveCommand<bool>();

        public IObservable<bool> OnSwitch => _onSwitch;

        public bool IsOn => _isOn;
        
        private void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => OnClick()).AddTo(_disposables);
            _image.sprite = _isOn ? _selectedSprite : _diselectedSprite;
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
            _onSwitch.Execute(true);
        }

        public void Off()
        {
            _isOn = false;
            _image.sprite = _diselectedSprite;
            _onSwitch.Execute(false);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}