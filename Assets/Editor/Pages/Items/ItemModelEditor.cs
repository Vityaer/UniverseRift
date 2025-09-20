using Editor.Common;
using Models;
using Models.Items;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using UIController.Inventory;

namespace Editor.Pages.Items
{
    [HideReferenceObjectPicker]
    public class ItemModelEditor : BaseModelEditor<ItemModel>
    {
        private readonly CommonDictionaries m_dictionaries;
        private string[] _allRarities => m_dictionaries.Rarities.Select(c => c.Value).Select(r => r.Id).ToArray();
        private string[] _allRatings => m_dictionaries.Ratings.Select(c => c.Value).Select(r => r.Id).ToArray();
        private string[] _allSets => m_dictionaries.ItemSets.Select(c => c.Value).Select(r => r.Id).ToArray();

        public ItemModelEditor(ItemModel item, CommonDictionaries commonDictionaries)
        {
            m_dictionaries = commonDictionaries;
            _model = item;
        }

        [ShowInInspector]
        [HorizontalGroup("Item")]
        [VerticalGroup("Item/Left")]
        [BoxGroup("Item/Left/Common")]
        [LabelText("Id")]
        [PropertyOrder(1)]
        [LabelWidth(110)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("Item")]
        [VerticalGroup("Item/Left")]
        [BoxGroup("Item/Left/Common")]
        [LabelText("Type")]
        [PropertyOrder(1)]
        [LabelWidth(110)]
        public ItemType Type
        {
            get => _model.Type;
            set => _model.Type = value;
        }

        [ShowInInspector]
        [HorizontalGroup("Item")]
        [VerticalGroup("Item/Left")]
        [BoxGroup("Item/Left/Common")]
        [LabelText("Bonuses")]
        [PropertyOrder(2)]
        [LabelWidth(110)]
        public List<Bonus> ListBonuses
        {
            get => _model.Bonuses;
            set => _model.Bonuses = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("Rating")]
        [PropertyOrder(3)]
        [ValueDropdown(nameof(_allRatings), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Rating
        {
            get
            {
                var result = _model.Rating;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allRatings.FirstOrDefault();
                }

                return result;
            }
            set => _model.Rating = value;
        }

        [ShowInInspector]
        [HorizontalGroup("4")]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("Set")]
        [PropertyOrder(4)]
        [ValueDropdown(nameof(_allSets), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Set
        {
            get
            {
                var result = _model.SetName;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allSets.FirstOrDefault();
                }

                return result;
            }
            set => _model.SetName = value;
        }
    }
}
