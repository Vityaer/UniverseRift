using Editor.Common;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Editor.Pages
{
    public class HeroPageEditor : BasePageEditor
    {
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

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true, NumberOfItemsPerPage = 5,
    CustomAddFunction = nameof(AddElement), CustomRemoveElementFunction = nameof(RemoveElements))]
        [HorizontalGroup("3")]
        [LabelText("Heroes")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<InfoHero> Heroes;
    }
}
