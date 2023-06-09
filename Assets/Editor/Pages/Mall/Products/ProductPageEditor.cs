using Db.CommonDictionaries;
using Editor.Common;
using Editor.Pages.Locations;
using Models;
using Models.Fights.Misc;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.Mall.Products
{
    public class ProductPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;

        private List<ProductModel> _products => _dictionaries.Products.Select(l => l.Value).ToList();

        public ProductPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Products = _products.Select(f => new ProductModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Products.Select(r => new LocationModel
            {
                Id = r.Id
            }).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Products.Add(id, new ProductModel() { Id = id });
            Products.Add(new ProductModelEditor(_dictionaries.Products[id]));
        }

        private void RemoveElements(ProductModelEditor light, object b, List<ProductModelEditor> lights)
        {
            var targetElement = Products.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Products.Remove(id);
            Products.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Rarity")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<ProductModelEditor> Products = new List<ProductModelEditor>();
    }
}
