using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Models.Misc.Helps;
using Sirenix.OdinInspector;
using Utils;

namespace Editor.Common.Pages.Misc.WhereGetPages
{
    public class WhereGetResourcePageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;


        public WhereGetResourcePageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }
        
        public override void Init()
        {
            base.Init();
            HelpResources = _dictionaries.HelpResourceModels.Select(l => l.Value).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            EditorUtils.Save(HelpResources);
            base.Save();
        }
        
        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            var model = new HelpResourceModel() { Id = id };
            _dictionaries.HelpResourceModels.Add(id, model);
            HelpResources.Add(model);
        }

        private void RemoveElements(HelpResourceModel light, object b, List<HelpResourceModel> lights)
        {
            var targetElement = HelpResources.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Markets.Remove(id);
            HelpResources.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Help resources")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<HelpResourceModel> HelpResources = new List<HelpResourceModel>();
    }
}