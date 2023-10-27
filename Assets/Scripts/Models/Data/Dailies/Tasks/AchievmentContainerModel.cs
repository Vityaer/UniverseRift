using Db.CommonDictionaries;
using Models.Achievments;
using Models.City.Markets;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Data.Dailies.Tasks
{
    public class AchievmentContainerModel : BaseModel
    {
        [NonSerialized] public CommonDictionaries CommonDictionaries;

        private string[] _allAchievmentId => CommonDictionaries.Achievments.Values.Select(r => r.Id).ToArray();
        [ValueDropdown(nameof(_allAchievmentId), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public List<string> TaskIds = new List<string>();

        public AchievmentContainerModel(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }
    }
}
