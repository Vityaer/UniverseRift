using Campaign;
using Editor.Common;
using Models.City.Forges;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;

namespace Pages.Buildings.Forge
{
    public class ForgeModelEditor : BaseModelEditor<ForgeModel>
    {
        private CommonDictionaries _commonDictionaries;
        private string[] _allSets => _commonDictionaries.ItemSets.Select(c => c.Value).Select(r => r.Id).ToArray();

        public ForgeModelEditor(ForgeModel model, CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
            _model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(250)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Sets")]
        [LabelWidth(250)]
        [ValueDropdown(nameof(_allSets), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public List<string> Sets
        {
            get => _model.Sets;
            set => _model.Sets = value;
        }
    }
}
