using Editor.Common;
using Editor.Pages.Locations;
using Models.Fights.Misc;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Assets.Editor.Pages.Locations
{
    internal class LocationPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<LocationModel> _locations => _dictionaries.Locations.Select(l => l.Value).ToList();

        public LocationPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Locations = _locations.Select(f => new LocationModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Locations.Select(r => new LocationModel
            {
                Id = r.Id,
            }).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Locations.Add(id, new LocationModel() { Id = id });
            Locations.Add(new LocationModelEditor(_dictionaries.Locations[id]));
        }

        private void RemoveElements(LocationModelEditor light, object b, List<LocationModelEditor> lights)
        {
            var targetElement = Locations.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Locations.Remove(id);
            Locations.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Locations")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<LocationModelEditor> Locations = new List<LocationModelEditor>();
    }
}
