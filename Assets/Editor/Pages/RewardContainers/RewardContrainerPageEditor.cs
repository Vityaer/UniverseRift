using Db.CommonDictionaries;
using Editor.Common;
using Models.Rewards;
using Pages.RewardContainers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Pages.BattlePases
{
    internal class RewardContrainerPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<RewardContainerModel> _rewardContainerModels => _dictionaries.RewardContainerModels.Select(l => l.Value).ToList();

        public RewardContrainerPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            EditorRewardContainerModels = _rewardContainerModels.Select(f => new RewardContainerModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = EditorRewardContainerModels.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.RewardContainerModels.Add(id, new RewardContainerModel() { Id = id });
            EditorRewardContainerModels.Add(new RewardContainerModelEditor(_dictionaries.RewardContainerModels[id], _dictionaries));
        }

        private void RemoveElements(RewardContainerModelEditor light, object b, List<RewardContainerModelEditor> lights)
        {
            var targetElement = EditorRewardContainerModels.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.RewardContainerModels.Remove(id);
            EditorRewardContainerModels.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("CampaignChapters")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<RewardContainerModelEditor> EditorRewardContainerModels = new();
    }
}
