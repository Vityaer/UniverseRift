using Db.CommonDictionaries;
using Editor.Common;
using Models;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Utils;

namespace Editor.Pages.Items.Set
{
    public class ItemSetPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<ItemSet> _sets => _dictionaries.ItemSets.Select(l => l.Value).ToList();

        public ItemSetPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            ItemSets = _sets.Select(set => new ItemSetModelEditor(set)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var sets = ItemSets.Select(r => new ItemSet
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            EditorUtils.Save(sets);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.ItemSets.Add(id, new ItemSet() { Id = id });
            ItemSets.Add(new ItemSetModelEditor(_dictionaries.ItemSets[id]));
        }

        private void RemoveElements(ItemSetModelEditor light, object b, List<ItemSetModelEditor> lights)
        {
            var targetElement = ItemSets.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.ItemSets.Remove(id);
            ItemSets.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
        NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("ItemSet")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<ItemSetModelEditor> ItemSets = new List<ItemSetModelEditor>();
    }
}
