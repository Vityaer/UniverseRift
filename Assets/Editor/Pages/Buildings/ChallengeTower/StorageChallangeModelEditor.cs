using Editor.Common;
using Models.City.Misc;
using Sirenix.OdinInspector;

namespace Pages.City.ChallengeTower
{
    [HideReferenceObjectPicker]
    public class StorageChallangeModelEditor : BaseModelEditor<StorageChallengeModel>
    {
        public StorageChallangeModelEditor(StorageChallengeModel model)
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
    }
}
