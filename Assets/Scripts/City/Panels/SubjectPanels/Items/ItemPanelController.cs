using LocalizationSystems;
using UIController.Inventory;
using UIController.ItemVisual;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.SubjectPanels
{
    public class ItemPanelController : UiPanelController<ItemPanelView>, IInitializable
    {
        //[Inject] private readonly InventoryController _inventoryController;
        private readonly ILocalizationSystem _localizationSystem;

        private HeroItemCellController _cellItem;
        private GameItem _selectItem;

        public ReactiveCommand OnAction = new ReactiveCommand();
        public ReactiveCommand OnDrop = new ReactiveCommand();
        public ReactiveCommand OnClose = new ReactiveCommand();

        public ItemPanelController(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public new void Initialize()
        {
            View.ActionButton.OnClickAsObservable().Subscribe(_ => OnClickButtonAction()).AddTo(Disposables);
            //View.OpenInventoryButton.OnClickAsObservable().Subscribe(_ => OpenInventory()).AddTo(Disposables);
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
            View.ActionButton.interactable = true;
            FillData(item);
        }

        public void ShowData(GameItem item)
        {
            View.ActionButton.interactable = false;
            FillData(item);
        }

        private void FillData(GameItem item)
        {
            View.MainImage.SetData(item);
            View.Name.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{item.Id}Name");

            View.ItemType.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{item.Type}Name");
            MessagesPublisher.OpenWindowPublisher.OpenWindow<ItemPanelController>(openType: OpenType.Additive);
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
            //_inventoryController.Open(_cellItem.CellType, _cellItem);
        }

        private void SelectItem()
        {

            //ActionButtonText.onClick.AddListener(InventoryController.Instance.SelectItem);
        }

        private void TakeOff()
        {
            //_inventoryController.Add(_selectItem);
            _cellItem.Clear();
        }
    }
}
