using Db.CommonDictionaries;
using Editor.Common;
using Models.Achievments;
using Sirenix.OdinInspector;

namespace Editor.Pages.Achievments
{
    public class MonthlyTasksModelEditor : BaseModelEditor<MonthlyTasksModel>
    {
        private CommonDictionaries _commonDictionaries;

        public MonthlyTasksModelEditor(MonthlyTasksModel model, CommonDictionaries commonDictionaries)
        {
            _model = model;
            _commonDictionaries = commonDictionaries;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(50)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }
    }
}
