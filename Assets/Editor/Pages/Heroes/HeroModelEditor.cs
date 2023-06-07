using Assets.Scripts.Models.Heroes;
using Editor.Common;
using Models.City.Mines;
using Models.Heroes;
using Sirenix.OdinInspector;

namespace Editor.Pages.Heroes
{
    [HideReferenceObjectPicker]
    public class HeroModelEditor : BaseModelEditor<HeroModel>
    {
        public HeroModelEditor(HeroModel model)
        {
            _model = model;
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

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("GeneralInfoHero")]
        [LabelWidth(50)]
        public GeneralInfoHero GeneralInfo
        {
            get => _model.General;
            set => _model.General = value;
        }
    }
}
