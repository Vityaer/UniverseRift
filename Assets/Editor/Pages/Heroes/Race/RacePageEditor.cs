using Db.CommonDictionaries;
using Editor.Common;
using Models.Heroes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.Heroes.Race
{
    public class RacePageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<RaceModel> _races => _dictionaries.Races.Select(l => l.Value).ToList();

        public RacePageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Races = _races.Select(f => new RaceModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Races.Select(r => new RaceModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Races.Add(id, new RaceModel() { Id = id });
            Races.Add(new RaceModelEditor(_dictionaries.Races[id]));
        }

        private void RemoveElements(RaceModelEditor light, object b, List<RaceModelEditor> lights)
        {
            var targetElement = Races.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Ratings.Remove(id);
            Races.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Rating")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<RaceModelEditor> Races = new List<RaceModelEditor>();
    }
}
