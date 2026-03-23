using Common.Db.CommonDictionaries;
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
            m_model = model;
            _commonDictionaries = commonDictionaries;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(50)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }
    }
}
