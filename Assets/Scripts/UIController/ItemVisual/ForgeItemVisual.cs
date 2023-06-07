using City.Buildings.Forge;
using City.TrainCamp;
using Common.Resourses;
using UIController.Inventory;
using UnityEngine;

namespace UIController.ItemVisual
{
    public class ForgeItemVisual : MonoBehaviour
    {
        [Header("Info")]
        public TypeMatter matter;
        private ItemSynthesis thing;
        public ItemSynthesis Thing { get => thing; }
        private Item item;
        public ThingUI UIItem;
        public ResourceObjectCost resourceCost;
        public ForgeItemObjectCost forgeItemCost;

        public void SetItem(ItemSynthesis item)
        {
            thing = item;
            UIItem.UpdateUI(thing.reward.Image);
        }

        public void SetItem(Item item)
        {
            this.item = item;
            UIItem.UpdateUI(item.Image);
        }

        public void SetItem(Item item, int amount)
        {
            SetItem(item);
            forgeItemCost.SetInfo(item, amount);
        }

        public void SetResource(Resource res)
        {
            UIItem.UpdateUI(res.Image, 1);
            resourceCost.SetData(res);
        }

        public void SelectItem()
        {
            if (matter == TypeMatter.Synthesis)
            {
                Forge.Instance.SelectItem(this, thing);
                UIItem.Select();
            }
        }
    }
}