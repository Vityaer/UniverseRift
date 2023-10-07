using Db.CommonDictionaries;
using Editor.Common;
using Models.City.Mines;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.City.Mines
{
    public class MinePageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<MineModel> _mines => _dictionaries.Mines.Select(l => l.Value).ToList();

        public MinePageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Mines = _mines.Select(f => new MineModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Mines.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Mines.Add(id, new MineModel() { Id = id });
            Mines.Add(new MineModelEditor(_dictionaries.Mines[id]));
        }

        private void RemoveElements(MineModelEditor light, object b, List<MineModelEditor> lights)
        {
            var targetElement = Mines.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Mines.Remove(id);
            Mines.Remove(targetElement);
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
        public List<MineModelEditor> Mines = new List<MineModelEditor>();
    }
}
