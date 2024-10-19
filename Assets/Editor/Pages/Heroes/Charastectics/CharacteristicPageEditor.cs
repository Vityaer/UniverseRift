using Db.CommonDictionaries;
using Editor.Common;
using Models.Heroes.HeroCharacteristics.Abstractions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Assets.Editor.Pages.Heroes.Charastectics
{
    public class CharacteristicPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<CharacteristicModel> _containers => _dictionaries.CharacteristicModels.Select(l => l.Value).ToList();

        public CharacteristicPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            CharacteristicModels = _containers.Select(f => new CharacteristicModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = CharacteristicModels.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.CharacteristicModels.Add(id, new CharacteristicModel() { Id = id });
            CharacteristicModels.Add(new CharacteristicModelEditor(_dictionaries.CharacteristicModels[id]));
        }

        private void RemoveElements(CharacteristicModelEditor light, object b, List<CharacteristicModelEditor> lights)
        {
            var targetElement = CharacteristicModels.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.CharacteristicModels.Remove(id);
            CharacteristicModels.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Characteristics")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<CharacteristicModelEditor> CharacteristicModels = new();
    }
}
