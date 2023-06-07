using Db.CommonDictionaries;
using Editor.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.Splinters
{
    public class SplinterPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;

        private List<SplinterModel> _splinters => _dictionaries.Splinters.Select(l => l.Value).ToList();

        public SplinterPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Splinters = _splinters.Select(f => new SplinterModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Splinters.Select(r => new SplinterModel
            {
                Id = r.Id
            }).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Splinters.Add(id, new SplinterModel() { Id = id });
            Splinters.Add(new SplinterModelEditor(_dictionaries.Splinters[id]));
        }

        private void RemoveElements(SplinterModelEditor light, object b, List<SplinterModelEditor> lights)
        {
            var targetElement = Splinters.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Splinters.Remove(id);
            Splinters.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Splinters")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<SplinterModelEditor> Splinters = new List<SplinterModelEditor>();
    }
}
