using Editor.Common;
using Models.City.Markets;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using UnityEngine;
using Utils;

namespace Editor.Pages.Mall.Market
{
    public class MarketPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;

        private List<MarketModel> _markets => _dictionaries.Markets.Select(l => l.Value).ToList();

        public MarketPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Markets = _markets.Select(f => new MarketModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var markets = Markets.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(markets);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Markets.Add(id, new MarketModel(_dictionaries) { Id = id });
            Markets.Add(new MarketModelEditor(_dictionaries.Markets[id], _dictionaries));
        }

        private void RemoveElements(MarketModelEditor light, object b, List<MarketModelEditor> lights)
        {
            var targetElement = Markets.First(e => e == light);
            var id = targetElement.Model.Id;
            _dictionaries.Markets.Remove(id);
            Markets.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Markets")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<MarketModelEditor> Markets = new List<MarketModelEditor>();

        [HorizontalGroup("4")]
        [PropertyOrder(3)]
        [Button("Check markets")]
        private void CheckMarkets()
        {
            foreach (var market in _dictionaries.Markets)
            {
                foreach (var productId in market.Value.Products)
                {
                    if (!_dictionaries.Products.ContainsKey(productId))
                    {
                        Debug.LogError($"Market: {market.Value.Id} product: {productId} not found");
                    }
                }
                
                foreach (var productId in market.Value.Promotions)
                {
                    if (!_dictionaries.Products.ContainsKey(productId))
                    {
                        Debug.LogError($"Market: {market.Value.Id} promo product: {productId} not found");
                    }
                }
            }
        }
    }
}
