using Campaign;
using Editor.Common;
using Editor.Pages.Campaigns;
using Models.Achievments;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Editor.Pages.Achievments
{
    public class AchievementPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<AchievmentModel> _achievments => _dictionaries.Achievments.Select(l => l.Value).ToList();

        public AchievementPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Achievments = _achievments.Select(f => new AchievmentModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var achievments = Achievments.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(achievments);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Achievments.Add(id, new AchievmentModel() { Id = id });
            Achievments.Add(new AchievmentModelEditor(_dictionaries.Achievments[id]));
        }

        private void RemoveElements(AchievmentModelEditor light, object b, List<AchievmentModelEditor> lights)
        {
            var targetElement = Achievments.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Achievments.Remove(id);
            Achievments.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Achievments")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<AchievmentModelEditor> Achievments = new List<AchievmentModelEditor>();
    }
}
