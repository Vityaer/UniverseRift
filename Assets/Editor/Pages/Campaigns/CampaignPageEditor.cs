using Campaign;
using Editor.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Editor.Pages.Campaigns
{
    public class CampaignPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<CampaignChapterModel> _campaignChapters => _dictionaries.CampaignChapters.Select(l => l.Value).ToList();

        public CampaignPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            CampaignChapters = _campaignChapters.Select(f => new CampaignModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = CampaignChapters.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.CampaignChapters.Add(id, new CampaignChapterModel() { Id = id });
            CampaignChapters.Add(new CampaignModelEditor(_dictionaries.CampaignChapters[id], _dictionaries));
        }

        private void RemoveElements(CampaignModelEditor light, object b, List<CampaignModelEditor> lights)
        {
            var targetElement = CampaignChapters.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.CampaignChapters.Remove(id);
            CampaignChapters.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("CampaignChapters")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<CampaignModelEditor> CampaignChapters = new List<CampaignModelEditor>();
    }
}
