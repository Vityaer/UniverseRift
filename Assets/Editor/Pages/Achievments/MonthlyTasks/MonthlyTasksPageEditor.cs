using Editor.Common;
using Models.Achievments;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Editor.Pages.Achievments.MonthlyTasks
{
    public class MonthlyTasksPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<MonthlyTasksModel> _monthlyTasks => _dictionaries.MonthlyTasks.Select(l => l.Value).ToList();

        public MonthlyTasksPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            MonthlyTasks = _monthlyTasks.Select(f => new MonthlyTasksModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var achievments = MonthlyTasks.Select(r => new MonthlyTasksModel
            {
                Id = r.Id
            }).ToList();

            EditorUtils.Save(achievments);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.MonthlyTasks.Add(id, new MonthlyTasksModel() { Id = id });
            MonthlyTasks.Add(new MonthlyTasksModelEditor(_dictionaries.MonthlyTasks[id], _dictionaries));
        }

        private void RemoveElements(MonthlyTasksModelEditor light, object b, List<MonthlyTasksModelEditor> lights)
        {
            var targetElement = MonthlyTasks.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.MonthlyTasks.Remove(id);
            MonthlyTasks.Remove(targetElement);
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
        public List<MonthlyTasksModelEditor> MonthlyTasks = new List<MonthlyTasksModelEditor>();
    }
}
