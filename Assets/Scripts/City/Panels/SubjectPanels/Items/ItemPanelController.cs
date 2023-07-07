using UIController.Inventory;
using UIController.ItemVisual;
using UiExtensions.Scroll.Interfaces;
using UniRx;

namespace City.Panels.SubjectPanels
{
    public class ItemPanelController : UiPanelController<ItemPanelView>
    {
        private HeroItemCellController _cellItem;

        public ReactiveCommand OnAction = new ReactiveCommand();
        public ReactiveCommand OnDrop = new ReactiveCommand();

        public void ShowData(GameItem item, HeroItemCellController cellItem, bool onHero = false)
        {
            //View.ActionButton.OnClickAsObservable().Subscribe(_ => OnAction.Execute()).AddTo(Disposables);
            //View.DropButton.OnClickAsObservable().Subscribe(_ => OnDrop.Execute()).AddTo(Disposables);

            _cellItem = cellItem;

            if (onHero == false)
            {
                //componentButtonAction.onClick.AddListener(InventoryController.Instance.SelectItem);
                //View.Bu.text = "Снарядить";
                //btnDrop.SetActive(false);
            }
            else
            {
                //componentButtonDrop.onClick.AddListener(() => cellItem.SetItem(null));
                //textButtonDrop.text = "Снять";
                //btnDrop.SetActive(true);
            }

            //selectItem = item;
            //UpdateUIInfo(item.Image, item.Id, type: item.Type.ToString(), generalInfo: item.GetTextBonuses());
            //OpenPanel();
        }
    }
}
