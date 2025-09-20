using Editor.Common;
using Models.Misc.Avatars;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Utils;

namespace Assets.Editor.Pages.Avatars
{
    public class AvatarPageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<AvatarModel> _avatars => _dictionaries.AvatarModels.Select(l => l.Value).ToList();

        public AvatarPageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            AvatarEditorModels = _avatars.Select(f => new AvatarModelEditor(f)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var heroCostLevelUps = AvatarEditorModels.Select(r => r.GetModel()).ToList();

            EditorUtils.Save(heroCostLevelUps);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.AvatarModels.Add(id, new AvatarModel() { Id = id });
            AvatarEditorModels.Add(new AvatarModelEditor(_dictionaries.AvatarModels[id]));
        }

        private void RemoveElements(AvatarModelEditor light, object b, List<AvatarModelEditor> lights)
        {
            var targetElement = AvatarEditorModels.First(e => e == light);
            var id = targetElement.Id;
            _dictionaries.AvatarModels.Remove(id);
            AvatarEditorModels.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Avatars")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<AvatarModelEditor> AvatarEditorModels = new List<AvatarModelEditor>();
    }
}
