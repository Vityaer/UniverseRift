using Editor.Common;
using Models.Achievments;
using Models.City.Markets;
using Models.Data.Dailies.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;

namespace Editor.Pages.DailyTasks
{
    public class AchievmentContainerModelEditor : BaseModelEditor<AchievmentContainerModel>
    {
        private CommonDictionaries _dictionaries;

        public AchievmentContainerModelEditor(AchievmentContainerModel model, CommonDictionaries dictionaries)
        {
            m_model = model;
            _dictionaries = dictionaries;
            m_model.CommonDictionaries = _dictionaries;

        }
        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelWidth(150)]
        [LabelText("Id")]
        public AchievmentContainerModel Model
        {
            get => m_model;
            set => m_model = value;
        }
    }
}
