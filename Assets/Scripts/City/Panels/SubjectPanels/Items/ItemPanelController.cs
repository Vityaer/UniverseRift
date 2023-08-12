using UIController.Inventory;
using UIController.ItemVisual;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainerUi.Model;
using VContainerUi.Messages;
using VContainer;
using VContainer.Unity;
using System;

namespace City.Panels.SubjectPanels
{
    public class ItemPanelController : UiPanelController<ItemPanelView>, IInitializable
    {
        [Inject] private readonly InventoryController _inventoryController;
        
        private HeroItemCellController _cellItem;
        private GameItem _selectItem;

        public ReactiveCommand OnAction = new ReactiveCommand();
        public ReactiveCommand OnDrop = new ReactiveCommand();
        public ReactiveCommand OnClose = new ReactiveCommand();

        public new void Initialize()
        {
            View.ActionButton.OnClickAsObservable().Subscribe(_ => OnClickButtonAction()).AddTo(Disposables);
            View.OpenInventoryButton.OnClickAsObservable().Subscribe(_ => OpenInventory()).AddTo(Disposables);
            base.Initialize();
        }

        public void Open(GameItem item, HeroItemCellController cellItem, bool onHero = false)
        {
            _cellItem = cellItem;
            _selectItem = item;

            if (onHero == false)
            {
                View.ActionButtonText.text = "Снарядить";
            }
            else
            {
                View.ActionButtonText.text = "Снять";
            }

            View.MainImage.SetData(item);
            _messagesPublisher.OpenWindowPublisher.OpenWindow<ItemPanelController>(openType: OpenType.Additive);
        }

        private void OnClickButtonAction()
        {
            OnAction.Execute();
            Close();
        }

        public override void OnHide()
        {
            OnClose.Execute();
            base.OnHide();
        }

        private void OpenInventory()
        {
            _inventoryController.Open(_cellItem.CellType, _cellItem);
        }

        private void SelectItem()
        {

            //ActionButtonText.onClick.AddListener(InventoryController.Instance.SelectItem);
        }

        private void TakeOff()
        {
            _inventoryController.Add(_selectItem);
            _cellItem.Clear();
        }
    }
}
