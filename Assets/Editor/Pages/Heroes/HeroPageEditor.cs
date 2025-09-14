using Db.CommonDictionaries;
using Editor.Common;
using Models.Heroes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Utils;

namespace Editor.Pages.Heroes
{
    public class HeroPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<HeroModel> _heroes => _dictionaries.Heroes.Select(l => l.Value).ToList();

        public HeroPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;

            Heroes = _heroes.Select(heroModel => new HeroModelEditor(heroModel, _dictionaries)).ToList();
            foreach (var hero in Heroes)
            {
                if (hero.GetModel().General.HeroId == null)
                {
                    hero.GetModel().General.HeroId = hero.Id;
                }
            }

            Init();
        }

        public override void Save()
        {
            var heroes = Heroes.Select(hero => hero.GetModel()).ToList();

            EditorUtils.Save(heroes);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.Heroes.Add(id, new HeroModel()
            {
                Id = id,

            });
            Heroes.Add(new HeroModelEditor(_dictionaries.Heroes[id], _dictionaries));
        }

        private void RemoveElements(HeroModelEditor light, object b, List<HeroModelEditor> lights)
        {
            var targetElement = Heroes.First(e => e == light);
            Heroes.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true, NumberOfItemsPerPage = 5,
    CustomAddFunction = nameof(AddElement), CustomRemoveElementFunction = nameof(RemoveElements))]
        [HorizontalGroup("3")]
        [LabelText("Heroes")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<HeroModelEditor> Heroes = new List<HeroModelEditor>();
    }
}
