using Db.CommonDictionaries;
using Editor.Common;
using Models;
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

        public HeroPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            var hero = new InfoHero();
            Heroes.Add(hero);
        }

        private void RemoveElements(InfoHero light, object b, List<InfoHero> lights)
        {
            var targetElement = Heroes.First(e => e == light);
            Heroes.Remove(targetElement);
        }

        public override void Save()
        {
            var heroes = Heroes.Select(hero => new HeroModel(hero)).ToList();

            EditorUtils.Save(heroes);
            base.Save();
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true, NumberOfItemsPerPage = 5,
    CustomAddFunction = nameof(AddElement), CustomRemoveElementFunction = nameof(RemoveElements))]
        [HorizontalGroup("3")]
        [LabelText("Heroes")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<InfoHero> Heroes = new List<InfoHero>();
    }
}
