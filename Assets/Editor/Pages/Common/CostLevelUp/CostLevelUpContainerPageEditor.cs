using City.TrainCamp;
using Db.CommonDictionaries;
using Editor.Common;
using Models.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Pages.Heroes.CostLevelUp
{
    public class CostLevelUpContainerPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<CostLevelUpContainer> _costs => _dictionaries.CostContainers.Select(l => l.Value).ToList();

        public CostLevelUpContainerPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            CostContainers = _costs.Select(f => new CostLevelUpContainerModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var heroCostLevelUps = CostContainers.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(heroCostLevelUps);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.CostContainers.Add(id, new CostLevelUpContainer() { Id = id });
            CostContainers.Add(new CostLevelUpContainerModelEditor(_dictionaries.CostContainers[id]));
        }

        private void RemoveElements(CostLevelUpContainerModelEditor light, object b, List<CostLevelUpContainerModelEditor> lights)
        {
            var targetElement = CostContainers.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.CostContainers.Remove(id);
            CostContainers.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Hero Cost Level Ups")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<CostLevelUpContainerModelEditor> CostContainers = new List<CostLevelUpContainerModelEditor>();
    }
}
