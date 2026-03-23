using Editor.Common;
using Models.Data.Inventories;
using Models.Items;
using Sirenix.OdinInspector;
using System.Linq;
using Common.Db.CommonDictionaries;

namespace Pages.Items.Relations
{
    public class ItemRelationModelEditor : BaseModelEditor<ItemRelationModel>
    {
        private readonly CommonDictionaries _dictionaries;
        private string[] _allItems => _dictionaries.Items.Select(c => c.Value).Select(r => r.Id).ToArray();

        public ItemRelationModelEditor(ItemRelationModel item, CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            m_model = item;
        }

        [ShowInInspector]
        [LabelText("Id")]
        [PropertyOrder(1)]
        [LabelWidth(110)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [LabelText("RequireCount")]
        [PropertyOrder(3)]
        public int RequireCount
        {
            get => m_model.RequireCount;
            set => m_model.RequireCount = value;
        }

        [ShowInInspector]
        [LabelText("Cost")]
        [PropertyOrder(3)]
        [LabelWidth(110)]
        public ResourceData Cost
        {
            get => m_model.Cost;
            set => m_model.Cost = value;
        }

        [ShowInInspector]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("ItemIngredientName")]
        [PropertyOrder(3)]
        [ValueDropdown(nameof(_allItems), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string ItemIngredientName
        {
            get
            {
                var result = m_model.ItemIngredientName;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allItems.FirstOrDefault();
                }

                return result;
            }
            set => m_model.ItemIngredientName = value;
        }

        [ShowInInspector]
        [ListDrawerSettings(ShowItemCount = true, ShowIndexLabels = true, Expanded = true, DraggableItems = false)]
        [LabelText("ResultItemName")]
        [PropertyOrder(3)]
        [ValueDropdown(nameof(_allItems), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string ResultItemName
        {
            get
            {
                var result = m_model.ResultItemName;
                if (string.IsNullOrEmpty(result))
                {
                    result = _allItems.FirstOrDefault();
                }

                return result;
            }
            set => m_model.ResultItemName = value;
        }


    }
}
