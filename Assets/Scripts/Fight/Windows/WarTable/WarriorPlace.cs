using Hero;
using System;
using TMPro;
using UIController.Cards;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.WarTable
{
    public class WarriorPlace : MonoBehaviour, IDisposable
    {
        public int Id;

        [SerializeField] private Button _button;
        [SerializeField] private Image _background;
        [SerializeField] private Image _imageHero;
        [SerializeField] private TextMeshProUGUI _levelText;


        private ReactiveCommand<WarriorPlace> _onClick = new ReactiveCommand<WarriorPlace>();
        private CompositeDisposable _disposables = new CompositeDisposable();

        public bool IsEmpty => Hero == null;
        public IObservable<WarriorPlace> OnClick => _onClick;
        public GameHero Hero { get; private set; }

        private void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => _onClick.Execute(this)).AddTo(_disposables);
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

        public void ClearPlace()
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