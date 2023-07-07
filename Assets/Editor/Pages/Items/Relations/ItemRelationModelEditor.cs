using Db.CommonDictionaries;
using Editor.Common;
using Models.Items;
using Sirenix.OdinInspector;
using System.Linq;

namespace Pages.Items.Relations
{
    public class ItemRelationModelEditor : BaseModelEditor<ItemRelationModel>
    {
        private readonly CommonDictionaries _dictionaries;
        private string[] _allItems => _dictionaries.Items.Select(c => c.Value).Select(r => r.Id).ToArray();

        public ItemRelationModelEditor(ItemRelationModel item, CommonDictionaries commonDictionaries)
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
        [HorizontalGroup("3")]
        [LabelText("RequireCount")]
        [PropertyOrder(3)]
        public int RequireCount
        {
            get => _model.RequireCount;
            set => _model.RequireCount = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("ItemIngredientName")]
        [PropertyOrder(3)]
        [ValueDropdown(nameof(_allItems), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string ItemIngredientName
        {
            get
            {
                var result = _model.ItemIngredientName;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allItems.FirstOrDefault();
                }

                return result;
            }
            set => _model.ItemIngredientName = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("ResultItemName")]
        [PropertyOrder(3)]
        [ValueDropdown(nameof(_allItems), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string ResultItemName
        {
            get
            {
                var result = _model.ResultItemName;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allItems.FirstOrDefault();
                }

                return result;
            }
            set => _model.ResultItemName = value;
        }


    }
}
