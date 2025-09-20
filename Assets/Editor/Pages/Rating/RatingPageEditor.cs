using Editor.Common;
using Models;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Editor.Pages.Rating
{
    public class RatingPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<RatingModel> _ratings => _dictionaries.Ratings.Select(l => l.Value).ToList();

        public RatingPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Ratings = _ratings.Select(f => new RatingModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Ratings.Select(r => new RatingModel
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
            _dictionaries.Ratings.Add(id, new RatingModel() { Id = id });
            Ratings.Add(new RatingModelEditor(_dictionaries.Ratings[id]));
        }

        private void RemoveElements(RatingModelEditor light, object b, List<RatingModelEditor> lights)
        {
            var targetElement = Ratings.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Ratings.Remove(id);
            Ratings.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Rating")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<RatingModelEditor> Ratings = new List<RatingModelEditor>();
    }
}
