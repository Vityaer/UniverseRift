using Db.CommonDictionaries;
using Editor.Common;
using Models.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Editor.Pages.Buildings.TaskBoards
{
    public class GameTaskEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private List<GameTaskModel> _gameTasks => _dictionaries.GameTaskModels.Select(l => l.Value).ToList();

        public GameTaskEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            GameTasks = _gameTasks.Select(f => new GameTaskModelEditor(f, _dictionaries)).ToList();
            DataExist = true;
        }

        public override void Save()
        {
            var gameTasks = GameTasks.Select(r => r.GetModel()).ToList();
            EditorUtils.Save(gameTasks);
            base.Save();
        }

        protected override void AddElement()
        {
            base.AddElement();
            var id = UnityEngine.Random.Range(0, 99999).ToString();
            _dictionaries.GameTaskModels.Add(id, new GameTaskModel() { Id = id });
            GameTasks.Add(new GameTaskModelEditor(_dictionaries.GameTaskModels[id], _dictionaries));
        }

        private void RemoveElements(GameTaskModelEditor light, object b, List<GameTaskModelEditor> lights)
        {
            var targetElement = GameTasks.First(e => e == light);
            var id = targetElement.Task.Id;
            _dictionaries.GameTaskModels.Remove(id);
            GameTasks.Remove(targetElement);
        }

        [ShowInInspector]
        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveElements), CustomAddFunction = nameof(AddElement))]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Game Tasks")]
        [PropertyOrder(2)]
        [Searchable(FilterOptions = SearchFilterOptions.ValueToString)]
        public List<GameTaskModelEditor> GameTasks = new List<GameTaskModelEditor>();
    }
}
