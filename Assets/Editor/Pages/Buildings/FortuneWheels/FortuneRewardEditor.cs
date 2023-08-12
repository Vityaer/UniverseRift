using Db.CommonDictionaries;
using Editor.Common;
using Models.City.FortuneRewards;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Pages.Buildings.FortuneWheels
{
    public class FortuneRewardEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<FortuneRewardModel> _fortuneRewards => _dictionaries.FortuneRewardModels.Select(l => l.Value).ToList();

        public FortuneRewardEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            FortuneResourceRewards = _fortuneRewards
                .Where(product => product is FortuneResourseRewardModel)
                .Select(product => product as FortuneResourseRewardModel)
                .ToList();

            FortuneItemRewards = _fortuneRewards
                .Where(product => product is FortuneItemRewardModel)
                .Select(product => product as FortuneItemRewardModel)
                .ForEach(product => product.Subject.CommonDictionaries = _dictionaries)
                .ToList();

            DataExist = true;
        }

        public override void Save()
        {
            var products = new List<FortuneRewardModel>();
            products.AddRange(FortuneResourceRewards);
            products.AddRange(FortuneItemRewards);
            EditorUtils.Save(products);
            base.Save();
        }



        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = false, NumberOfItemsPerPage = 20)]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Fortune Resource Rewards")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<FortuneResourseRewardModel> FortuneResourceRewards = new List<FortuneResourseRewardModel>();

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = false,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("4")]
        [LabelText("Fortune Items Rewards")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<FortuneItemRewardModel> FortuneItemRewards = new List<FortuneItemRewardModel>();

        protected override void AddElement()
        {
            var fortuneItem = new FortuneItemRewardModel(_dictionaries);
            FortuneItemRewards.Add(fortuneItem);
        }

        private void RemoveElements(FortuneItemRewardModel light, object b, List<FortuneItemRewardModel> lights)
        {
            var targetElement = FortuneItemRewards.First(e => e == light);
            FortuneItemRewards.Remove(targetElement);
        }
    }
}
