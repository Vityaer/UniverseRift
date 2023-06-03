using Db.CommonDictionaries;
using Editor.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.Pages.Items
{
    [HideReferenceObjectPicker]
    public class ItemModelEditor : BaseModelEditor<Item>
    {
        private readonly CommonDictionaries _dictionaries;
        private string[] _allRarities => _dictionaries.Rarities.Select(c => c.Value).Select(r => r.Id).ToArray();
        private string[] _allTypes => _dictionaries.ItemTypes.Select(c => c.Value).Select(r => r.Id).ToArray();
        private string[] _allRatings => _dictionaries.Ratings.Select(c => c.Value).Select(r => r.Id).ToArray();
        private string[] _allSets => _dictionaries.ItemSets.Select(c => c.Value).Select(r => r.Id).ToArray();

        public ItemModelEditor(Item item, CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
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
        [LabelText("Bonuses")]
        [PropertyOrder(2)]
        [LabelWidth(110)]
        public List<Bonus> ListBonuses = new List<Bonus>();

        [ShowInInspector]
        [HorizontalGroup("3")]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("Type")]
        [PropertyOrder(3)]
        [ValueDropdown(nameof(_allTypes), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Type
        {
            get
            {
                var result = _model.Type;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allTypes.FirstOrDefault();
                }

                return result;
            }
            set => _model.Type = value;
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
                var result = _model.Set;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allSets.FirstOrDefault();
                }

                return result;
            }
            set => _model.Set = value;
        }

        [ShowInInspector]
        [HorizontalGroup("4")]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("Rarity")]
        [PropertyOrder(4)]
        [ValueDropdown(nameof(_allRarities), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string Rarity
        {
            get
            {
                var result = _model.Rarity;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allRarities.FirstOrDefault();
                }

                return result;
            }
            set => _model.Rarity = value;
        }
    }
}
