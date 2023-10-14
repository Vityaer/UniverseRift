using Db.CommonDictionaries;
using Editor.Common;
using Editor.Pages.DailyRewards;
using Models.Data.Dailies;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.DailyTasks
{
    public class DailyTasksPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<DailyRewardModel> _achievments => _dictionaries.DailyRewardDatas.Select(l => l.Value).ToList();

        public DailyTasksPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            DailyRewardDatas = _achievments.Select(f => new DailyRewardModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var achievments = DailyRewardDatas.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(achievments);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.DailyRewardDatas.Add(id, new DailyRewardModel() { Id = id });
            DailyRewardDatas.Add(new DailyRewardModelEditor(_dictionaries.DailyRewardDatas[id]));
        }

        private void RemoveElements(DailyRewardModelEditor light, object b, List<DailyRewardModelEditor> lights)
        {
            var targetElement = DailyRewardDatas.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Achievments.Remove(id);
            DailyRewardDatas.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("DailyRewardDatas")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<DailyRewardModelEditor> DailyRewardDatas = new List<DailyRewardModelEditor>();
    }
}
