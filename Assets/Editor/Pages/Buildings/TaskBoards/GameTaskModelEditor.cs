using Db.CommonDictionaries;
using Editor.Common;
using Models.Tasks;
using Sirenix.OdinInspector;

namespace Editor.Pages.Buildings.TaskBoards
{
    public class GameTaskModelEditor : BaseModelEditor<GameTaskModel>
    {
        private CommonDictionaries _dictionaries;

        public GameTaskModelEditor(GameTaskModel model, CommonDictionaries dictionaries)
        {
            _dictionaries = dictionaries;
            _model = model;

            model.Reward._dictionaries = _dictionaries;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Task")]
        [LabelWidth(150)]
        public GameTaskModel Task
        {
            get => _model;
            set => _model = value;
        }
    }
}
