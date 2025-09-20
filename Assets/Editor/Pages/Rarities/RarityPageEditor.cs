using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Editor.Common;
using Models;
using Sirenix.OdinInspector;
using Utils;

namespace Editor.Units
{
    public class RarityPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<RarityModel> _rarities => _dictionaries.Rarities.Select(l => l.Value).ToList();

        public RarityPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Rarities = _rarities.Select(f => new RarityEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Rarities.Select(r => new RarityModel
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
            _dictionaries.Rarities.Add(id, new RarityModel() { Id = id });
            Rarities.Add(new RarityEditor(_dictionaries.Rarities[id]));
        }

        private void RemoveElements(RarityEditor light, object b, List<RarityEditor> lights)
        {
            var targetElement = Rarities.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Rarities.Remove(id);
            Rarities.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Rarity")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<RarityEditor> Rarities = new List<RarityEditor>();
    }
}