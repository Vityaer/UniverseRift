using Models.City.Mines;
using System;
using TMPro;
using UniRx;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using VContainer;
using VContainerUi.Abstraction;

namespace City.Buildings.Mines
{
    public class MineCard : UiView
    {
        public GameObject Panel;
        public Image MineImage;
        public Image OutLight;
        public TextMeshProUGUI CountRequirementText;
        public Button MainButton;

        private MineModel _mineModel;
        private GameMineRestriction _mineRestriction;
        private ReactiveCommand<MineCard> _onSelect = new();

        public IObservable<MineCard> OnSelect => _onSelect;
        public bool GetCanCreateFromCount { get => _mineRestriction.CurrentCount < _mineRestriction.MaxCount; }
        public MineModel Model => _mineModel;

        protected override void Awake()
        {
            MainButton.onClick.AddListener(Click);
            base.Awake();
        }

        public void SetData(MineModel mineModel, GameMineRestriction mineRestriction)
        {
            _mineModel = mineModel;
            _mineRestriction = mineRestriction;

            var path = _mineModel.SpritePath.ReplaceForResources();
            path = path.Replace(".png", "");
            var sprite = Resources.Load<Sprite>(path);
            MineImage.sprite = sprite;
            CountRequirementText.text = FunctionHelp.AmountFromRequireCount(_mineRestriction.CurrentCount, _mineRestriction.MaxCount);
            Panel.SetActive(true);
        }

        public void Click()
        {
            if (_mineRestriction.CurrentCount < _mineRestriction.MaxCount)
            {
                _onSelect.Execute(this);
            }
        }

        public void Select()
        {
            OutLight.enabled = true;
        }

        public void Diselect()
        {
            OutLight.enabled = false;
        }

        public void Hide()
        {
            Panel.SetActive(false);
        }
    }
}