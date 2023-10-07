using Db.CommonDictionaries;
using Editor.Common;
using Models.City.Mines;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.Buildings.Mines.MineRestrictions
{
    public class MineRestrictionPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<MineRestrictionModel> _mineRestrictions => _dictionaries.MineRestrictions.Select(l => l.Value).ToList();

        public MineRestrictionPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            MineRestrictions = _mineRestrictions.Select(f => new MineRestrictionModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = MineRestrictions.Select(r => r.GetModel()).ToList();
            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.MineRestrictions.Add(id, new MineRestrictionModel() { Id = id });
            MineRestrictions.Add(new MineRestrictionModelEditor(_dictionaries.MineRestrictions[id], _dictionaries));
        }

        private void RemoveElements(MineRestrictionModelEditor light, object b, List<MineRestrictionModelEditor> lights)
        {
            var targetElement = MineRestrictions.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.MineRestrictions.Remove(id);
            MineRestrictions.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("MineRestrictions")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<MineRestrictionModelEditor> MineRestrictions = new List<MineRestrictionModelEditor>();
    }
}
