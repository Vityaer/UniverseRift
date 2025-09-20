using Editor.Common;
using Models.City.Hires;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Editor.Pages.Buildings.Hires
{
    public class HireContainerPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<HireContainerModel> _hireContainers => _dictionaries.HireContainerModels.Select(l => l.Value).ToList();

        public HireContainerPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            HireContainers = _hireContainers.Select(f => new HireContainerModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = HireContainers.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.HireContainerModels.Add(id, new HireContainerModel() { Id = id });
            HireContainers.Add(new HireContainerModelEditor(_dictionaries.HireContainerModels[id]));
        }

        private void RemoveElements(HireContainerModelEditor light, object b, List<HireContainerModelEditor> lights)
        {
            var targetElement = HireContainers.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.HireContainerModels.Remove(id);
            HireContainers.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Hire containers")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<HireContainerModelEditor> HireContainers = new();
    }
}
