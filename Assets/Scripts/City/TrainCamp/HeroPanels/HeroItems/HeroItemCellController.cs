using City.Panels.SubjectPanels;
using City.TrainCamp;
using Cysharp.Threading.Tasks;
using Models.Common;
using Models.Items;
using Network.DataServer;
using Network.DataServer.Messages.Items;
using System;
using City.Panels.Inventories;
using City.Panels.SubjectPanels.Items;
using City.TrainCamp.HeroPanels;
using UIController.Inventory;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UIController.ItemVisual
{
    public class HeroItemCellController : MonoBehaviour, IDisposable
    {
        [Inject] private InventoryPanelController _inventoryPanelController;
        [Inject] private ItemPanelController _itemPanelController;
        [Inject] private PageArmyController _trainCamp;
        [Inject] private HeroPanelController _heroPanelController;
        [Inject] private CommonGameData _commonGameData;

        [SerializeField] private ItemType _cellType;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Image _image;
        [SerializeField] private RatingHero _ratingController;
        [SerializeField] private Button _button;

        private CompositeDisposable _disposables = new();
        private CompositeDisposable _tempDisposables = new();
        private CompositeDisposable _swampTempDisposables;
        private GameItem _item;

        public GameItem Item => _item;
        public ItemType CellType => _cellType;

        void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => ClickOnCell()).AddTo(_disposables);
        }

        private void ClickOnCell()
        {
            if (_item != null)
            {
                _itemPanelController.Open(_item, true);
                _itemPanelController.OnAction.Subscribe(_ => TakeOff().Forget()).AddTo(_tempDisposables);
                _itemPanelController.OnSwapAction.Subscribe(_ => StartSwapItems()).AddTo(_tempDisposables);
                _itemPanelController.OnClose.Subscribe(_ => RefreshTempSubscribe()).AddTo(_tempDisposables);
            }
            else
            {
                _inventoryPanelController.Open(_cellType, this);
                _inventoryPanelController.OnObjectSelect.Subscribe(item => SetItem(item as GameItem).Forget()).AddTo(_tempDisposables);
                _inventoryPanelController.OnClose.Subscribe(item => RefreshTempSubscribe()).AddTo(_tempDisposables);
                _inventoryPanelController.WaitSelected = true;
            }
        }

        private void StartSwapItems()
        {
            RefreshSwapSubscribe();
            _inventoryPanelController.Open(_cellType, this);
            _inventoryPanelController.OnObjectSelect.Subscribe(item => Swaptem(item as GameItem).Forget()).AddTo(_swampTempDisposables);
            _inventoryPanelController.OnClose.Subscribe(item => RefreshSwapSubscribe()).AddTo(_swampTempDisposables);
            _inventoryPanelController.WaitSelected = true;
        }

        private async UniTaskVoid Swaptem(GameItem gameItem)
        {
            await SetItem(gameItem);
            _itemPanelController.CloseAfterSwap();
        }

        private async UniTaskVoid TakeOff()
        {
            var message = new TakeOffItemMessage { PlayerId = _commonGameData.PlayerInfoData.Id, HeroId = _heroPanelController.Hero.HeroData.Id, ItemType = _cellType };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                _trainCamp.ReturnSelectHero().Costume.TakeOff(_item);
                _inventoryPanelController.Add(_item);
                Clear();
                _heroPanelController.UpdateTextAboutHero();
            }
        }

        public void SetData(GameItem newItem)
        {
            _item = newItem;
            if (_item != null)
            {
                SetBonus();
                UpdateUI();
            }
            else
            {
                DefaulfView();
            }
        }

        private void RefreshTempSubscribe()
        {
            _tempDisposables.Dispose();
            _tempDisposables = new();
            _inventoryPanelController.WaitSelected = false;
        }

        private void RefreshSwapSubscribe()
        {
            _swampTempDisposables?.Dispose();
            _swampTempDisposables = new();
        }

        public void DefaulfView()
        {
            _image.sprite = _defaultSprite;
            _ratingController?.ShowRating(0);
        }

        private void SetBonus()
        {
            _trainCamp.ReturnSelectHero().Costume.TakeOn(_item);
            _heroPanelController.UpdateTextAboutHero();
        }

        private void UpdateUI()
        {
            _image.sprite = _item.Image;
            //ratingController?.ShowRating(item.Rating);
        }

        public void Clear()
        {
            if (_item == null)
                return;

            _item = null;
            DefaulfView();
        }

        public async UniTask SetItem(GameItem newItem)
        {
            var message = new SetItemMessage { PlayerId = _commonGameData.PlayerInfoData.Id, HeroId = _heroPanelController.Hero.HeroData.Id, ItemId = newItem.Id };
            var result = await DataServer.PostData(message);
            RefreshTempSubscribe();
            if (!string.IsNullOrEmpty(result))
            {
                if (_item != null)
                {
                    _inventoryPanelController.Add(_item);
                    _heroPanelController.UpdateTextAboutHero();
                }

                _item = newItem;
                var item = new GameItem(newItem.Model, 1);
                _inventoryPanelController.Remove(item);
                SetData(item);
            }
        }

        public void Dispose()
        {
            _tempDisposables?.Dispose();
            _swampTempDisposables?.Dispose();
            _disposables.Dispose();
        }
    }
}