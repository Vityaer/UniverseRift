using Editor.Common;
using Models.City.TravelCircle;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Editor.Pages.Buildings.TravelRaceCampaigns
{
    public class TravelRaceCircleEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<TravelRaceModel> _travelCampaigns => _dictionaries.TravelRaceCampaigns.Select(l => l.Value).ToList();

        public TravelRaceCircleEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            TravelCampaigns = _travelCampaigns.Select(f => new TravelRaceModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var gameTasks = TravelCampaigns.Select(r => r.GetModel()).ToList();
            EditorUtils.Save(gameTasks);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.TravelRaceCampaigns.Add(id, new TravelRaceModel() { Id = id });
            TravelCampaigns.Add(new TravelRaceModelEditor(_dictionaries.TravelRaceCampaigns[id], _dictionaries));
        }

        private void RemoveElements(TravelRaceModelEditor light, object b, List<TravelRaceModelEditor> lights)
        {
            var targetElement = TravelCampaigns.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.GameTaskModels.Remove(id);
            TravelCampaigns.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Travels")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<TravelRaceModelEditor> TravelCampaigns = new List<TravelRaceModelEditor>();
    }
}
