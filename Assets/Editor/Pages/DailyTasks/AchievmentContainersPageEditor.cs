using Db.CommonDictionaries;
using Editor.Common;
using Models.Data.Dailies.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.DailyTasks
{
    public class AchievmentContainersPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;

        private List<AchievmentContainerModel> _achievmentContainers => _dictionaries.AchievmentContainers.Select(l => l.Value).ToList();

        public AchievmentContainersPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            AchievmentContainers = _achievmentContainers.Select(f => new AchievmentContainerModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var achievments = AchievmentContainers.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(achievments);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.AchievmentContainers.Add(id, new AchievmentContainerModel(_dictionaries) { Id = id });
            AchievmentContainers.Add(new AchievmentContainerModelEditor(_dictionaries.AchievmentContainers[id], _dictionaries));
        }

        private void RemoveElements(AchievmentContainerModelEditor light, object b, List<AchievmentContainerModelEditor> lights)
        {
            var targetElement = AchievmentContainers.First(e => e == light);
            var id = targetElement.Model.Id;
            _dictionaries.Achievments.Remove(id);
            AchievmentContainers.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("AchievmentContainers")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<AchievmentContainerModelEditor> AchievmentContainers = new List<AchievmentContainerModelEditor>();
    }
}
