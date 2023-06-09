using Db.CommonDictionaries;
using Editor.Common;
using Editor.Pages.Heroes.Race;
using Models.Heroes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.Heroes.Resistances
{
    public class ResistancesPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<ResistanceModel> _resistances => _dictionaries.Resistances.Select(l => l.Value).ToList();

        public ResistancesPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Resistances = _resistances.Select(f => new ResistanceModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var resistances = Resistances.Select(r => new ResistanceModel
            {
                Id = r.Id
            }).ToList();

            EditorUtils.Save(resistances);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Resistances.Add(id, new ResistanceModel() { Id = id });
            Resistances.Add(new ResistanceModelEditor(_dictionaries.Resistances[id]));
        }

        private void RemoveElements(ResistanceModelEditor light, object b, List<ResistanceModelEditor> lights)
        {
            var targetElement = Resistances.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Ratings.Remove(id);
            Resistances.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Resistances")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<ResistanceModelEditor> Resistances = new List<ResistanceModelEditor>();
    }
}
