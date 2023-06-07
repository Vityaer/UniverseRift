using Common;
using Common.Resourses;
using UIController.Inventory;
using UIController.Rewards;
using UnityEngine;

namespace UIController.ItemVisual
{
    public class SubjectCellController : MonoBehaviour
    {

        [Header("Info")]
        public ThingUI UIItem;

        private VisualAPI visual;
        private Resource res;
        private ItemController item;
        private SplinterController splinter;
        private BaseObject _subject;

        private void Start()
        {
            UIItem.SubjectButton?.onClick.AddListener(OpenDetail);
        }

        private void OpenDetail()
        {
            PanelInfoItem.Instance.OpenInfo(_subject);
        }

        private void SetVisual(VisualAPI visual)
        {
            CheckCell();
            this.visual = visual;
        }

        public void SetItem(VisualAPI visual)
        {
            CheckCell();
            this.visual = visual;
            visual.SetUI(UIItem);
        }

        public void SetItem(Resource res)
        {
            CheckCell();
            SetVisual(res.GetVisual());
            this.res = res;
            _subject = res;
            res.SetUI(UIItem);
        }

        public void SetItem(ItemController itemController)
        {
            CheckCell();
            SetVisual(itemController.GetVisual());
            item = itemController;
            _subject = itemController.item;
            itemController.SetUI(UIItem);
        }

        public void SetItem(SplinterController splinterController)
        {
            CheckCell();
            SetVisual(splinterController.GetVisual());
            splinter = splinterController;
            _subject = splinterController.splinter;
            splinterController.SetUI(UIItem);
        }

        public void SetItem(Item item)
        {
            CheckCell();
            SetVisual(item.GetVisual());
            item.SetUI(UIItem);
        }

        public void SetItem(PosibleRewardObject rewardObject)
        {
            CheckCell();
            // Debug.Log("set posible item");
            switch (rewardObject)
            {
                case PosibleRewardResource reward:
                    // Debug.Log("posible resource: " + reward.GetResource.GetName());
                    UIItem.UpdateUI(reward.GetResource.Image, string.Empty);
                    break;
                case PosibleRewardItem reward:
                    //UIItem.UpdateUI(reward.GetItem.Image/*, reward.GetItem.GetRare*/);
                    break;
                case PosibleRewardSplinter reward:
                    UIItem.UpdateUI(reward.GetSplinter.Image);
                    break;
            }
        }

        public void Clear()
        {
            res?.ClearUI();
            item?.ClearUI();
            splinter?.ClearUI();
            UIItem.Clear();
            visual = null;
        }

        public void ClickOnItem()
        {
            visual?.ClickOnItem();
        }

        public void OffCell()
        {
            Clear();
            gameObject.SetActive(false);
        }

        private void CheckCell()
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
        }
    }
}