using Db.CommonDictionaries;
using Editor.Common;
using Models.Achievments;
using Models.City.Markets;
using Models.Data.Dailies.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Editor.Pages.DailyTasks
{
    public class AchievmentContainerModelEditor : BaseModelEditor<AchievmentContainerModel>
    {
        private CommonDictionaries _dictionaries;

        public AchievmentContainerModelEditor(AchievmentContainerModel model, CommonDictionaries dictionaries)
        {
            _model = model;
            _dictionaries = dictionaries;

        }
        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelWidth(150)]
        [LabelText("Id")]
        public AchievmentContainerModel Model
        {
            get => _model;
            set => _model = value;
        }
    }
}
