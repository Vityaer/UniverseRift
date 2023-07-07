using City.Buildings.Forge;
using City.TrainCamp;
using Common.Resourses;
using System;
using UIController.Inventory;
using UniRx;
using UnityEngine;

namespace UIController.ItemVisual
{
    public class ForgeItemVisual : MonoBehaviour
    {
        [Header("Info")]
        public TypeMatter matter;
        private ItemSynthesis thing;
        public ItemSynthesis Thing { get => thing; }
        private GameItem item;
        public ThingUI UIItem;
        public ResourceObjectCost resourceCost;
        public ForgeItemObjectCost forgeItemCost;

        private ReactiveCommand<ForgeItemVisual> _onSelected = new ReactiveCommand<ForgeItemVisual>();

        public IObservable<ForgeItemVisual> OnSelected => _onSelected;

        public void SetItem(ItemSynthesis item)
        {
            thing = item;
            UIItem.UpdateUI(thing.reward.Image);
        }

        public void SetItem(GameItem item)
        {
            this.item = item;
            UIItem.UpdateUI(item.Image);
        }

        public void SetItem(GameItem item, int amount)
        {
            SetItem(item);
            forgeItemCost.SetInfo(item, amount);
        }

        public void SetResource(GameResource res)
        {
            UIItem.UpdateUI(res.Image, 1);
            resourceCost.SetData(res);
        }

        public void SelectItem()
        {
            if (matter == TypeMatter.Synthesis)
            {
                _onSelected.Execute(this);
                UIItem.Select();
            }
        }
    }
}