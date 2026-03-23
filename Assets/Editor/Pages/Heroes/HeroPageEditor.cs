using Editor.Common;
using Models.Heroes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using UnityEditor;
using Utils;

namespace Editor.Pages.Heroes
{
    public class HeroPageEditor : BasePageEditor
    {
        private readonly CommonDictionaries m_dictionaries;
        private List<HeroModel> _heroes => m_dictionaries.Heroes.Select(l => l.Value).ToList();

        public HeroPageEditor(CommonDictionaries commonDictionaries)
        {
            m_dictionaries = commonDictionaries;

            Heroes = _heroes.Select(heroModel => new HeroModelEditor(heroModel, m_dictionaries)).ToList();
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
            m_dictionaries.Heroes.Add(id, new HeroModel()
            {
                Id = id,

            });
            Heroes.Add(new HeroModelEditor(m_dictionaries.Heroes[id], m_dictionaries));
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
