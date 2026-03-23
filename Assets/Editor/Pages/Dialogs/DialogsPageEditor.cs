using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Editor.Common;
using Models.Dialogs;
using Sirenix.OdinInspector;
using Utils;

namespace Editor.Pages.Dialogs
{
    public class DialogsPageEditor : BasePageEditor
    {
        private readonly CommonDictionaries m_dictionaries;
        private List<DialogModel> Dialogs => m_dictionaries.Dialogs.Select(l => l.Value).ToList();

        public DialogsPageEditor(CommonDictionaries commonDictionaries)
        {
            m_dictionaries = commonDictionaries;

            EditorDialogs = Dialogs.Select(heroModel => new DialogModelEditor(heroModel)).ToList();
            Init();
        }

        public override void Save()
        {
            var heroes = EditorDialogs.Select(hero => hero.GetModel()).ToList();

            EditorUtils.Save(heroes);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            DialogModel model = new DialogModel()
            {
                Id = id,

            };
            m_dictionaries.Dialogs.Add(id, model);
            EditorDialogs.Add(new DialogModelEditor(model));
        }

        private void RemoveElements(DialogModelEditor light, object b, List<DialogModelEditor> lights)
        {
            var targetElement = EditorDialogs.First(e => e == light);
            var id = targetElement.Model.Id;
            m_dictionaries.Dialogs.Remove(id);
            EditorDialogs.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true, NumberOfItemsPerPage = 5,
            CustomAddFunction = nameof(AddElement), CustomRemoveElementFunction = nameof(RemoveElements))]
        [HorizontalGroup("3")]
        [LabelText("Dialogs")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<DialogModelEditor> EditorDialogs = new List<DialogModelEditor>();
    }
}