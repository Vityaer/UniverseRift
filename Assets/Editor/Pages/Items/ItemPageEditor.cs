using Db.CommonDictionaries;
using Editor.Common;
using Editor.Units;
using Models;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UIController.Inventory;
using UnityEditor;
using Utils;

namespace Editor.Pages.Items
{
    public class ItemPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<ItemModel> _items => _dictionaries.Items.Select(l => l.Value).ToList();

        public ItemPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }
        
        public override void Init()
        {
            base.Init();
            Items = _items.Select(item => new ItemModelEditor(item, _dictionaries)).ToList();
            DataExist = true;
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Items.Add(id, new ItemModel() { Id = id });
            var item = new ItemModelEditor(new ItemModel(), _dictionaries);
            Items.Add(item);
        }

        private void RemoveElements(ItemModelEditor light, object b, List<ItemModelEditor> lights)
        {
            var targetElement = Items.First(e => e == light);
            Items.Remove(targetElement);
        }

        public override void Save()
        {
            //var items = Items.Select(itemModel => itemModel.GetModel).ToList();
            //EditorUtils.Save(items);
            base.Save();
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true, NumberOfItemsPerPage = 5,
    CustomAddFunction = nameof(AddElement), CustomRemoveElementFunction = nameof(RemoveElements))]
        [HorizontalGroup("3")]
        [LabelText("Items")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<ItemModelEditor> Items = new List<ItemModelEditor>();
    }
}
