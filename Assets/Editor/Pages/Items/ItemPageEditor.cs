using Editor.Common;
using Editor.Units;
using Models;
using Models.Items;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using UIController.Inventory;
using UnityEditor;
using UnityEngine;
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
            var items = Items.Select(itemModel => itemModel.GetModel()).ToList();
            EditorUtils.Save(items);
            base.Save();
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        public string NewSetName;
        [Button("Create set")]
        private void CreateSet()
        {
            if (NewSetName == string.Empty)
                return;

            foreach (var type in (ItemType[])Enum.GetValues(typeof(ItemType)))
            {
                AddElement();
                var model = Items[Items.Count - 1].GetModel();
                model.Id = $"{NewSetName}{type}";
                model.SetName = NewSetName;
                model.Type = type;
                model.Bonuses = new List<Bonus>() { new Bonus() };
            }
        }

        [Space(10)]
        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true, NumberOfItemsPerPage = 4,
    CustomAddFunction = nameof(AddElement), CustomRemoveElementFunction = nameof(RemoveElements))]
        [HorizontalGroup("3")]
        [LabelText("Items")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<ItemModelEditor> Items = new List<ItemModelEditor>();
    }
}
