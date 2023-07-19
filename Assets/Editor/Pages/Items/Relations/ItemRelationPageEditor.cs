using Common.Resourses;
using Db.CommonDictionaries;
using Editor.Common;
using Models.Common.BigDigits;
using Models.Data.Inventories;
using Models.Items;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UIController.Inventory;
using Utils;

namespace Pages.Items.Relations
{
    public class ItemRelationPageEditor: BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<ItemRelationModel> _itemRelations => _dictionaries.ItemRelations.Select(l => l.Value).ToList();

        public ItemRelationPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }
        
        public override void Init()
        {
            base.Init();
            ItemRelations = _itemRelations.Select(item => new ItemRelationModelEditor(item, _dictionaries)).ToList();
            DataExist = true;
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.ItemRelations.Add(id, new ItemRelationModel() { Id = id });
            var item = new ItemRelationModelEditor(new ItemRelationModel(), _dictionaries);
            ItemRelations.Add(item);
        }

        private void RemoveElements(ItemRelationModelEditor light, object b, List<ItemRelationModelEditor> lights)
        {
            var targetElement = ItemRelations.First(e => e == light);
            ItemRelations.Remove(targetElement);
        }

        public override void Save()
        {
            var itemRelations = ItemRelations.Select(itemRelationModel => itemRelationModel.GetModel()).ToList();
            EditorUtils.Save(itemRelations);
            base.Save();
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [PropertyOrder(1)]
        public string NewSetName;
        [ShowInInspector]
        [PropertyOrder(1)]
        [HorizontalGroup("1")]
        public string RequireSetName;

        [HorizontalGroup("2")]
        [Button("Create set")]
        private void CreateSet()
        {
            if (NewSetName == string.Empty || RequireSetName == string.Empty)
                return;

            foreach (var type in (ItemType[])Enum.GetValues(typeof(ItemType)))
            {
                AddElement();
                var model = ItemRelations[ItemRelations.Count - 1].GetModel();
                model.Id = $"Recipe{NewSetName}{type}";
                model.ResultItemName = $"{NewSetName}{type}";
                model.ItemIngredientName = $"{RequireSetName}{type}";
                model.RequireCount = 3;
                model.Cost = new ResourceData() { Type = ResourceType.Gold, Amount = new BigDigit(1, 3) };
            }
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true, NumberOfItemsPerPage = 4,
    CustomAddFunction = nameof(AddElement), CustomRemoveElementFunction = nameof(RemoveElements))]
        [HorizontalGroup("3")]
        [LabelText("Items")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<ItemRelationModelEditor> ItemRelations = new List<ItemRelationModelEditor>();
    }
}
