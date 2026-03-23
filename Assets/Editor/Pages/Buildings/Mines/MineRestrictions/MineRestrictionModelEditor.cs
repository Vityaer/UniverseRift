using Editor.Common;
using Models.City.Mines;
using Sirenix.OdinInspector;
using System.Linq;
using Common.Db.CommonDictionaries;

namespace Editor.Pages.Buildings.Mines.MineRestrictions
{
    public class MineRestrictionModelEditor : BaseModelEditor<MineRestrictionModel>
    {
        private CommonDictionaries _commonDictionaries;

        public MineRestrictionModelEditor(MineRestrictionModel model, CommonDictionaries commonDictionaries)
        {
            m_model = model;
            _commonDictionaries = commonDictionaries;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(150)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("MineId")]
        [LabelWidth(150)]
        [ValueDropdown(nameof(_allMinesName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string MineId
        {
            get => m_model.MineId;
            set => m_model.MineId = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("MaxCount")]
        [LabelWidth(150)]
        public int MaxCount
        {
            get => m_model.MaxCount;
            set => m_model.MaxCount = value;
        }

        private string[] _allMinesName => _commonDictionaries.Mines.Values.Select(r => r.Id).ToArray();
    }
}
