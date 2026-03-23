using City.Achievements;
using City.Acievements;
using Editor.Common;
using Models.Achievments;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Utils;

namespace Editor.Pages.Achievments
{
    public class AchievmentModelEditor : BaseModelEditor<AchievmentModel>
    {
        public AchievmentModelEditor(AchievmentModel model)
        {
            m_model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(200)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("Progress Type")]
        [LabelWidth(200)]
        public ProgressType ProgressType
        {
            get => m_model.ProgressType;
            set => m_model.ProgressType = value;
        }

        [ShowInInspector]
        [HorizontalGroup("4")]
        [LabelText("Stages")]
        [LabelWidth(200)]
        public List<AchievmentStageModel> Stages
        {
            get => m_model.Stages;
            set => m_model.Stages = value;
        }

        [ShowInInspector]
        [HorizontalGroup("5")]
        [LabelText("ImplementationName")]
        [LabelWidth(200)]
        [ValueDropdown(nameof(_allAbilitiesName), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public string ImplementationName
        {
            get => m_model.ImplementationName;
            set => m_model.ImplementationName = value;
        }
        [JsonIgnore] private string[] _allAbilitiesName => InheritanceUtils.GetAllTypeNames<GameAchievment>();
    }
}
