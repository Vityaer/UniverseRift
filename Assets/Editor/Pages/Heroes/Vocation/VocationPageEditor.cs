using Db.CommonDictionaries;
using Editor.Common;
using Editor.Pages.Heroes.Vocation;
using Models.Heroes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Pages.Heroes.Vocation
{

    public class VocationPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<VocationModel> _vocations => _dictionaries.Vocations.Select(l => l.Value).ToList();

        public VocationPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            Vocations = _vocations.Select(f => new VocationModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Vocations.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Vocations.Add(id, new VocationModel() { Id = id });
            Vocations.Add(new VocationModelEditor(_dictionaries.Vocations[id]));
        }

        private void RemoveElements(VocationModelEditor light, object b, List<VocationModelEditor> lights)
        {
            var targetElement = Vocations.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.Ratings.Remove(id);
            Vocations.Remove(targetElement);
        }

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Vocations")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<VocationModelEditor> Vocations = new List<VocationModelEditor>();
    }

}
