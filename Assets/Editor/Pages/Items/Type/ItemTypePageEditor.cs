using Db.CommonDictionaries;
using Editor.Common;
using Editor.Units;
using Models;
using Models.Items;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Editor.Pages.Items.Type
{
    public class ItemTypePageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<ItemType> _itemTypes => _dictionaries.ItemTypes.Select(l => l.Value).ToList();

        public ItemTypePageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            ItemTypes = _itemTypes.Select(f => new ItemTypeModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = ItemTypes.Select(r => new ItemType
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
            _dictionaries.ItemTypes.Add(id, new ItemType() { Id = id });
            ItemTypes.Add(new ItemTypeModelEditor(_dictionaries.ItemTypes[id]));
        }

        private void RemoveElements(ItemTypeModelEditor light, object b, List<ItemTypeModelEditor> lights)
        {
            var targetElement = ItemTypes.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.ItemTypes.Remove(id);
            ItemTypes.Remove(targetElement);
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
        public List<ItemTypeModelEditor> ItemTypes = new List<ItemTypeModelEditor>();
    }
}
