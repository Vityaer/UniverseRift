using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Editor.Common;
using Models.Inventory.Splinters;
using Sirenix.OdinInspector;
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
            Splinters = _splinters.Select(f => new SplinterModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Splinters.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Splinters.Add(id, new SplinterModel() { Id = id });
            Splinters.Add(new SplinterModelEditor(_dictionaries.Splinters[id], _dictionaries));
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
            NumberOfItemsPerPage = 5,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Splinters")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<SplinterModelEditor> Splinters = new List<SplinterModelEditor>();

        [PropertyOrder(3)]
        [Button("Add Splinter missing heroes")]
        public void AddMissingHeroes()
        {
            foreach (var hero in _dictionaries.Heroes)
            {
                var key = $"{hero.Key}Splinter";
                var findSplinter = Splinters.FindIndex(splinter => splinter.Id == key);
                if (findSplinter != -1)
                    continue;

                var splinterModel = new SplinterModel()
                {
                    Id = key,
                    ModelId = hero.Key,
                    SplinterType = SplinterType.Hero,
                    RequireCount = 30
                };
                
                _dictionaries.Splinters.Add(key, splinterModel);
                Splinters.Add(new SplinterModelEditor(_dictionaries.Splinters[key], _dictionaries));
            }
        }
    }
}