using Assets.Editor.Pages.Heroes.RatingUps;
using Editor.Common;
using Models.Heroes.PowerUps;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Pages.Heroes.RatingUps
{
    public class RatingUpPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<RatingUpContainer> _containers => _dictionaries.RatingUpContainers.Select(l => l.Value).ToList();

        public RatingUpPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Containers = _containers.Select(f => new RatingUpModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Containers.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.RatingUpContainers.Add(id, new RatingUpContainer() { Id = id });
            Containers.Add(new RatingUpModelEditor(_dictionaries.RatingUpContainers[id]));
        }

        private void RemoveElements(RatingUpModelEditor light, object b, List<RatingUpModelEditor> lights)
        {
            var targetElement = Containers.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.RatingUpContainers.Remove(id);
            Containers.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Containers")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<RatingUpModelEditor> Containers = new List<RatingUpModelEditor>();
    }
}
