using Hero;
using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.Common.WarTable
{
    public class WarriorPlace : MonoBehaviour, IDisposable
    {
        public int Id;

        [SerializeField] private Button _button;
        [SerializeField] private Image _background;
        [SerializeField] private Image _imageHero;
        [SerializeField] private TextMeshProUGUI _levelText;

        private ReactiveCommand<WarriorPlace> _startDrag = new();
        private ReactiveCommand<WarriorPlace> _onDrop = new();
        private ReactiveCommand<WarriorPlace> _onClick = new();
        private CompositeDisposable _disposables = new();
        private bool _mouseInnerFlag;
        private bool _isDraging;

        public bool IsEmpty => Hero == null;
        public bool MouseInnerFlag => _mouseInnerFlag;
        public IObservable<WarriorPlace> OnClick => _onClick;
        public IObservable<WarriorPlace> OnStartDrag => _startDrag;
        public IObservable<WarriorPlace> OnDrop => _onDrop;
        public GameHero Hero { get; private set; }

        private void Awake()
        {
            Clear();
        }

        private void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => Click()).AddTo(_disposables);

            _button.OnPointerEnterAsObservable().Subscribe(_ => MouseInner(true)).AddTo(_disposables);
            _button.OnPointerExitAsObservable().Subscribe(_ => MouseInner(false)).AddTo(_disposables);

            _button.OnPointerDownAsObservable().Subscribe(_ => OnPointerDown()).AddTo(_disposables);
            _button.OnPointerUpAsObservable().Subscribe(_ => OnPointerUp()).AddTo(_disposables);
        }

        private void Click()
        {
            if (_isDraging)
                return;

            _onClick.Execute(this);
        }

        public void SetDragingStatus(bool status)
        {
            _isDraging = status;
        }

        private void MouseInner(bool flag)
        {
            _mouseInnerFlag = flag;
        }

        private void OnPointerDown()
        {
            if (!_mouseInnerFlag)
                return;

            _startDrag.Execute(this);
        }

        private void OnPointerUp()
        {
            if (!_mouseInnerFlag)
                return;

            _onDrop.Execute(this);
        }


        public void SetHero(GameHero hero)
        {
            Hero = hero;
            UpdateUI();
        }

        public void UpdateUI()
        {
            _imageHero.sprite = Hero.Avatar;
            _imageHero.enabled = true;
            _levelText.text = $"{Hero.HeroData.Level}";
        }

        public void Clear()
        {
            Hero = null;
            _imageHero.enabled = false;
            _levelText.text = string.Empty;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}