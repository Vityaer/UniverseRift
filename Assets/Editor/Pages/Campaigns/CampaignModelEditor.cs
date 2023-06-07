using Editor.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Editor.Pages.Campaigns
{
    [HideReferenceObjectPicker]
    public class CampaignModelEditor : BaseModelEditor<CampaignChapterModel>
    {
        public CampaignModelEditor(CampaignChapterModel model)
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
        [LabelText("Name")]
        [LabelWidth(50)]
        public string Name
        {
            get => _model.Name;
            set => _model.Name = value;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Mission")]
        [LabelWidth(50)]
        public List<CampaignMission> Missions
        {
            get => _model.Missions;
            set => _model.Missions = value;
        }
    }
}
