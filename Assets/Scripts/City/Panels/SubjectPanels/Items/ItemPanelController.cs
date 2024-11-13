using City.Panels.SubjectPanels.Items;
using LocalizationSystems;
using System;
using UIController.Inventory;
using UIController.ItemVisual;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using Utils;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.SubjectPanels
{
    public class ItemPanelController : UiPanelController<ItemPanelView>, IInitializable
    {
        private readonly ILocalizationSystem _localizationSystem;

        private HeroItemCellController _cellItem;
        private GameItem _selectItem;
        private DynamicUiList<ItemBonusView, Bonus> _bonusesPool;

        public ReactiveCommand OnAction = new();
        public ReactiveCommand OnSwapAction = new();
        public ReactiveCommand OnDrop = new();
        public ReactiveCommand OnClose = new();

        public ItemPanelController(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public new void Initialize()
        {
            _bonusesPool = new(View.itemBonusViewPrefab, View.BonusesScroll.content, View.BonusesScroll, null, OnCreateBonusView);
            View.ActionButton.OnClickAsObservable().Subscribe(_ => OnClickButtonAction()).AddTo(Disposables);
            View.SwapButton.OnClickAsObservable().Subscribe(_ => StartSwapItems()).AddTo(Disposables);
            base.Initialize();
        }

        public void Open(GameItem item, HeroItemCellController cellItem, bool onHero = false)
        {
            _cellItem = cellItem;
            _selectItem = item;

            View.ActionButton.gameObject.SetActive(true);
            if (onHero == false)
            {
                View.ActionButtonText.StringReference = _localizationSystem.GetLocalizedContainer("SetItemButtonLabel");
                View.SwapButton.gameObject.SetActive(false);
            }
            else
            {
                View.ActionButtonText.StringReference = _localizationSystem.GetLocalizedContainer("TakeOffItemButtonLabel");
                View.SwapButton.gameObject.SetActive(true);
            }
            View.ActionButton.interactable = true;
            FillData(item);
        }

        private void OnCreateBonusView(ItemBonusView view)
        {
            view.Init(_localizationSystem);
        }

        public void ShowData(GameItem item)
        {
            View.ActionButton.gameObject.SetActive(false);
            View.SwapButton.gameObject.SetActive(false);
            FillData(item);
        }

        private void FillData(GameItem item)
        {
            View.MainImage.SetData(item);
            View.Name.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{item.Id}Name");

            View.ItemType.StringReference = _localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{item.Type}TypeName");

            _bonusesPool.ShowDatas(item.Model.Bonuses);
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

        private void StartSwapItems()
        {
            OnSwapAction.Execute();
        }

        public void CloseAfterSwap()
        {
            Close();
        }
    }
}
