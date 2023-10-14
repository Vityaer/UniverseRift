using Db.CommonDictionaries;
using Editor.Common;
using Models.City.Markets;
using Models.Data.Inventories;
using Models.Items;
using Pages.Items.Relations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utils;

namespace Editor.Pages.Mall.Products
{
    public class ProductPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;

        private List<BaseProductModel> _products => _dictionaries.Products.Select(l => l.Value).ToList();

        public ProductPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();

            ResourseProducts = _products
                .Where(product => product is ResourceProductModel)
                .Select(product => product as ResourceProductModel)
                .ToList();

            ItemProducts = _products
                .Where(product => product is ItemProductModel)
                .Select(product => product as ItemProductModel)
                .ForEach(product => product.Subject.CommonDictionaries = _dictionaries)
                .ToList();

            DataExist = true;
        }

        public override void Save()
        {
            var products = new List<BaseProductModel>();
            products.AddRange(ResourseProducts);
            products.AddRange(ItemProducts);
            EditorUtils.Save(products);
            base.Save();
        }

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Resource")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        [ListDrawerSettings(DraggableItems = false, Expanded = false, NumberOfItemsPerPage = 4)]
        public List<ResourceProductModel> ResourseProducts = new List<ResourceProductModel>();

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("4")]
        [LabelText("Item")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = false, NumberOfItemsPerPage = 4,
    CustomAddFunction = nameof(AddItemElement), CustomRemoveElementFunction = nameof(RemoveItemElements))]
        public List<ItemProductModel> ItemProducts = new List<ItemProductModel>();

        private void AddItemElement()
        {
            var item = new ItemProductModel(_dictionaries);
            ItemProducts.Add(item);
        }

        private void RemoveItemElements(ItemProductModel light, object b, List<ItemProductModel> lights)
        {
            var targetElement = ItemProducts.First(e => e == light);
            ItemProducts.Remove(targetElement);
        }
    }
}
