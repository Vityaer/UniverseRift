using City.TrainCamp;
using Db.CommonDictionaries;
using Models.Items;
using Sirenix.OdinInspector;
using System.Linq;
using UIController.Inventory;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UIController.ItemVisual
{
    public class HeroItemCellController : MonoBehaviour
    {
        [Inject] private InventoryController _inventoryController;
        [Inject] private PageArmyController _trainCamp;
        [Inject] private HeroPanelController _heroPanelController;

        private ItemType _cellType;

        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Image _image;
        [SerializeField] private RatingHero _ratingController;

        private GameItem _item;
        public ItemType CellType => _cellType;

        void Start()
        {
            DefaulfView();
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
            UpdateUI();
        }

        private void UpdateUI()
        {
            _image.sprite = _item.Image;
            _image.color = new Color(255, 255, 255, 1);
            //ratingController?.ShowRating(item.Rating);
        }
        //API
        public void Clear()
        {
            _item = null;
            DefaulfView();
        }

        public void SetItem(GameItem newItem)
        {
            if (_item != null)
            {
                _inventoryController.Add(_item);
                //_trainCamp.TakeOff(_item);
                _heroPanelController.UpdateTextAboutHero();
            }

            _item = newItem;

            if (_item != null)
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
            if (_item != null)
            {
                _inventoryController.OpenInfoItem(_item, _cellType, this);
            }
            else
            {
                _inventoryController.Open(_cellType, this);
            }
        }
    }
}