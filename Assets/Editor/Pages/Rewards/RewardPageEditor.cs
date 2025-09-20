using Editor.Common;
using Pages.Rewards;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using UIController.Rewards;
using Utils;

namespace Editor.Pages.Rewards
{
    public class RewardPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<RewardModel> _rewards => _dictionaries.Rewards.Select(l => l.Value).ToList();

        public RewardPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Rewards = _rewards.Select(f => new RewardModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var rewards = Rewards.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(rewards);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Rewards.Add(id, new RewardModel() { Id = id });
            Rewards.Add(new RewardModelEditor(_dictionaries.Rewards[id], _dictionaries));
        }

        private void RemoveElements(RewardModelEditor light, object b, List<RewardModelEditor> lights)
        {
            var targetElement = Rewards.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Rewards.Remove(id);
            Rewards.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Rewards")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<RewardModelEditor> Rewards = new List<RewardModelEditor>();
    }
}
