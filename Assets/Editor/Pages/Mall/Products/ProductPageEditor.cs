using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Common.Resourses;
using Editor.Common;
using Models.City.Markets;
using Models.Common.BigDigits;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
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
                .OfType<ResourceProductModel>()
                .ToList();

            ItemProducts = _products
                .OfType<ItemProductModel>()
                .ForEach(product => product.Subject.CommonDictionaries = _dictionaries)
                .ToList();

            SplinterProducts = _products
                .OfType<SplinterProductModel>()
                .ForEach(product => product.Subject.CommonDictionaries = _dictionaries)
                .ToList();

            DataExist = true;
        }

        public override void Save()
        {
            var products = new List<BaseProductModel>();
            products.AddRange(ResourseProducts);
            products.AddRange(ItemProducts);
            products.AddRange(SplinterProducts);
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
        [PropertyOrder(3)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = false,
            NumberOfItemsPerPage = 4,
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

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("5")]
        [LabelText("Splinter")]
        [PropertyOrder(3)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = false,
            NumberOfItemsPerPage = 4,
            CustomAddFunction = nameof(AddSplinterElement),
            CustomRemoveElementFunction = nameof(RemoveSplinterElements))]
        public List<SplinterProductModel> SplinterProducts = new List<SplinterProductModel>();

        private void AddSplinterElement()
        {
            var item = new SplinterProductModel(_dictionaries);
            item.Id = UnityEngine.Random.Range(1000, 9999).ToString();
            SplinterProducts.Add(item);
        }

        private void RemoveSplinterElements(SplinterProductModel light, object b, List<SplinterProductModel> lights)
        {
            var targetElement = SplinterProducts.First(e => e == light);
            SplinterProducts.Remove(targetElement);
        }

        [PropertyOrder(4)]
        [Button("Check products")]
        public void CheckProducts()
        {
            CheckSplinters();
        }

        private void CheckSplinters()
        {
            CheckAltarMarketSplinters();
            CheckHeroMarketSplinters();
        }

        private void CheckAltarMarketSplinters()
        {
            foreach (var hero in _dictionaries.Heroes)
            {
                var altarMarketKey = $"{hero.Key}AltarMarketProduct";

                var splinterId = $"{hero.Key}Splinter";
                if (!_dictionaries.Splinters.TryGetValue(splinterId, out var splinterModel))
                {
                    Debug.LogError($"Splinter: {splinterId} not found");
                    continue;
                }

                if (_dictionaries.Products.TryGetValue(altarMarketKey, out var product))
                {
                    continue;
                }

                var heroProductModel = new SplinterProductModel(_dictionaries);
                heroProductModel.Id = altarMarketKey;
                heroProductModel.Subject.Id = splinterId;
                heroProductModel.Subject.Amount = 30;

                heroProductModel.CountSell = 2;
                heroProductModel.Type = MarketProductType.Splinter;
                heroProductModel.Cost = new ResourceData{
                    Type = ResourceType.RedDust,
                    Amount = new BigDigit(300)};
                
                SplinterProducts.Add(heroProductModel);
            }
        }

        private void CheckHeroMarketSplinters()
        {
            foreach (var hero in _dictionaries.Heroes)
            {
                var heroMarketKey = $"{hero.Key}HeroMarketProduct";

                var splinterId = $"{hero.Key}Splinter";
                if (!_dictionaries.Splinters.TryGetValue(splinterId, out var splinterModel))
                {
                    Debug.LogError($"Splinter: {splinterId} not found");
                    continue;
                }

                if (_dictionaries.Products.TryGetValue(heroMarketKey, out var product))
                {
                    continue;
                }

                var heroProductModel = new SplinterProductModel(_dictionaries);
                heroProductModel.Id = heroMarketKey;
                heroProductModel.Subject.Id = splinterId;
                heroProductModel.Subject.Amount = 30;

                heroProductModel.CountSell = 2;
                heroProductModel.Type = MarketProductType.Splinter;
                heroProductModel.Cost = new ResourceData{
                    Type = ResourceType.SpaceMask,
                    Amount = new BigDigit(30)};
                
                SplinterProducts.Add(heroProductModel);
            }
        }
    }
}