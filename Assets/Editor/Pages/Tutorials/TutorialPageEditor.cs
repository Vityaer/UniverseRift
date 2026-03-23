using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Editor.Common;
using Models.Tutorials;
using Sirenix.OdinInspector;
using Utils;

namespace Editor.Pages.Tutorials
{
    public class TutorialPageEditor : BasePageEditor
    {
        private readonly CommonDictionaries m_dictionaries;
        private List<TutorialModel> m_tutorials => m_dictionaries.Tutorials.Select(l => l.Value).ToList();

        public TutorialPageEditor(CommonDictionaries commonDictionaries)
        {
            m_dictionaries = commonDictionaries;
            Init();
        }
        
        public override void Init()
        {
            base.Init();
            Tutorials = m_tutorials.Select(f => new TutorialModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var units = Tutorials.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(units);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            string id = UnityEngine.Random.Range(0, 99999).ToString();
            m_dictionaries.Tutorials.Add(id, new TutorialModel() { Id = id });
            Tutorials.Add(new TutorialModelEditor(m_dictionaries.Tutorials[id]));
        }

        private void RemoveElements(TutorialModelEditor light, object b, List<TutorialModelEditor> lights)
        {
            TutorialModelEditor targetElement = Tutorials.First(e => e == light);
            string id = targetElement.Model.Id;
            m_dictionaries.Splinters.Remove(id);
            Tutorials.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 5,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Tutorials")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<TutorialModelEditor> Tutorials = new List<TutorialModelEditor>();
    }
}