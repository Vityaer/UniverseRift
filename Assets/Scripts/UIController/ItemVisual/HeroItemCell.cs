using City.TrainCamp;
using UIController.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ItemVisual
{
    public class HeroItemCell : MonoBehaviour
    {
        public string typeCell;
        public Sprite defaultSprite;

        [Header("Info")]
        public Image image;
        public RatingHero ratingController;

        private Item item;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        void Start()
        {
            DefaulfView();
        }


        public void DefaulfView()
        {
            image.sprite = defaultSprite;
            ratingController?.ShowRating(0);
        }

        private void SetBonus()
        {
            TrainCamp.Instance.ReturnSelectHero().CostumeHero.TakeOn(item);
            TrainCamp.Instance.HeroPanel.UpdateTextAboutHero();
            UpdateUI();
        }

        private void UpdateUI()
        {
            image.sprite = item.Image;
            image.color = new Color(255, 255, 255, 1);
            //ratingController?.ShowRating(item.Rating);
        }
        //API
        public void Clear()
        {
            item = null;
            DefaulfView();
        }

        public void SetItem(Item newItem)
        {
            if (item != null)
            {
                InventoryController.Instance.AddItem(item);
                TrainCamp.Instance.TakeOff(item);
                TrainCamp.Instance.HeroPanel.UpdateTextAboutHero();
            }

            item = newItem;

            if (item != null)
            {
                SetBonus();
            }
            else
            {
                DefaulfView();
            }
        }

        public void ClickOnCell()
        {
            if (item != null)
            {
                InventoryController.Instance.OpenInfoItem(item, typeCell, this);
            }
            else
            {
                InventoryController.Instance.Open(typeCell, this);
            }
        }
    }
}